namespace Athena.Application.Features.TeacherFeatures.TeacherStudents.Commands
{
    public record DeleteStudentFromTeacherByTeacherCourseLevelYearStudentIdRequest(Guid Id) : IRequest<Guid>;

    public class DeleteStudentFromTeacherByTeacherCourseLevelYearStudentIdRequestHandler : IRequestHandler<DeleteStudentFromTeacherByTeacherCourseLevelYearStudentIdRequest, Guid>
    {
        private readonly IRepository<TeacherCourseLevelYearStudent> _teacherCourseLevelYearStudentRepo;
        private readonly IStringLocalizer<DeleteStudentFromTeacherByTeacherCourseLevelYearStudentIdRequestHandler> _t;
        
        public DeleteStudentFromTeacherByTeacherCourseLevelYearStudentIdRequestHandler(
            IRepository<TeacherCourseLevelYearStudent> teacherCourseLevelYearStudentRepo, 
            IStringLocalizer<DeleteStudentFromTeacherByTeacherCourseLevelYearStudentIdRequestHandler> t)
        {
            _teacherCourseLevelYearStudentRepo = teacherCourseLevelYearStudentRepo;
            _t = t;
        }

        public async Task<Guid> Handle(DeleteStudentFromTeacherByTeacherCourseLevelYearStudentIdRequest request, CancellationToken cancellationToken)
        {
            var student = await _teacherCourseLevelYearStudentRepo.GetByIdAsync(request.Id, cancellationToken);
            _ = student ?? throw new NotFoundException(_t["TeacherCourseLevelYearStudent {0} Not Found!", request.Id]);

            await _teacherCourseLevelYearStudentRepo.DeleteAsync(student, cancellationToken);
            return request.Id;
        }
    }
}
