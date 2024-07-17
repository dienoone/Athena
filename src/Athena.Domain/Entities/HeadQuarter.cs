namespace Athena.Domain.Entities
{
    public class HeadQuarter : AuditableEntity, IAggregateRoot
    {
        public string Name { get; private set; } = default!;
        public string City { get; private set; } = default!;
        public string? Region { get; private set; } 
        public string Street { get; private set; } = default!;
        public string Building { get; private set; } = default!;
        public Guid TeacherId { get; private set; }
        public virtual Teacher Teacher { get; private set; } = default!;

        public virtual ICollection<HeadQuarterPhone> HeadQuarterPhones { get; private set; } = default!;
        public virtual ICollection<Group> Groups { get; private set; } = default!;

        public HeadQuarter(string name, string city, string? region, string street, string building, Guid teacherId, Guid businessId)
        {
            Name = name;
            City = city;
            Region = region;
            Street = street;
            Building = building;
            TeacherId = teacherId;
            BusinessId = businessId;
        }

        public HeadQuarter Update(string? name, string? city, string? region, string? street, string? building)
        {
            if (name is not null && Name?.Equals(name) is not true) Name = name;
            if (city is not null && City?.Equals(city) is not true) City = city;
            if (region is not null && Region?.Equals(region) is not true) Region = region;
            if (street is not null && Street?.Equals(street) is not true) Street = street;
            if (building is not null && Building?.Equals(building) is not true) Building = building;
            return this;
        }
    }
}
