using System.Runtime.InteropServices.ComTypes;
namespace Athena.Application.Features.StudentFeatures.Teachers.Dtos
{
    public class TeachersDto : IDto
    {
        public List<AssignedTeacherDto>? AssignedTeachers { get; set; }
        public List<ExploreTeacherDto>? ExploreTeachers { get; set; }
    }
}