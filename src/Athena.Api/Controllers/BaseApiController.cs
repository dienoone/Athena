using MediatR;

namespace Athena.Api.Controllers
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private ISender _mediator = null!;
        private IPublisher _publisher = null!;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
        protected IPublisher Publisher => _publisher ??= HttpContext.RequestServices.GetRequiredService<IPublisher>();
    }
}
