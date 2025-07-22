using System;
using Microsoft.AspNetCore.Mvc;
using portfolio_builder_server.DTOs;
using portfolio_builder_server.Entities;
using portfolio_builder_server.Interfaces;
using portfolio_builder_server.Services;

namespace portfolio_builder_server.Controllers;

public class UserController(IUserAuthServices userAuth,ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserAuth>> Register(RegisterDto registerDto)
    {
        if (await userAuth.UserExists(registerDto.Email)) return BadRequest("Email Exists");

        var user = await userAuth.Register(registerDto.Email, registerDto.Password);

        if (user == null) return BadRequest("Registration failed");

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserAuth>> Login(LoginDto loginDto)
    {
        var user = await userAuth.Login(loginDto.Email, loginDto.Password);

        if (user == null) return Unauthorized("Invalid email or password");
        Console.WriteLine(user);
        var token = tokenService.CreateToken(user);

        Response.Cookies.Append("token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(1),
            IsEssential = true
        });

        return Ok(new { message = "Login success" });;
    }

}
