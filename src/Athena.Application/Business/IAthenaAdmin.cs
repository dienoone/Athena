namespace Athena.Application.Business
{
    public interface IAthenaAdmin : ITransientService
    {
        Task<Guid> GetAthenBusinessId();
    }
}
