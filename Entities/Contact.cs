using System;

namespace portfolio_builder_server.Entities;

public class Contact : BaseEntity
{
    public string? Email { get; set; }
    public string? Facebook { get; set; }
    public string? Line { get; set; }
    public string? Tel { get; set; }
    public string? Github { get; set; }
    public string? Linkedin { get; set; }
}
