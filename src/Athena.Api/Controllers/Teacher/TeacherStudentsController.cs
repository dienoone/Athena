using Athena.Application.Features.TeacherFeatures.TeacherStudents.Commands;
using Athena.Application.Features.TeacherFeatures.TeacherStudents.Dtos;
using Athena.Application.Features.TeacherFeatures.TeacherStudents.Queries;

namespace Athena.Api.Controllers.Teacher
{
    public class TeacherStudentsController : VersionNeutralApiTeacherGroupController
    {
        [HttpGet("Code/{code}")]
        [OpenApiOperation("Get Student by Code.", "")]
        public async Task<StudentByCodeDto> GetByCodeAsync(string code)
        {
            return await Mediator.Send(new GetStudentByCodeRequest(code));
        }

        [HttpPost("Assign")]
        [OpenApiOperation("Assign Student For Teacher.")]
        public async Task<Guid> AssignStudentForTeacherAsync(AssignStudentForTeacherRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpGet]
        [OpenApiOperation("Get Students List.", "")]
        public async Task<StudentsListRequestDto> GetStudentsListAsync()
        {
            return await Mediator.Send(new GetTeacherStudentsListRequest());
        }

        //GetJoinRequestsRequest
        [HttpGet("requests")]
        [OpenApiOperation("Get Join-requests-list.", "")]
        public async Task<List<JoinRequestLevelDto>> GetJoinRequestsListAsync()
        {
            return await Mediator.Send(new GetJoinRequestsRequest());
        }

        //GetJoinRequestDetailByRequestIdRequest
        [HttpGet("requests/{id:guid}")]
        [OpenApiOperation("Get join-request-detail-by-requestId.", "")]
        public async Task<JoinRequestDetailDto> GetJoinRequestDetailAsync(Guid id)
        {
            return await Mediator.Send(new GetJoinRequestDetailByRequestIdRequest(id));
        }

        //AcceptJoinRequestByRequestIdRequest
        [HttpPost("requests/{id:guid}")]
        [OpenApiOperation("Get accept-join-request-by-requestId.", "")]
        public async Task<Guid> AcceptJoinRequestAsync(Guid id)
        {
            return await Mediator.Send(new AcceptJoinRequestByRequestIdRequest(id));
        }

        //RejectJoinRequestByRequestIdRequest
        [HttpPut("requests/{id:guid}")]
        [OpenApiOperation("Get reject-join-request-by-requestId.", "")]
        public async Task<ActionResult<Guid>> RejectJoinRequestAsync(RejectJoinRequestByRequestIdRequest request, Guid id)
        {
            return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
        }

        [HttpGet("Info/{id:guid}")]
        [OpenApiOperation("Get Student Card Detail.", "")]
        public async Task<StudentCardDto> GetStudentCardAsync(Guid id)
        {
            return await Mediator.Send(new GetStudentCardByStudentIdRequest(id));
        }

        [HttpDelete("Info/{id:guid}")]
        [OpenApiOperation("Delete student-from-teacher by teacherCourseLevelYearStudentId")]
        public async Task<Guid> DeleteStudentFromTeacherAsync(Guid id)
        {
            return await Mediator.Send(new DeleteStudentFromTeacherByTeacherCourseLevelYearStudentIdRequest(id));
        }

        // GetExamResultByTeacherCourseLevelYearStudentIdRequest
        [HttpGet("info/examsResult/{id:guid}")]
        [OpenApiOperation("Get-exam-results-by-teacherCourseLevelYearStudentId.", "")]
        public async Task<List<ExamResultsDto>> GetExamResultsAsync(Guid id)
        {
            return await Mediator.Send(new GetExamResultByTeacherCourseLevelYearStudentIdRequest(id));
        }

        [HttpPut("Group")]
        [OpenApiOperation("Update Group For Student", "")]
        public async Task<Guid> UpdateGroupForStudent(UpdateStudentGroupRequest request)
        {
            return await Mediator.Send(request);
        }
    }
}
