﻿namespace Explorer.Tours.API.Dtos;

public class TourSearchFilterDto
{
    public string? Name { get; set; }
    public double? MinPrice { get; set; }
    public double? MaxPrice { get; set; }
    public int? MinDifficulty { get; set; }
    public int? MaxDifficulty { get; set; }
    public double? MinDuration { get; set; }
    public double? MaxDuration { get; set; }
    public int? MinAverageRating { get; set; }
    public double? MinLength { get; set; }
    public double? MaxLength { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public double? MaxDistance { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
