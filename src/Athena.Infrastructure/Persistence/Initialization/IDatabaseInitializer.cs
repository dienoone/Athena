namespace Athena.Infrastructure.Persistence.Initialization
{
    public interface IDatabaseInitializer
    {
        Task InitializeDatabasesAsync(CancellationToken cancellationToken);
    }
}
