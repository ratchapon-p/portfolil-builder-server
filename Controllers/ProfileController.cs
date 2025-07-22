using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using portfolio_builder_server.Entities;
using portfolio_builder_server.Interfaces;

namespace portfolio_builder_server.Controllers;

public class ProfileController(IProfileRepository profile) : BaseApiController
{
    //TODO: api/profile/:id
    //? Get profile by id
    //TODO: There's no authorize in get profile by id because must public too

    // [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Profile>> GetProfile(int id)
    {
        var profileItem = await profile.GetProfileByIdAsync(id);
        if (profileItem == null) return NotFound();
        return profileItem;
    }

    //! ทำให้ get user id จาก authorize เพื่อ save ลง db ของตัวอื่นๆได้
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateProfile(Profile profileItem)
    {

        var userId = GetUserId();

        profileItem.UserId = userId;

        profile.AddProfile(profileItem);
        if (await profile.SaveAllAsync())
        {
            return CreatedAtAction("GetProfile", new { id = profileItem.Id }, profileItem);
        }
        return BadRequest("Problem Create Profile");
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProfile(int id, Profile profileItem)
    {
        var userId = GetUserId();

        profileItem.UserId = userId;

        if (profileItem.Id != id || !profile.ProfileExists(id)) return BadRequest("Cannot update profile, Profile not exists");

        profile.UpdateProfile(profileItem);
        
        if (await profile.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem Update Profile");
    }


}
