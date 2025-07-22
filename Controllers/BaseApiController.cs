using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using portfolio_builder_server.Entities;
using portfolio_builder_server.Interfaces;
using portfolio_builder_server.RequestHelpers;

namespace portfolio_builder_server.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BaseApiController : Controller
{
    protected int GetUserId()
    {
        var userClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("Notfound user");
        var userId = int.Parse(userClaim.Value);
        return userId;
    }
    protected async Task<ActionResult> CreatePagedResult<T>(IGenericListRepository<T> repo, ISpecification<T> spec,int userId, int pageIndex, int pageSize) where T : BaseEntity
    {
        var item = await repo.ListAsync(spec, userId);
        var count = await repo.CountAsync(spec,userId);

        var pagination = new Pagination<T>(pageIndex, pageSize, count,item);
        
        return Ok(pagination);
    }
}
