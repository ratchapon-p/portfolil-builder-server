using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using portfolio_builder_server.Data;
using portfolio_builder_server.DTOs;
using portfolio_builder_server.Entities;
using portfolio_builder_server.Interfaces;

namespace portfolio_builder_server.Controllers;

public class EducationController(IGenericListRepository<Education> repo) : BaseApiController
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Education>> GetEducationById(int id)
    {
        var education = await repo.GetByIdAsync(id);

        if (education == null) return NotFound(new { success = false, message = "Not Found education form this id" });

        return  Ok(new {success = true, data = education});;
    }

    [HttpGet("list/{userId:int}")]
    public async Task<ActionResult<IReadOnlyList<Education>>> GetEducationListByUserId(
        int userId,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 5
    )
    {
        var spec = new PaginationSpecification<Education>();
        spec.ApplyPaging(pageIndex * pageSize, pageSize);

        return await CreatePagedResult(repo, spec, userId, pageIndex, pageSize);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Education>> CreateEducation(EducationDto educationItem)
    {
        var userId = GetUserId();
        var education = new Education
        {
            Degree = educationItem.Degree,
            Institution = educationItem.Institution,
            Location = educationItem.Location,
            StartDate = educationItem.StartDate,
            EndDate = educationItem.EndDate,
            Gpax = educationItem.Gpax,
            UserId = userId
        };

        repo.Add(education);
        if (await repo.SaveAllAsync())
        {
            return CreatedAtAction("GetEducationById", new { id = education.Id }, education);
        }

        return BadRequest(new { success = false, message = "Problem create education" });
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Education>> CreateEducation(int id, EducationDto educationItem)
    {
        var userId = GetUserId();

        if (educationItem.Id != id) return BadRequest(new { success = false, message = "Cannot update education" });

        var education = await repo.GetByIdAsync(id);

        if (education == null || education.UserId != userId) return NotFound(new { success = false, message = "Education not found or does not belong to user" });

        education.Degree = educationItem.Degree;
        education.Institution = educationItem.Institution;
        education.Location = educationItem.Location;
        education.StartDate = educationItem.StartDate;
        education.EndDate = educationItem.EndDate;
        education.Gpax = educationItem.Gpax;

        repo.Update(education);
        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest(new { success = false, message = "Problem Update education" });
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> RemoveEducation(int id)
    {
        var education = await repo.GetByIdAsync(id);

        if (education == null) return NotFound(new { success = false, message = "Not Found Education to delete" });

        repo.Remove(education);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest(new { success = false, message = "Problem delete education" });
    }
}
