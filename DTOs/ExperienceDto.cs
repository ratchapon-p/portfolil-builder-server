using System;

namespace portfolio_builder_server.DTOs;

public class ExperienceDto
{
    public int? Id { get; set; }
    public string? Role { get; set; }
    public string? Company { get; set; }
    public string? Location { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
}
