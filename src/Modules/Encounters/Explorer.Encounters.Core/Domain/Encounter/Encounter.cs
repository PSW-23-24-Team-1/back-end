﻿using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Encounters.Core.Domain.Encounter
{
    public class Encounter : Entity
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public double Longitude { get; init; }
        public double Latitude { get; init; }
        public double Radius { get; init; }
        public int XpReward { get; init; }
        public EncounterStatus Status { get; private set; }
        public List<EncounterInstance> Instances { get; } = new List<EncounterInstance>();

        public Encounter() { }
        public Encounter(string title, string description, double longitude, double latitude, int xp, EncounterStatus status)
        {
            Title = title;
            Description = description;
            Longitude = longitude;
            Latitude = latitude;
            XpReward = xp;
            Status = status;
            Validate();
        }
        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Title)) throw new ArgumentException("Invalid Title");
            if (string.IsNullOrWhiteSpace(Description)) throw new ArgumentException("Invalid Description");
            if (Longitude < -180 || Longitude > 180) throw new ArgumentException("Invalid Longitude");
            if (Latitude < -90 || Latitude > 90) throw new ArgumentException("Invalid Latitude");
            if (XpReward < 0) throw new ArgumentException("XP cannot be negative");
        }

        public void Archive()
        {
            Status = EncounterStatus.Archived;
        }

        public void Publish()
        {
            Status = EncounterStatus.Active;
        }

        public void ActivateEncounter(long userId, double userLongitude, double userLatitude)
        {
            if (Status != EncounterStatus.Active)
                throw new ArgumentException("Encounter is not yet published.");
            if (hasUserActivatedEncounter(userId))
                throw new ArgumentException("User has already activated/completed this encounter.");
            if (isUserInRange(userLongitude, userLatitude))
                throw new ArgumentException("User is not close enough to the encounter.");

            Instances.Add(new EncounterInstance(userId));
        }

        protected void CompleteEncounter(long userId)
        {
            Instances.First(x => x.UserId == userId).Complete();
        }

        protected bool hasUserActivatedEncounter(long userId)
        {
            return Instances.FirstOrDefault(x => x.UserId == userId) != default(EncounterInstance);
        }

        protected bool isUserInRange(double userLongitute, double userLatitude)
        {
            if (userLongitute < -180 || userLongitute > 180) throw new ArgumentException("Invalid Longitude");
            if (userLatitude < -90 || userLatitude > 90) throw new ArgumentException("Invalid Latitude");

            const double earthRadius = 6371000;
            double latitude1 = Latitude * Math.PI / 180;
            double longitude1 = Longitude * Math.PI / 180;
            double latitude2 = userLatitude * Math.PI / 180;
            double longitude2 = userLongitute * Math.PI / 180;

            double latitudeDistance = latitude2 - latitude1;
            double longitudeDistance = longitude2 - longitude1;

            double a = Math.Sin(latitudeDistance / 2) * Math.Sin(latitudeDistance / 2) +
                       Math.Cos(latitude1) * Math.Cos(latitude2) *
                       Math.Sin(longitudeDistance / 2) * Math.Sin(longitudeDistance / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = earthRadius * c;

            return distance < Radius;
        }
    }
    public enum EncounterStatus { Active, Draft, Archived };

}