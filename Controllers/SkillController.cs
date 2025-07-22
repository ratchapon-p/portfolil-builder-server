using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using portfolio_builder_server.Data;
using portfolio_builder_server.DTOs;
using portfolio_builder_server.Entities;
using portfolio_builder_server.Interfaces;

namespace portfolio_builder_server.Controllers;

public class SkillController(IGenericListRepository<Skill> repo) : BaseApiController
{
[HttpGet("{id:int}")]
    public async Task<ActionResult<Skill>> GetSkillById(int id)
    {
        var skill = await repo.GetByIdAsync(id);

        if (skill == null) return NotFound("Not Found skill form this id");

        return skill;
    }

    [HttpGet("list/{userId:int}")]
    public async Task<ActionResult<IReadOnlyList<Skill>>> GetSkillListByUserId(
        int userId,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 5
    )
    {
        var spec = new PaginationSpecification<Skill>();
        spec.ApplyPaging(pageIndex * pageSize, pageSize);

        return await CreatePagedResult(repo, spec, userId, pageIndex, pageSize);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Skill>> CreateSkill(SkillDto skillItem)
    {
        var userId = GetUserId();
        var skill = new Skill
        {
            Name = skillItem.Name,
            Level = skillItem.Level,
            SkillImage = skillItem.SkillImage,
            UserId = userId
        };

        repo.Add(skill);
        if (await repo.SaveAllAsync())
        {
            return CreatedAtAction("GetSkillById", new { id = skill.Id }, skill);
        }

        return BadRequest("Problem create skill");
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Skill>> CreateSkill(int id, SkillDto skillItem)
    {
        var userId = GetUserId();

        if (skillItem.Id != id) return BadRequest("Cannot update skill");

        var skill = await repo.GetByIdAsync(id);

        if (skill == null || skill.UserId != userId) return NotFound("Skill not found or does not belong to user");

        skill.Name = skillItem.Name;
        skill.Level = skillItem.Level;
        skill.SkillImage = skillItem.SkillImage;
        

        repo.Update(skill);
        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem Update skill");
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> RemoveSkill(int id)
    {
        var skill = await repo.GetByIdAsync(id);

        if (skill == null) return NotFound("Not Found Skill to delete");

        repo.Remove(skill);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem delete skill");
    }
}
