namespace Athena.Application.Features.NotificatioinFeatures.Dtos
{
    public class SignalRConnectionDto : IDto
    {
        public Guid Id { get; set; }
        public string? ConnectionId { get; set; }
        public Guid? BusinessId { get; set; }
        public IEnumerable<string>? Groups { get; set; }
    }
}
