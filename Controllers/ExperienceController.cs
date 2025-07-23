using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using portfolio_builder_server.Data;
using portfolio_builder_server.DTOs;
using portfolio_builder_server.Entities;
using portfolio_builder_server.Interfaces;

namespace portfolio_builder_server.Controllers;

public class ExperienceController(IGenericListRepository<Experience> repo) : BaseApiController
{
[HttpGet("{id:int}")]
    public async Task<ActionResult<Experience>> GetExperienceById(int id)
    {
        var experience = await repo.GetByIdAsync(id);

        if (experience == null) return NotFound(new {success= false, message = "Not Found experience form this id"});

        return Ok(new {success = true, data = experience});
    }

    [HttpGet("list/{userId:int}")]
    public async Task<ActionResult<IReadOnlyList<Experience>>> GetExperienceListByUserId(
        int userId,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 5
    )
    {
        var spec = new PaginationSpecification<Experience>();
        spec.ApplyPaging(pageIndex * pageSize, pageSize);

        return await CreatePagedResult(repo, spec, userId, pageIndex, pageSize);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Experience>> CreateExperience(ExperienceDto experienceItem)
    {
        var userId = GetUserId();
        var experience = new Experience
        {
            Role = experienceItem.Role,
            Company = experienceItem.Company,
            Location = experienceItem.Location,
            StartDate = experienceItem.StartDate,
            EndDate = experienceItem.EndDate,
            Description = experienceItem.Description,
            UserId = userId
        };

        repo.Add(experience);
        if (await repo.SaveAllAsync())
        {
            return CreatedAtAction("GetExperienceById", new { id = experience.Id }, experience);
        }

        return BadRequest(new {success= false, message = "Problem create experience"});
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Experience>> CreateExperience(int id, ExperienceDto experienceItem)
    {
        var userId = GetUserId();

        if (experienceItem.Id != id) return BadRequest(new { success = false, message = "Cannot update experience" });

        var experience = await repo.GetByIdAsync(id);

        if (experience == null || experience.UserId != userId) return NotFound(new { success = false, message = "Experience not found or does not belong to user" });

        experience.Role = experienceItem.Role;
        experience.Company = experienceItem.Company;
        experience.Location = experienceItem.Location;
        experience.StartDate = experienceItem.StartDate;
        experience.EndDate = experienceItem.EndDate;
        experience.Description = experienceItem.Description;

        repo.Update(experience);
        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest(new { success = false, message = "Problem Update experience" });
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> RemoveExperience(int id)
    {
        var experience = await repo.GetByIdAsync(id);

        if (experience == null) return NotFound(new { success = false, message = "Not Found Experience to delete" });

        repo.Remove(experience);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem delete experience");
    }
}
