using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using portfolio_builder_server.DTOs;
using portfolio_builder_server.Entities;
using portfolio_builder_server.Interfaces;
using portfolio_builder_server.Services;

namespace portfolio_builder_server.Controllers;

public class UserController(IUserAuthServices userAuth, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserAuth>> Register(RegisterDto registerDto)
    {
        if (await userAuth.UserExists(registerDto.Email)) return BadRequest(new {success=false,message = "Email Exists"});

        var user = await userAuth.Register(registerDto.Email, registerDto.Password);

        if (user == null) return BadRequest(new{success=false,message = "Registration failed"});

        return Created(string.Empty, new 
            {
                success = true,
                message = "Registration successful"
            });
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserAuth>> Login(LoginDto loginDto)
    {
        var user = await userAuth.Login(loginDto.Email, loginDto.Password);

        if (user == null) return Unauthorized(new{success=false,message = "Invalid email or password"});
        Console.WriteLine(user);
        var token = tokenService.CreateToken(user);

        Response.Cookies.Append("token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddDays(1),
            IsEssential = true
        });

        return Ok(new { success = true, message = "Login success",user = new {email = user.Email} }); ;
    }

    [Authorize]
    [HttpPost("logout")]
    public ActionResult Logout()
    {
        Response.Cookies.Delete("token");
        return Ok(new { success = true,message = "Logged out" });
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetUserById(int id)
    {
        var user = await userAuth.GetUserByIdAsync(id);

        if (user == null) return NotFound("Not Found project form this id");

        return Ok(new { success = true, data = new { user.Email, user.Id } });
    }
}
