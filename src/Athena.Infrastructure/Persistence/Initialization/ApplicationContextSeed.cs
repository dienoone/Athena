using Athena.Infrastructure.Persistence.Context;

namespace Athena.Infrastructure.Persistence.Initialization
{
    public static class ApplicationContextSeed
    {
        public static async Task CoursesSeed(ApplicationDbContext dbContext, CancellationToken cancellationToken)
        {
            Console.WriteLine("Courses Has Data? :: " + await dbContext.Courses.AnyAsync(cancellationToken: cancellationToken));
            if (!(await dbContext.Courses.AnyAsync(cancellationToken: cancellationToken)))
            {

                List<Course> courses = new List<Course>
                {
                    new Course("اللغه العربيه"),
                    new Course("اللغه الانجليزيه"),
                    new Course("الكيمياء"),
                    new Course("الفيزياء"),
                    new Course("الرياضيات"),
                };
                await dbContext.Courses.AddRangeAsync(courses, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
                Console.WriteLine("Courses");
            }
        }

        public static async Task LevelClassificationSeed(ApplicationDbContext dbContext, CancellationToken cancellationToken)
        {
            if (!await dbContext.Levels.AnyAsync(cancellationToken))
            {
                EducationClassification educationClassification1 = new("لا يوجد");
                EducationClassification educationClassification2 = new("علمى");
                EducationClassification educationClassification3 = new("ادبى");
                EducationClassification educationClassification4 = new("علمى علوم");
                EducationClassification educationClassification5 = new("علمى رياضه");

                await dbContext.Classifications.AddAsync(educationClassification1, cancellationToken);
                await dbContext.Classifications.AddAsync(educationClassification2, cancellationToken);
                await dbContext.Classifications.AddAsync(educationClassification3, cancellationToken);
                await dbContext.Classifications.AddAsync(educationClassification4, cancellationToken);
                await dbContext.Classifications.AddAsync(educationClassification5, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);

                Level level1 = new("الصف الاول الثانوى", 1);
                Level level2 = new("الصف الثانى الثانوى", 2);
                Level level3 = new("الصف الثالث الثانوى", 3);

                await dbContext.Levels.AddAsync(level1, cancellationToken);
                await dbContext.Levels.AddAsync(level2, cancellationToken);
                await dbContext.Levels.AddAsync(level3, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);

                LevelClassification levelClassification1 = new(level1.Id, educationClassification1.Id);
                LevelClassification levelClassification2 = new(level2.Id, educationClassification2.Id);
                LevelClassification levelClassification3 = new(level2.Id, educationClassification3.Id);
                LevelClassification levelClassification4 = new(level3.Id, educationClassification3.Id);
                LevelClassification levelClassification5 = new(level3.Id, educationClassification4.Id);
                LevelClassification levelClassification6 = new(level3.Id, educationClassification5.Id);

                await dbContext.LevelClassifications.AddAsync(levelClassification1, cancellationToken);
                await dbContext.LevelClassifications.AddAsync(levelClassification2, cancellationToken);
                await dbContext.LevelClassifications.AddAsync(levelClassification3, cancellationToken);
                await dbContext.LevelClassifications.AddAsync(levelClassification4, cancellationToken);
                await dbContext.LevelClassifications.AddAsync(levelClassification5, cancellationToken);
                await dbContext.LevelClassifications.AddAsync(levelClassification6, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
                Console.WriteLine("LevelClassification");
            }
        }

    }
}
