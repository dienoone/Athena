using Athena.Application.Features.DashboardFeatures.Teachers.Dto;
using Athena.Application.Features.TeacherFeatures.Years.Commands;
using Athena.Application.Features.TeacherFeatures.Years.Dtos;
using Athena.Application.Features.TeacherFeatures.Years.Queries;

namespace Athena.Api.Controllers.Teacher
{
    //ToDo Need To Edit Actions And Perimssion to All Of These
    public class YearsController : VersionNeutralApiTeacherGroupController
    {

        #region Year:

        [HttpGet("required")]
        [OpenApiOperation("Get Teacher-Coure-levels")]
        public async Task<List<TeacherCourseLevelDto>> GetTeahcerCourseLevelsAsync()
        {
            return await Mediator.Send(new GetYearRequiredRequest());
        }


        [HttpGet]
        [OpenApiOperation("Get Years.", "")]
        public async Task<YearListRequestDto> GetAsync()
        {
            return await Mediator.Send(new GetYearsRequest());
        }

        [HttpGet("{id:guid}")]
        [OpenApiOperation("Get year details.", "")]
        public async Task<YearDetailDto> GetAsync(Guid id)
        {
            return await Mediator.Send(new GetYearDetailByIdRequest(id));
        }

        // GetAvailableLevelsByYearIdRequest
        [HttpGet("levels/{id:guid}")]
        [OpenApiOperation("Get available levels.", "")]
        public async Task<List<AvailableLevelsForYearDto>> GetLevelsAsync(Guid id)
        {
            return await Mediator.Send(new GetAvailableLevelsByYearIdRequest(id));
        }

        [HttpPost]
        [MustHavePermission(AAction.Create, AResource.Years)]
        [OpenApiOperation("Create a new Year.", "")]
        public async Task<Guid> CreateAsync(CreateYearRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("End/{id:guid}")]
        [MustHavePermission(AAction.Toggle, AResource.Years)]
        [OpenApiOperation("End a Year.", "")]
        public async Task<ActionResult<Guid>> EndAsync(Guid id)
        {
            return await Mediator.Send(new EndYearRequest(id));
        }

        [HttpPut("{id:guid}")]
        [MustHavePermission(AAction.Update, AResource.Years)]
        [OpenApiOperation("Update a year.", "")]
        public async Task<ActionResult<Guid>> UpdateAsync(UpdateYearStateRequest request, Guid id)
        {
            return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
        }

        [HttpDelete("{id:guid}")]
        [OpenApiOperation("Delete a year.", "")]
        public async Task<Guid> DeleteAsync(Guid id)
        {
            return await Mediator.Send(new DeleteYearByIdRequest(id));
        }

        #endregion

        #region TeacherCourseLevelYear:

        [HttpPost("Level/{id:guid}")]
        [MustHavePermission(AAction.Update, AResource.Years)]
        [OpenApiOperation("Create new levels.", "")]
        public async Task<ActionResult<Guid>> CreateLevelsForYearsAsync(CreateLevelsForYearByLevelIdsRequest request, Guid id)
        {
            return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
        }

        [HttpPut("Level/{id:guid}")]
        [MustHavePermission(AAction.Update, AResource.Years)]
        [OpenApiOperation("Update a level.", "")]
        public async Task<ActionResult<Guid>> UpdateTeacherCourseLevelYearAsync(UpdateTeacherCourseLevelYearByIdRequest request, Guid id)
        {
            return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
        }

        [HttpDelete("Level/{id:guid}")]
        [OpenApiOperation("Delete a level.", "")]
        public async Task<Guid> DeleteTeacherCourseLevelYearAsync(Guid id)
        {
            return await Mediator.Send(new DeleteTeacherCourseLevelYearByIdRequest(id));
        }

        #endregion

        #region Semsters:

        /*[HttpPut("Level/Semster/{id:guid}")]
        [MustHavePermission(AAction.Update, AResource.Years)]
        [OpenApiOperation("Open a semster.", "")]
        public async Task<ActionResult<Guid>> OpenSemsterAsync(Guid id)
        {
            return await Mediator.Send(new OpenSemsterRequest(id));
        }*/

        #endregion

    }
}
