using System;

namespace portfolio_builder_server.DTOs;

public class EducationDto
{
    public int? Id { get; set; }
    public string? Institution { get; set; }
    public string? Degree { get; set; }
    public string? Location { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Gpax { get; set; }
}
