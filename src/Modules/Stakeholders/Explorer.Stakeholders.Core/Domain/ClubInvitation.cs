﻿using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain;

public class ClubInvitation : Entity
{
    public long ClubId { get; init; }
    public long TouristId { get; init; }
    public DateTime Time { get; init; }
    public InvitationStatus Status { get; set; }

    public ClubInvitation() : this(-1, -1) { }

    public ClubInvitation(long clubId, long touristId) : this(clubId, touristId, DateTime.Now, InvitationStatus.Waiting) { }

    public ClubInvitation(long clubId, long touristId, DateTime time, InvitationStatus status)
    {
        ClubId = clubId;
        TouristId = touristId;
        Time = time;
        Status = status;
    }
}

public enum InvitationStatus
{
    Waiting,
    Accepted,
    Declined
}