using System;

namespace portfolio_builder_server.DTOs;

public class RegisterDto
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}
