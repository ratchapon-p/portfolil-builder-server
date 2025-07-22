using System;

namespace portfolio_builder_server.DTOs;

public class ProjectDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? TechStack { get; set; }
    public string? DemoLink { get; set; }
    public string? RepoLink { get; set; }
}
