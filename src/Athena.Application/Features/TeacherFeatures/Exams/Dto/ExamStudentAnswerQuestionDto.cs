namespace Athena.Application.Features.TeacherFeatures.Exams.Dto
{
    public class ExamStudentAnswerQuestionDto : QuestionDetailDto, IDto
    {
        public string? StudentAnswer { get; set; }
        public double StudentDegree { get; set; }
        public bool IsCorrected { get; set; }
        public bool IsAnswered { get; set; }

    }
}
