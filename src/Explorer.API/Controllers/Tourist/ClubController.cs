﻿using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/club")]
    public class ClubController : BaseApiController
    {
        private readonly IClubService _clubService;

        public ClubController(IClubService clubService)
        {
            _clubService = clubService;
        }
        [HttpGet]
        public ActionResult<PagedResult<ClubResponseDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _clubService.GetClubsPaged(page, pageSize);
            return CreateResponse(result);
        }
        [HttpGet]
        [Route("ownerclubs")]
        public ActionResult<PagedResult<ClubResponseDto>> GetOwnerClubs()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            long ownerId = long.Parse(identity.FindFirst("id").Value);
            var result = _clubService.GetOwnerClubs(ownerId);
            return CreateResponse(result);
        }
        [HttpPost]
        public ActionResult<ClubResponseDto> Create([FromBody] ClubCreateDto club)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
            {
                club.OwnerId = long.Parse(identity.FindFirst("id").Value);
            }
            var result = _clubService.Create(club);
            return CreateResponse(result);
        }
        [HttpPut]
        public ActionResult<ClubResponseDto> Update([FromBody] ClubResponseDto club)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
            {
                //kompletna provera bi bila kada bih uzeo club na osnovu njegovog id-a
                // i onda njegov OwnerId uporedio sa ulogovanim
                if(club.OwnerId != long.Parse(identity.FindFirst("id").Value))
                {
                    return Forbid();
                }
            }
            var result = _clubService.Update(club);
            return CreateResponse(result);
        }
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _clubService.Delete(id);
            return CreateResponse(result);
        }
    }
}
