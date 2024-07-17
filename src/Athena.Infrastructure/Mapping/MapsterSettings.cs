using Athena.Application.Features.DashboardFeatures.Courses.Dtos;
using Athena.Application.Features.DashboardFeatures.ExamTypes.Dtos;
using Athena.Application.Features.DashboardFeatures.Levels.Dtos;
using Athena.Application.Features.DashboardFeatures.Notifications.Dtos;
using Athena.Application.Features.DashboardFeatures.Years.Dtos;
using Athena.Application.Features.NotificatioinFeatures.Dtos;
using Athena.Application.Features.StudentFeatures.Teachers.Dtos;
using Athena.Application.Features.TeacherFeatures.Exams.Dto;
using Athena.Application.Features.TeacherFeatures.Groups.Dtos;
using Athena.Application.Features.TeacherFeatures.HeadQuarters.Dtos;
using Athena.Application.Features.TeacherFeatures.TeacherStudents.Dtos;
using Athena.Application.Features.TeacherFeatures.Years.Dtos;
using Mapster;

namespace Athena.Infrastructure.Mapping
{
    public class MapsterSettings
    {
        public static void Configure()
        {
            // here we will define the type conversion / Custom-mapping
            // More details at https://github.com/MapsterMapper/Mapster/wiki/Custom-mapping

            // This one is actually not necessary as it's mapped by convention
            TypeAdapterConfig<Level, LevelDetailDto>.NewConfig().Map(dest => dest.Classifications, src => src.LevelClassifications);
            TypeAdapterConfig<LevelClassification, LevelClassificationsDto>.NewConfig()
                .Map(dest => dest.LevelClassificationId, src => src.Id)
                .Map(dest => dest.Name, src => src.EducationClassification.Name);

            TypeAdapterConfig<Course, CourseDto>.NewConfig();

            TypeAdapterConfig<TeacherCourseLevel, Application.Features.DashboardFeatures.Teachers.Dto.TeacherCourseLevelDto>.NewConfig()
                .Map(dest => dest.TeacherCourseLevelId, src => src.Id)
                .Map(dest => dest.LevelName, src => src.Level.Name);

            TypeAdapterConfig<ExamType, ExamTypeDto>.NewConfig();


            TypeAdapterConfig<DashboardYear, DashboardYearDto>.NewConfig()
                .Map(dest => dest.Start, src => src.Start)
                .Map(dest => dest.End, src => src.Start + 1)
                .Map(dest => dest.StartDate, src => src.CreatedAt);

            #region HeadQuarters:

            TypeAdapterConfig<HeadQuarter, HeadQuarterListDto>.NewConfig()
                .Map(dest => dest.Phones, src => src.HeadQuarterPhones);

            TypeAdapterConfig<HeadQuarter, HeadQuarterPhoneDto>.NewConfig();

            TypeAdapterConfig<HeadQuarter, HeadQuarterDetailDto>.NewConfig()
                .Map(dest => dest.Phones, src => src.HeadQuarterPhones);

            TypeAdapterConfig<HeadQuarter, HeadQuaerteRequiredDto>.NewConfig();

            TypeAdapterConfig<HeadQuarter, TeacherHeadquartersDetailsDto>.NewConfig();

            TypeAdapterConfig<HeadQuarterPhone, TeacherHeadquarterPhonesDetailsDto>.NewConfig();

            #endregion

            #region Years:

            TypeAdapterConfig<Year, YearListDto>.NewConfig()
                .Map(dest => dest.Start, src => src.DashboardYear.Start)
                .Map(dest => dest.End, src => src.DashboardYear.Start + 1);

            TypeAdapterConfig<Year, YearDetailDto>.NewConfig()
                .Map(dest => dest.Start, src => src.DashboardYear.Start)
                .Map(dest => dest.End, src => src.DashboardYear.Start + 1)
                .Map(dest => dest.Levels, src => src.TeacherCourseLevelYears);

            // Eh da?
            TypeAdapterConfig<TeacherCourseLevelYear, TeacherCourseLevelYearDto>.NewConfig()
                .Map(dest => dest.TeacherCourseLevelId, src => src.TeacherCourseLevel.Id)
                .Map(dest => dest.LevelName, src => src.TeacherCourseLevel.Level.Name)
                .Map(dest => dest.Semsters, src => src.TeacherCourseLevelYearSemsters);

            TypeAdapterConfig<TeacherCourseLevelYearSemster, TeacherCourseLevelYearSemsterDto>.NewConfig();
            
            TypeAdapterConfig<TeacherCourseLevelYear, LevelsRequiredDto>.NewConfig()
                .Map(dest => dest.TeacherCourseLevelYearId, src => src.Id)
                .Map(dest => dest.LevelName, src => src.TeacherCourseLevel.Level.Name);

            #endregion

            #region Groups:

            TypeAdapterConfig<Group, GroupListDto>.NewConfig()
                .Map(dest => dest.Level, src => src.TeacherCourseLevelYear.TeacherCourseLevel.Level.Name)
                .Map(dest => dest.HeadQuarter, src => src.HeadQuarter.Name)
                .Map(dest => dest.StudentsCount, src => src.GroupStudents.Count);

            TypeAdapterConfig<Group, GroupDetailDto>.NewConfig()
                .Map(dest => dest.TeacherCourseLevelId, src => src.TeacherCourseLevelYearId)
                .Map(dest => dest.Level, src => src.TeacherCourseLevelYear.TeacherCourseLevel.Level.Name)
                .Map(dest => dest.HeadQuarterId, src => src.HeadQuarterId)
                .Map(dest => dest.HeadQuarter, src => src.HeadQuarter.Name)
                .Map(dest => dest.StudentsCount, src => src.GroupStudents.Count)
                .Map(dest => dest.YearState, src => src.TeacherCourseLevelYear.Year.YearState)
                .Map(dest => dest.GroupScaduals, src => src.GroupScaduals);

            TypeAdapterConfig<GroupScadual, GroupScadualDto>.NewConfig();

            TypeAdapterConfig<GroupScadual, DaysOfAttendanceDto>.NewConfig();

            TypeAdapterConfig<TeacherCourseLevelYear, LevelsRequiredDto>.NewConfig()
                .Map(dest => dest.TeacherCourseLevelYearId, src => src.Id)
                .Map(dest => dest.LevelName, src => src.TeacherCourseLevel.Level.Name);

            #endregion

            #region TeacherStudents:

            // TeacherStudentsListDto
            TypeAdapterConfig<TeacherCourseLevelYearStudent, TeacherStudentYearLevelListDto>.NewConfig()
                .Map(dest => dest.LevelName, src => src.TeacherCourseLevelYear.TeacherCourseLevel.Level.Name)
                .Map(dest => dest.Students, src => src.Student);

            TypeAdapterConfig<Student, TeacherStudentYearLevelStudentDto>.NewConfig()
                .Map(dest => dest.LevelName, src => src.LevelClassification.Level.Name)
                .Map(dest => dest.EducationClassificationName, src => src.LevelClassification.EducationClassification.Name)
                .Map(dest => dest.ImagePath, src => src.Image);

           /* TypeAdapterConfig<TeacherCourseLevelYear, YearWithGroupsDto>.NewConfig()
                .Map(dest => dest.Start, src => src.Year.DashboardYear.Start)
                .Map(dest => dest.End, src => src.Year.DashboardYear.Start + 1)
                .Map(dest => dest.Groups, src => src.Groups);*/
            TypeAdapterConfig<Group, GroupsRequiredDto>.NewConfig();


            #endregion

            #region Exams:

            TypeAdapterConfig<Exam, ExamListDto>.NewConfig()
                .Map(dest => dest.Level, src => src.TeacherCourseLevelYear.TeacherCourseLevel.Level.Name)
                .Map(dest => dest.ExamType, src => src.ExamType.Name);

            TypeAdapterConfig<Group, GroupsForCreateExamDto>.NewConfig();
            TypeAdapterConfig<ExamGroup, ExamGroupDto>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Group.Name);


