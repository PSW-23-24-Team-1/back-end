﻿using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.UseCases.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/tour-preferences")]
    public class TourPreferenceController : BaseApiController
    {
        private readonly ITourPreferenceService _tourPreferencesService;

        public TourPreferenceController(ITourPreferenceService tourPreferencesService)
        {
            _tourPreferencesService = tourPreferencesService;
        }

        [HttpGet]
        public ActionResult<TourPreferenceResponseDto> Get()
        {
            int userId = int.Parse(HttpContext.User.Claims.First(i => i.Type.Equals("id", StringComparison.OrdinalIgnoreCase)).Value);
            var result = _tourPreferencesService.GetByUserId(userId);
            return CreateResponse(result);
        }

        [HttpPost("create")]
        public ActionResult<TourPreferenceResponseDto> Create([FromBody] TourPreferenceCreateDto preference)
        {
            preference.UserId = int.Parse(HttpContext.User.Claims.First(i => i.Type.Equals("id", StringComparison.OrdinalIgnoreCase)).Value);
            var result = _tourPreferencesService.Create(preference);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _tourPreferencesService.Delete(id);
            return CreateResponse(result);
        }

        [HttpPut]
        public ActionResult<TourPreferenceResponseDto> Update([FromBody] TourPreferenceUpdateDto preference)
        {
            preference.UserId = int.Parse(HttpContext.User.Claims.First(i => i.Type.Equals("id", StringComparison.OrdinalIgnoreCase)).Value);
            var result = _tourPreferencesService.Update(preference);
            return CreateResponse(result);
        }
    }
}