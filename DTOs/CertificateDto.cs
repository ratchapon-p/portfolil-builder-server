using System;

namespace portfolio_builder_server.DTOs;

public class CertificateDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? CertificationImage { get; set; }
    public string? CertificationLink { get; set; }
    public DateTime? ReceiveDate { get; set; }
    public DateTime? ExpireDate { get; set; }
}
