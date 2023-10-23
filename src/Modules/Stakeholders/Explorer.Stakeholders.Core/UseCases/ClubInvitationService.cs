﻿using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Public.Administration;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases;

public class ClubInvitationService : IClubInvitationService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IClubInvitationRepository _invitationRepository;
    private readonly IClubRepository _clubRepository;
    private readonly IClubJoinRequestRepository _requestRepository;
    private readonly IClubMemberManagementService _clubMemberManagementService;

    public ClubInvitationService(IMapper mapper, IUserRepository userRepository, IClubInvitationRepository invitationRepository, IClubRepository clubRepository, IClubJoinRequestRepository requestRepository, IClubMemberManagementService clubMemberManagementService)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _invitationRepository = invitationRepository;
        _clubRepository = clubRepository;
        _requestRepository = requestRepository;
        _clubMemberManagementService = clubMemberManagementService;
    }

    public Result<ClubInvitationDto> InviteTourist(ClubInvitationDto invitationDto)
    {
        try
        {
            var invitation = _mapper.Map<ClubInvitation>(invitationDto);
            var tourist = _userRepository.GetPersonId(invitationDto.TouristId);
            var club = _clubRepository.Get(invitationDto.ClubId);

            _invitationRepository.Create(invitation);

            return invitationDto;
        }
        catch (KeyNotFoundException)
        {
            return Result.Fail(FailureCode.NotFound).WithError(FailureCode.NotFound);
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }

    public Result Reject(long clubInvitationId, long userId)
    {
        try
        {
            var invitation = _invitationRepository.Get(clubInvitationId);

            if (!isWaiting(invitation))
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(FailureCode.InvalidArgument);
            }

            if (userId != invitation.TouristId)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(FailureCode.InvalidArgument);
            }

            invitation.Status = InvitationStatus.Declined;

            _invitationRepository.Update(invitation);

            return Result.Ok().WithSuccess("Club invitation rejected successfully.");
        }
        catch (KeyNotFoundException)
        {
            return Result.Fail(FailureCode.NotFound).WithError(FailureCode.NotFound);
        }
    }

    public Result Accept(long clubInvitationId, long userId)
    {
        try
        {
            var invitation = _invitationRepository.Get(clubInvitationId);

            if (!isWaiting(invitation))
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(FailureCode.InvalidArgument);
            }

            if (userId != invitation.TouristId)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(FailureCode.InvalidArgument);
            }

            invitation.Status = InvitationStatus.Accepted;

            _invitationRepository.Update(invitation);
            _clubMemberManagementService.AddMember(invitation.ClubId, userId);
            _requestRepository.DeletePending(invitation.ClubId, invitation.TouristId);

            return Result.Ok().WithSuccess("Club invitation rejected successfully.");
        }
        catch (KeyNotFoundException)
        {
            return Result.Fail(FailureCode.NotFound).WithError(FailureCode.NotFound);
        }
    }

    private bool isWaiting(ClubInvitation clubInvitation)
    {
        return clubInvitation.Status == InvitationStatus.Waiting;
    }
}