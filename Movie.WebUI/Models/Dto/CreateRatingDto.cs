﻿namespace Movie.WebUI.Models.Dto;

public class CreateRatingDto
{
    [Required]
    public string UserId { get; set; }

    [Required]
    public int RatingValue { get; set; }

    [Required]
    public int MovieId { get; set; }
}
