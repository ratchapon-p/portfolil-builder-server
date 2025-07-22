using System;

namespace portfolio_builder_server.Entities;

public class Project : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? TechStack { get; set; }
    public string? DemoLink { get; set; }
    public string? RepoLink { get; set; }
    public byte IsDelete { get; set; } = 0;
}
