using System;

namespace portfolio_builder_server.Entities;

public class Experience : BaseEntity
{
    public string? Role { get; set; }
    public string? Company { get; set; }
    public string? Location { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public byte IsDelete { get; set; } = 0;
}
