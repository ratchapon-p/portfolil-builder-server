using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using portfolio_builder_server.Entities;
using portfolio_builder_server.Interfaces;

namespace portfolio_builder_server.Data;

public class UserAuthRepository(StoreContext context,IPasswordHasher<UserAuth> hasher) : IUserAuthServices
{
    public Task<UserAuth?> GetUserByIdAsync(int id)
    {
        return context.Set<UserAuth>().FindAsync(id).AsTask();
    }

    public async Task<UserAuth> Login(string email, string password)
    {
        email = email.Trim().ToLower();
        var user = await context.UserAuthen.SingleOrDefaultAsync(x => x.Email == email) ?? throw new Exception("User not found");
        
        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

        if (result != PasswordVerificationResult.Success) throw new UnauthorizedAccessException("Invalid credentials");

        return user;

    }

    public async Task<UserAuth> Register(string email, string password)
    {
        email = email.Trim().ToLower();
        if (await context.UserAuthen.AnyAsync(user => user.Email == email)) throw new Exception("User already Exists");

        if (password.Length < 6) throw new ArgumentException("Password must be at least 6 characters long.");

        var user = new UserAuth { Email = email };
        user.PasswordHash = hasher.HashPassword(user, password);

        context.UserAuthen.Add(user);
        await context.SaveChangesAsync();

        return user;
    }

    public async Task<bool> UserExists(string email)
    {
        return await context.UserAuthen.AnyAsync(u => u.Email == email);
    }
}
