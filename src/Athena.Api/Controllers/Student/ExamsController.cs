using Athena.Application.Features.StudentFeatures.Exams.Dtos;
using Athena.Application.Features.StudentFeatures.Exams.Queries;

namespace Athena.Api.Controllers.Student
{
    public class ExamsController : VersionNeutralApiStudentGroupController
    {
        [HttpGet]
        [OpenApiOperation("get all exams.", "")]
        public async Task<ExamsRequestDto> GetAsync()
        {
            return await Mediator.Send(new GetExamsRequest());
        }

        [HttpGet("filter")]
        [OpenApiOperation("get filter exams.", "")]
        public async Task<List<ExamListDto>> GetFilterListAsync([FromQuery]FilterExamsByStartDateAndCourseRequest request)
        {
            return await Mediator.Send(new FilterExamsByStartDateAndCourseRequest(request.DateTime, request.CourseId));
        }
        
        [HttpGet("upcomming/{id:guid}")]
        [OpenApiOperation("get upcomming exam.", "")]
        public async Task<UpcomingExamDto> GetUpcommingExamAsync(Guid id)
        {
            return await Mediator.Send(new GetUpcomingExamByIdRequest(id));
        }

        //GetExamResultsByExamIdRequest
        [HttpGet("result")]
        [OpenApiOperation("get exam results", "")]
        public async Task<ExamResultDto> GetExamResultsAsync(Guid id)
        {
            return await Mediator.Send(new GetExamResultsByExamIdRequest(id));
        }
    }
}