            TypeAdapterConfig<TeacherCourseLevelYear, TeacherCourseLevelYearRequiredToCreateExamDto>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.TeacherCourseLevel.Level.Name);

            #endregion

            #region Notifications:

            TypeAdapterConfig<NotificationType, NotificationTypeDto>.NewConfig()
                .Map(dest => dest.Templates, src => src.NotificationTypeTemplates);

            TypeAdapterConfig<NotificationTypeTemplate, NotificationTypeTemplateDto>.NewConfig();

            TypeAdapterConfig<NotificationRecipient, NotificationDto>.NewConfig()
                .Map(dest => dest.Id, src => src.NotificationId)
                .Map(dest => dest.Type, src => src.Notification.NotificationType.Type)
                .Map(dest => dest.NotificationLabel, src => src.Notification.NotificationLabel)
                .Map(dest => dest.EntityId, src => src.Notification.EntityId)
                .Map(dest => dest.Image, src => src.Notification.Image)
                .Map(dest => dest.CreatedOn, src => src.Notification.CreatedOn);
                

            #endregion


            #region StudentsApp:

            #region ExploreTeachers:

            TypeAdapterConfig<TeacherCourseLevelYear, ExploreTeacherYearDto>.NewConfig()
                .Map(dest => dest.YearState, src => src.Year.YearState)
                .Map(dest => dest.Start, src => src.Year.DashboardYear.Start)
                .Map(dest => dest.End, src => src.Year.DashboardYear.Start + 1);

            TypeAdapterConfig<Group, ExploreTeacherYearGroupDto>.NewConfig()
                .Map(dest => dest.HeadQuarterDto, src => src.HeadQuarter)
                .Map(dest => dest.Scaduals, src => src.GroupScaduals);

            TypeAdapterConfig<HeadQuarter, ExploreTeacherYearGroupHeadQuarterDto>.NewConfig()
                .Map(dest => dest.Phones, src => src.HeadQuarterPhones);

            TypeAdapterConfig<HeadQuarterPhone, ExploreTeacherYearGroupHeadQuarterPhoneDto>.NewConfig();

            TypeAdapterConfig<GroupScadual, ExploreTeacherYearGroupScadualDto>.NewConfig();

            TypeAdapterConfig<HeadQuarter, TeacherHeadquartersDetailsDto>.NewConfig()
                .Map(dest => dest.Phones, src => src.HeadQuarterPhones);

            TypeAdapterConfig<HeadQuarterPhone, TeacherHeadquarterPhonesDetailsDto>.NewConfig();

            #endregion

            #endregion

        }
    }

}
