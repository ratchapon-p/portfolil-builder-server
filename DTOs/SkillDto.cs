using System;

namespace portfolio_builder_server.DTOs;

public class SkillDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Level { get; set; }
    public string? SkillImage { get; set; }
}
