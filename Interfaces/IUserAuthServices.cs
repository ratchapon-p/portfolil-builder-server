using System;
using portfolio_builder_server.Entities;

namespace portfolio_builder_server.Interfaces;

public interface IUserAuthServices
{
    Task<UserAuth> Register(string email, string password);
    Task<UserAuth> Login(string email, string password);
    Task<bool> UserExists(string email);
}
