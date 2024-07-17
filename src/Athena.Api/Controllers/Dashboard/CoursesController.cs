using Athena.Application.Features.DashboardFeatures.Courses.Commands;
using Athena.Application.Features.DashboardFeatures.Courses.Dtos;
using Athena.Application.Features.DashboardFeatures.Courses.Queries;

namespace Athena.Api.Controllers.Dashboard
{
    public class CoursesController : VersionNeutralApiDashboardGroupController
    {
        [HttpGet]
        [AllowAnonymous]
        [OpenApiOperation("Get courses.", "")]
        public async Task<List<CourseDto>> GetAsync()
        {
            return await Mediator.Send(new GetCoursesRequest());
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [OpenApiOperation("Get course details.", "")]
        public async Task<CourseDto> GetAsync(Guid id)
        {
            return await Mediator.Send(new GetCourseByIdRequest(id));
        }

        [HttpPost]
        [MustHavePermission(AAction.Create, AResource.Courses)]
        [OpenApiOperation("Create a new Course.", "")]
        public async Task<Guid> CreateAsync(CreateCourseRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("{id:guid}")]
        [MustHavePermission(AAction.Update, AResource.Courses)]
        [OpenApiOperation("Update a course.", "")]
        public async Task<ActionResult<Guid>> UpdateAsync(UpdateCourseRequest request, Guid id)
        {
            return id != request.Id
           ? BadRequest()
           : Ok(await Mediator.Send(request));
        }


        [HttpDelete("{id:guid}")]
        [MustHavePermission(AAction.Delete, AResource.Courses)]
        [OpenApiOperation("Delete a course.", "")]
        public async Task<Guid> DeleteAsync(Guid id)
        {
            return await Mediator.Send(new DeleteCourseRequest(id));
        }
    }
}
