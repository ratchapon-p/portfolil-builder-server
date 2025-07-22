using System;

namespace portfolio_builder_server.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using portfolio_builder_server.Data;
using portfolio_builder_server.DTOs;
using portfolio_builder_server.Entities;
using portfolio_builder_server.Interfaces;


public class ProjectController(IGenericListRepository<Project> repo) : BaseApiController
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Project>> GetProjectById(int id)
    {
        var project = await repo.GetByIdAsync(id);

        if (project == null) return NotFound("Not Found project form this id");

        return project;
    }

    [HttpGet("list/{userId:int}")]
    public async Task<ActionResult<IReadOnlyList<Project>>> GetProjectListByUserId(
        int userId,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 5
    )
    {
        var spec = new PaginationSpecification<Project>();
        spec.ApplyPaging(pageIndex * pageSize, pageSize);

        return await CreatePagedResult(repo, spec, userId, pageIndex, pageSize);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Project>> CreateProject(ProjectDto projectItem)
    {
        var userId = GetUserId();
        var project = new Project
        {
            Name = projectItem.Name,
            TechStack = projectItem.TechStack,
            DemoLink = projectItem.DemoLink,
            RepoLink = projectItem.RepoLink,
            Description = projectItem.Description,
            UserId = userId
        };

        repo.Add(project);
        if (await repo.SaveAllAsync())
        {
            return CreatedAtAction("GetProjectById", new { id = project.Id }, project);
        }

        return BadRequest("Problem create project");
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Project>> CreateProject(int id, ProjectDto projectItem)
    {
        var userId = GetUserId();

        if (projectItem.Id != id) return BadRequest("Cannot update project");

        var project = await repo.GetByIdAsync(id);

        if (project == null || project.UserId != userId) return NotFound("Project not found or does not belong to user");

        project.Name = projectItem.Name;
        project.TechStack = projectItem.TechStack;
        project.DemoLink = projectItem.DemoLink;
        project.RepoLink = projectItem.RepoLink;
        project.Description = projectItem.Description;

        repo.Update(project);
        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem Update project");
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> RemoveProject(int id)
    {
        var project = await repo.GetByIdAsync(id);

        if (project == null) return NotFound("Not Found Project to delete");

        repo.Remove(project);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem delete project");
    }
}
