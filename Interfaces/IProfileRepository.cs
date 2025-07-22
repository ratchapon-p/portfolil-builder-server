using System;
using portfolio_builder_server.Entities;

namespace portfolio_builder_server.Interfaces;

public interface IProfileRepository
{
    Task<Profile?> GetProfileByIdAsync(int id);
    void AddProfile(Profile profile);
    void UpdateProfile(Profile profile);
    Task<bool> SaveAllAsync();
    bool ProfileExists(int id);
}
