using Athena.Application.Features.StudentFeatures.Teachers.Commands;
using Athena.Application.Features.StudentFeatures.Teachers.Dtos;
using Athena.Application.Features.StudentFeatures.Teachers.Queries;

namespace Athena.Api.Controllers.Student
{
    public class TeachersController : VersionNeutralApiStudentGroupController
    {
        [HttpGet]
        [OpenApiOperation("get all teachers.", "")]
        public async Task<TeachersDto> GetListAsync()
        {
            return await Mediator.Send(new GetTeachersRequest());
        }

        [HttpGet("{id:guid}")]
        [OpenApiOperation("get teacher details.", "")]
        public async Task<TeacherDetailsDto> GetTeacherDetailsAsync(Guid id)
        {
            return await Mediator.Send(new GetTeacherDetailsRequest(id));
        }

        [HttpGet("explore")]
        [OpenApiOperation("get explore teachers.", "")]
        public async Task<ExploreTeachersDto> GetExploreTeachersAsync()
        {
            return await Mediator.Send(new GetExploreTeachersRequest());
        }

        [HttpGet("explore/{id:guid}")]
        [OpenApiOperation("get explore teacher.", "")]
        public async Task<ExploreTeacherDetailDto> GetExploreTeacherAsync(Guid id)
        {
            return await Mediator.Send(new GetExploreTeacherDetailRequest(id));
        }

        //GetExploreTeacherYearForJoinByTeacherIdRequest
        [HttpGet("explore/year/{id:guid}")]
        [OpenApiOperation("get explore-teacher-year-groups-for-join.", "")]
        public async Task<ExploreTeacherYearForJoinDto> GetExploreTeacherYearForJoinAsync(Guid id)
        {
            return await Mediator.Send(new GetExploreTeacherYearForJoinByTeacherIdRequest(id));
        }

        [HttpGet("explore/review/{id:guid}")]
        [OpenApiOperation("get join-request-for-review.", "")]
        public async Task<ExploreTeacherRequestReviewDto> GetJoinRequestAsync(Guid id)
        {
            return await Mediator.Send(new GetJoinRequestByRequestIdRequest(id));
        }

        [HttpPut("explore/review/{id:guid}")]
        [OpenApiOperation("get join-request-for-review.", "")]
        public async Task<ActionResult<Guid>> UpdateJoinRequestAsync(UpdateJointRequestByJoinIdRequest request, Guid id)
        {
            return id != request.Id
           ? BadRequest()
           : Ok(await Mediator.Send(request));
        }

        //JoinTeacherRequest
        [HttpPost("explore")]
        [OpenApiOperation("get join-teacher-request.", "")]
        public async Task<Guid> JoinTeacherAsync(JoinTeacherRequest request)
        {
            return await Mediator.Send(request);
        }

        //DeleteJoinRequestByRequestIdRequest
        [HttpDelete("explore/{id:guid}")]
        [OpenApiOperation("get delete-join-teacher-request-by-requestId.", "")]
        public async Task<Guid> DelteJoinTeacherByIdAsync(Guid id)
        {
            return await Mediator.Send(new DeleteJoinRequestByRequestIdRequest(id));
        }

    }
}
