using System;

namespace portfolio_builder_server.Entities;

public class UserAuth
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public string PasswordHash { get; set; } = "";
}
