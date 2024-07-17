using Athena.Application.Features.DashboardFeatures.Classification.Commands;
using Athena.Application.Features.DashboardFeatures.Testing;

namespace Athena.Api.Controllers.Dashboard
{
    public class TestController : VersionNeutralApiDashboardGroupController
    {
        [HttpPut("{id:guid}")]
        [AllowAnonymous]
        [OpenApiOperation("EndExam.", "")]
        public async Task<Guid> UpdateAsync(Guid id)
        {
            return await Mediator.Send(new EndExamByIdRequest(id));
        }
    }
}
