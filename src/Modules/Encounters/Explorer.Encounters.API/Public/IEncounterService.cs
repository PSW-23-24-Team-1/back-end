﻿using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using FluentResults;

namespace Explorer.Encounters.API.Public
{
    public interface IEncounterService
    {
        Result<EncounterResponseDto> Create<EncounterCreateDto>(EncounterCreateDto encounter);
        Result<EncounterResponseDto> Update<EncounterUpdateDto>(EncounterUpdateDto encounter);
        Result<PagedResult<EncounterResponseDto>> GetPaged(int page, int pageSize);
        Result<PagedResult<EncounterResponseDto>> GetActive(int page, int pageSize);
        Result<EncounterResponseDto> Get(long id);
        Result Delete(long id);
    }
}