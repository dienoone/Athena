using Athena.Application.Features.TeacherFeatures.Exams.Commands.Create;
using Athena.Application.Features.TeacherFeatures.Exams.Commands.Delete;
using Athena.Application.Features.TeacherFeatures.Exams.Commands.Update;
using Athena.Application.Features.TeacherFeatures.Exams.Dto;
using Athena.Application.Features.TeacherFeatures.Exams.Queries;

namespace Athena.Api.Controllers.Teacher
{
    public class ExamsController : VersionNeutralApiTeacherGroupController
    {
        [HttpGet("required")]
        [OpenApiOperation("Get ExamTypes-Yearlevels.", "")]
        public async Task<CreateExamRequiredDto> GetRequiredAsync()
        {
            return await Mediator.Send(new GetCreateExamRequiredRequest());
        }

        [HttpGet("groups/{id:guid}")]
        [OpenApiOperation("Get Groups-By-TeacherCourseLevelYearId.", "")]
        public async Task<List<GroupsForCreateExamDto>> GetGroupsAsync(Guid id)
        {
            return await Mediator.Send(new GetGroupsForCreateExamRequest(id));
        }

        [HttpPost]
        [OpenApiOperation("Create Exam.", "")]
        public async Task<Guid> CreateExam(CreateExamRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpGet]
        [OpenApiOperation("Get Exam List.", "")]
        public async Task<List<ExamListDto>> GetExamListAsync()
        {
            return await Mediator.Send(new GetExamListRequest());
        }

        [HttpGet("{id:guid}")]
        [OpenApiOperation("Get Exam Detail.", "")]
        public async Task<ExamDetailDto> GetExamDetailAsync(Guid id)
        {
            return await Mediator.Send(new GetExamDetailByIdRequest(id));
        }

        [HttpPut("{id:guid}")]
        [OpenApiOperation("update exam.", "")]
        public async Task<ActionResult<Guid>> UpdateExamAsync(UpdateExamRequest request, Guid id)
        {
            return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
        }

        [HttpDelete("{id:guid}")]
        [OpenApiOperation("delete exam.", "")]
        public async Task<Guid> DeleteExamAsync(Guid id)
        {
            return await Mediator.Send(new DeleteExamRequest(id));
        }

        [HttpGet("group/{id:guid}")]
        [OpenApiOperation("get available groups for update exam.", "")]
        public async Task<List<GroupsForCreateExamDto>> GetAvailableGroupsAsync(Guid id)
        {
            return await Mediator.Send(new GetAvailableGroupsForUpdateExamRequest(id));
        }

        [HttpDelete("group/{id:guid}")]
        [OpenApiOperation("delete a exam group.", "")]
        public async Task<Guid> DeleteExamGroupAsync(Guid id)
        {
            return await Mediator.Send(new DeleteExamGroupReqeust(id));
        }

        [HttpPost("group/{id:guid}")]
        [OpenApiOperation("create new exam group", "")]
        public async Task<ActionResult<Guid>> CreateExamGroupAsync(CreateExamGroupsRequest request, Guid id)
        {
            return id != request.Id
           ? BadRequest()
           : Ok(await Mediator.Send(request));
        }
        
        [HttpPut("section/{id:guid}")]
        [OpenApiOperation("update section.")]
        public async Task<ActionResult<Guid>> UpdateSectionAsync(UpdateExamSectionRequest request, Guid id)
        {
            return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
        }

        //CreateSectionQuestionReqeust
        [HttpPost("section/question/{id:guid}")]
        [OpenApiOperation("create new question for section", "")]
        public async Task<ActionResult<Guid>> CreateQuestionAsync(CreateSectionQuestionReqeust request, Guid id)
        {
            return id != request.SectionId
           ? BadRequest()
           : Ok(await Mediator.Send(request));
        }

        // UpdateQuestionByQuestionIdRequest
        [HttpPut("section/question/{id:guid}")]
        [OpenApiOperation("update question by id", "")]
        public async Task<ActionResult<Guid>> UpdateQuestionAsync(UpdateQuestionByQuestionIdRequest request, Guid id)
        {
            return id != request.Id
           ? BadRequest()
           : Ok(await Mediator.Send(request));
        }

        #region Delete :

        [HttpDelete("section/{id:guid}")]
        [OpenApiOperation("delete section.", "")]
        public async Task<Guid> DeleteSectionAsync(Guid id)
        {
            return await Mediator.Send(new DeleteExamSectionRequest(id));
        }

        [HttpDelete("section/sectionimage/{id:guid}")]
        [OpenApiOperation("delete section image.", "")]
        public async Task<Guid> DeleteSectionImageAsync(Guid id)
        {
            return await Mediator.Send(new DeleteSectionImageByIdRequest(id));
        }

        [HttpDelete("section/question/{id:guid}")]
        [OpenApiOperation("delete question.", "")]
        public async Task<Guid> DeleteQuestionAsync(Guid id)
        {
            return await Mediator.Send(new DeleteQuestionByIdRequest(id));
        }

        [HttpDelete("section/question/questionchoice/{id:guid}")]
        [OpenApiOperation("delete question choice.", "")]
        public async Task<Guid> DeleteQuestionChoiceAsync(Guid id)
        {
            return await Mediator.Send(new DeleteQuestionChoiceByIdRequest(id));
        }

        [HttpDelete("section/question/questionimage/{id:guid}")]
        [OpenApiOperation("delete question image.", "")]
        public async Task<Guid> DeleteQuestionImageAsync(Guid id)
        {
            return await Mediator.Send(new DeleteQuestionImageByIdRequest(id));
        }

        //DeleteQuestionChoiceImageByQuestioinChoiceIdRequest
        [HttpDelete("section/question/questionchoice/image/{id:guid}")]
        [OpenApiOperation("delete question choice image by questionChoiceId.", "")]
        public async Task<Guid> DeleteQuestionChoiceImageAsync(Guid id)
        {
            return await Mediator.Send(new DeleteQuestionChoiceImageByQuestioinChoiceIdRequest(id));
        }
        #endregion


        [HttpGet("results/{id:guid}")]
        [OpenApiOperation("Get Exam Results.", "")]
        public async Task<ExamResultDto> GetExamResultsAsync(Guid id)
        {
            return await Mediator.Send(new GetExamResultsByIdRequest(id));
        }

        [HttpGet("results/correctRoom/{id:guid}")]
        [OpenApiOperation("Get Exam Correct Room.", "")]
        public async Task<ExamCorrectionRoomDto> GetExamCorrectRoomAsync(Guid id)
        {
            return await Mediator.Send(new GetExamCorrectionRoomByExamIdRequest(id));
        }

        [HttpGet("results/student/{id:guid}")]
        [OpenApiOperation("Get Student Answers", "")]
        public async Task<StudentAnswerDto> GetStudentAnswers(Guid id)
        {
            return await Mediator.Send(new GetExamStudentResultsByExamGroupStudentIdRequest(id));
        }

        [HttpPut("results/student")]
        [OpenApiOperation("Correct written questions.", "")]
        public async Task<Guid> CorrectWrittenQuestions(CorrectExamQuestionRequest request)
        {
            return await Mediator.Send(request);
        }

        //ExamResultsReadyByExamIdRequest
        [HttpPut("results/ready/{id:guid}")]
        [OpenApiOperation("Update exam make-results-ready-to-show", "")]
        public async Task<Guid> ExamResultsReadyByExamIdAsync(Guid id)
        {
            return await Mediator.Send(new ExamResultsReadyByExamIdRequest(id));
        }
    }
}
