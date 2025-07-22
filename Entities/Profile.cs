using System;

namespace portfolio_builder_server.Entities;

public class Profile : BaseEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Tag { get; set; }
    public string? Bio { get; set; }
    public string? UserProfileImage { get; set; }
}
