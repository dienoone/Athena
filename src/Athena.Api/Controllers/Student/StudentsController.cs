using Athena.Application.Features.StudentFeatures.Students;
using Athena.Application.Features.StudentFeatures.Students.Dtos;

namespace Athena.Api.Controllers.Student
{
    public class StudentsController : VersionNeutralApiStudentGroupController
    {
        [HttpGet]
        [OpenApiOperation("Get Students List", "")]
        public async Task<List<StudentsListDto>> GetListAsync()
        {
            return await Mediator.Send(new GetAllStudentsRequest());
        }
    }
}
