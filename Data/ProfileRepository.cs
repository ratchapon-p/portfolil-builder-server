using System;
using Microsoft.EntityFrameworkCore;
using portfolio_builder_server.Entities;
using portfolio_builder_server.Interfaces;

namespace portfolio_builder_server.Data;

public class ProfileRepository(StoreContext context) : IProfileRepository
{
    public void AddProfile(Profile profile)
    {
        context.Set<Profile>().Add(profile);
    }

    public async Task<Profile?> GetProfileByIdAsync(int id)
    {
        return await context.Profiles.FindAsync(id).AsTask();
    }

    public bool ProfileExists(int id)
    {
        return context.Profiles.Any(x => x.Id == id);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void UpdateProfile(Profile profile)
    {
        context.Entry(profile).State = EntityState.Modified;
    }
}
