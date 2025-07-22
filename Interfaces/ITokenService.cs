using System;
using portfolio_builder_server.Entities;

namespace portfolio_builder_server.Interfaces;

public interface ITokenService
{
    string CreateToken(UserAuth user);
}
