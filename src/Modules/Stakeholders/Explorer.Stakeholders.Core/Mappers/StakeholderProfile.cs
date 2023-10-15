using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;
using System.Reflection;

namespace Explorer.Stakeholders.Core.Mappers;

public class StakeholderProfile : Profile
{
    public StakeholderProfile()
    {
        CreateMap<ClubJoinRequest, ClubJoinRequestDto>().ReverseMap()
            .ConstructUsing(src => new ClubJoinRequest(src.TouristId, src.ClubId, src.RequestedAt, ClubJoinRequestStatus.Pending));
    }
}