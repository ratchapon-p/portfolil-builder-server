using System;

namespace portfolio_builder_server.Entities;

public class Skill : BaseEntity
{
    public string? Name { get; set; }
    public string? Level { get; set; }
    public string? SkillImage { get; set; }
    public byte IsDelete { get; set; } = 0;
}
