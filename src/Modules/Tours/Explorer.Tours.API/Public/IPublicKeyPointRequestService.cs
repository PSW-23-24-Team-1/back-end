﻿using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Dtos.TouristEquipment;

using FluentResults;

namespace Explorer.Tours.API.Public;

    public interface IPublicKeyPointRequestService
    {
        Result<PublicKeyPointRequestResponseDto> Create<PublicKeyPointRequestCreateDto>(PublicKeyPointRequestCreateDto request);
        Result<PagedResult<PublicKeyPointRequestResponseDto>> GetPaged(int page, int pageSize);
        Result<PublicKeyPointRequestResponseDto> Update<PublicKeyPointRequestUpdateDto>(PublicKeyPointRequestUpdateDto request);
}

