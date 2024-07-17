﻿using Athena.Infrastructure.Auditing;
using Athena.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using System.Data;

namespace Athena.Infrastructure.Persistence.Context
{
    public abstract class BaseDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, ApplicationRoleClaim, IdentityUserToken<string>>
    {
        protected readonly ICurrentUser _currentUser;
        private readonly ISerializerService _serializer;
        private readonly IEventPublisher _events;

        protected BaseDbContext(DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer, IEventPublisher events)
            : base(options)
        {
            _currentUser = currentUser;
            _serializer = serializer;
            _events = events;
        }

        // Used by Dapper
        public IDbConnection Connection => Database.GetDbConnection();

        public DbSet<Trail> AuditTrails => Set<Trail>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //QueryFilters need to be applied before base.OnModelCreating
            modelBuilder.AppendGlobalQueryFilter<ISoftDelete>(s => s.DeletedOn == null);
            //modelBuilder.AppendGlobalQueryFilter<IAuditableEntity>(s => s.BusinessId == _currentUser.GetBusinessId());

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // TODO: We want this only for development probably... maybe better make it configurable in logger.json config?
           // optionsBuilder.EnableSensitiveDataLogging();

            // If you want to see the sql queries that efcore executes:

            // Uncomment the next line to see them in the output window of visual studio
            // optionsBuilder.LogTo(m => System.Diagnostics.Debug.WriteLine(m), Microsoft.Extensions.Logging.LogLevel.Information);

            // Or uncomment the next line if you want to see them in the console
            // optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var auditEntries = HandleAuditingBeforeSaveChanges(_currentUser.GetUserId());

            int result = await base.SaveChangesAsync(cancellationToken);

            await HandleAuditingAfterSaveChangesAsync(auditEntries, cancellationToken);

            await SendDomainEventsAsync();

            return result;
        }

        private List<AuditTrail> HandleAuditingBeforeSaveChanges(Guid userId)
        {
            // Specify the desired time zone (Egypt Standard Time)
            TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");

            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = userId;
                        entry.Entity.LastModifiedBy = userId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone); ;
                        entry.Entity.LastModifiedBy = userId;
                        break;

                    case EntityState.Deleted:
                        if (entry.Entity is ISoftDelete softDelete)
                        {
                            softDelete.DeletedBy = userId;
                            softDelete.DeletedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone); ;
                            entry.State = EntityState.Modified;
                        }

                        break;
                }
            }

            ChangeTracker.DetectChanges();

            var trailEntries = new List<AuditTrail>();
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>()
                .Where(e => e.State is EntityState.Added or EntityState.Deleted or EntityState.Modified)
                .ToList())
            {
                var trailEntry = new AuditTrail(entry, _serializer)
                {
                    TableName = entry.Entity.GetType().Name,
                    UserId = userId
                };
                trailEntries.Add(trailEntry);
                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        trailEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        trailEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            trailEntry.TrailType = TrailType.Create;
                            trailEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            trailEntry.TrailType = TrailType.Delete;
                            trailEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified && entry.Entity is ISoftDelete && property.OriginalValue == null && property.CurrentValue != null)
                            {
                                trailEntry.ChangedColumns.Add(propertyName);
                                trailEntry.TrailType = TrailType.Delete;
                                trailEntry.OldValues[propertyName] = property.OriginalValue;
                                trailEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            else if (property.IsModified && property.OriginalValue?.Equals(property.CurrentValue) == false)
                            {
                                trailEntry.ChangedColumns.Add(propertyName);
                                trailEntry.TrailType = TrailType.Update;
                                trailEntry.OldValues[propertyName] = property.OriginalValue;
                                trailEntry.NewValues[propertyName] = property.CurrentValue;
                            }

                            break;
                    }
                }
            }

            foreach (var auditEntry in trailEntries.Where(e => !e.HasTemporaryProperties))
            {
                AuditTrails.Add(auditEntry.ToAuditTrail());
            }

            return trailEntries.Where(e => e.HasTemporaryProperties).ToList();
        }

        private Task HandleAuditingAfterSaveChangesAsync(List<AuditTrail> trailEntries, CancellationToken cancellationToken = new())
        {
            if (trailEntries == null || trailEntries.Count == 0)
            {
                return Task.CompletedTask;
            }

            foreach (var entry in trailEntries)
            {
                foreach (var prop in entry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        entry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        entry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }

                AuditTrails.Add(entry.ToAuditTrail());
            }

            return SaveChangesAsync(cancellationToken);
        }

        private async Task SendDomainEventsAsync()
        {
            var entitiesWithEvents = ChangeTracker.Entries<IEntity>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Count > 0)
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var domainEvents = entity.DomainEvents.ToArray();
                entity.DomainEvents.Clear();
                foreach (var domainEvent in domainEvents)
                {
                    await _events.PublishAsync(domainEvent);
                }
            }
        }
    }
}
