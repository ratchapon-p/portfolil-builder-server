using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using portfolio_builder_server.Entities;
using portfolio_builder_server.Interfaces;

namespace portfolio_builder_server.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(UserAuth user)
    {
        var jwtKey = config["JwtKey"];
        if (string.IsNullOrEmpty(jwtKey)) throw new Exception("JwtKey is not configured");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var defaultIssuer = config["Issuer"] ?? "https://localhost:5001";
        var defaultAudience = config["Audience"] ?? "http://localhost:3000";

        var token = new JwtSecurityToken(
            issuer: defaultIssuer,
            audience: defaultAudience,
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
