using System;

namespace portfolio_builder_server.Entities;

public class Education : BaseEntity
{
    public string? Degree { get; set; }
    public string? Institution { get; set; }
    public string? Location { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Gpax { get; set; }
    public byte IsDelete { get; set; } = 0;
}
