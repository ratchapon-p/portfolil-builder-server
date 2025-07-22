using System;

namespace portfolio_builder_server.Entities;

public class Certificate : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? CertificationImage { get; set; }
    public string? CertificationLink { get; set; }
    public DateTime? ReceiveDate { get; set; }
    public DateTime? ExpireDate { get; set; }
    public byte IsDelete { get; set; } = 0;
}
