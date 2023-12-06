using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Tours.API.Dtos.TouristPosition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{

    [Authorize(Policy = "touristPolicy")]
    [Microsoft.AspNetCore.Components.Route("api/tourist/misc-encounter")]
    public class MiscEncounterController:BaseApiController
    {

        private readonly IEncounterService _encounterService;
        public MiscEncounterController(IEncounterService encounterService)
        {
            _encounterService = encounterService;
        }

        [HttpPost("{id:long}/complete")]
        public ActionResult<EncounterResponseDto> Complete([FromBody]  long id)
        {
            long userId = int.Parse(HttpContext.User.Claims.First(i => i.Type.Equals("id", StringComparison.OrdinalIgnoreCase)).Value);
            var result = _encounterService.CompleteMiscEncounter(userId, id);
            return CreateResponse(result);
        }

        [HttpPost("all-misc-encounter")]
        public ActionResult<PagedResult<EncounterResponseDto>> GetAllInRangeOf([FromBody] [FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _encounterService.GetAll(page, pageSize);
            return CreateResponse(result);
        }

    }
}
