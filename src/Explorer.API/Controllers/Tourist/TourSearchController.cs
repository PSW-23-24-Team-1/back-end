﻿using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/tour")]
    public class TourSearchController : BaseApiController
    {
        private readonly ITourSearchService _tourSearchService;

        public TourSearchController(ITourSearchService tourSearchService)
        {
            _tourSearchService = tourSearchService;
        }

        [HttpGet]
        public ActionResult<PagedResult<TourResponseDto>> SearchByGeoLocation([FromQuery] double longitude, [FromQuery] double latitude, [FromQuery] double maxDistance, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourSearchService.Search(longitude, latitude, maxDistance, page, pageSize);
            return CreateResponse(result);
        }
    }
}