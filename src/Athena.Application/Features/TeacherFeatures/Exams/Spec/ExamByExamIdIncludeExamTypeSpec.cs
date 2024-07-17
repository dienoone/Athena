﻿namespace Athena.Application.Features.TeacherFeatures.Exams.Spec
{
    public class ExamByExamIdIncludeExamTypeSpec : Specification<Exam>, ISingleResultSpecification
    {
        public ExamByExamIdIncludeExamTypeSpec(Guid Id) =>
            Query.Where(e => e.Id == Id)
                .Include(e => e.ExamType);
    }
}