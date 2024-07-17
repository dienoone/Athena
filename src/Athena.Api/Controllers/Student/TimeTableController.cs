using Athena.Application.Features.StudentFeatures.TimeTable.Dtos;
using Athena.Application.Features.StudentFeatures.TimeTable.Queries;

namespace Athena.Api.Controllers.Student
{
    public class TimeTableController : VersionNeutralApiStudentGroupController
    {
        [HttpGet]
        [OpenApiOperation("Get time table.", "")]
        public async Task<TimeTableRequestDto> GetAsync()
        {
            return await Mediator.Send(new GetTimeTableReqeust());
        }
    }
}
