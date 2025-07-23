using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using portfolio_builder_server.Data;
using portfolio_builder_server.DTOs;
using portfolio_builder_server.Entities;
using portfolio_builder_server.Interfaces;

namespace portfolio_builder_server.Controllers;

public class CertificateController(IGenericListRepository<Certificate> repo) : BaseApiController
{
[HttpGet("{id:int}")]
    public async Task<ActionResult<Certificate>> GetCertificateById(int id)
    {
        var certificate = await repo.GetByIdAsync(id);

        if (certificate == null) return NotFound(new { success = false, message = "Not Found certificate form this id" });

        return Ok(new {success = true, data = certificate});;
    }

    [HttpGet("list/{userId:int}")]
    public async Task<ActionResult<IReadOnlyList<Certificate>>> GetCertificateListByUserId(
        int userId,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 5
    )
    {
        var spec = new PaginationSpecification<Certificate>();
        spec.ApplyPaging(pageIndex * pageSize, pageSize);

        return await CreatePagedResult(repo, spec, userId, pageIndex, pageSize);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Certificate>> CreateCertificate(CertificateDto certificateItem)
    {
        var userId = GetUserId();
        var certificate = new Certificate
        {
            Name = certificateItem.Name,
            Description = certificateItem.Description,
            CertificationImage = certificateItem.CertificationImage,
            CertificationLink = certificateItem.CertificationLink,
            ReceiveDate = certificateItem.ReceiveDate,
            ExpireDate = certificateItem.ExpireDate,
            UserId = userId
        };

        repo.Add(certificate);
        if (await repo.SaveAllAsync())
        {
            return CreatedAtAction("GetCertificateById", new { id = certificate.Id }, certificate);
        }

        return BadRequest(new {success= false, message = "Problem create certificate"});
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Certificate>> CreateCertificate(int id, CertificateDto certificateItem)
    {
        var userId = GetUserId();

        if (certificateItem.Id != id) return BadRequest(new {success= false, message = "Cannot update certificate"});

        var certificate = await repo.GetByIdAsync(id);

        if (certificate == null || certificate.UserId != userId) return NotFound(new {success= false, message = "Certificate not found or does not belong to user"});

        certificate.Name = certificateItem.Name;
        certificate.Description = certificateItem.Description;
        certificate.CertificationImage = certificateItem.CertificationImage;
        certificate.CertificationLink = certificateItem.CertificationLink;
        certificate.ReceiveDate = certificateItem.ReceiveDate;
        certificate.ExpireDate = certificateItem.ExpireDate;
        

        repo.Update(certificate);
        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest(new {success= false, message = "Problem Update certificate"});
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> RemoveCertificate(int id)
    {
        var certificate = await repo.GetByIdAsync(id);

        if (certificate == null) return NotFound(new {success= false, message = "Not Found Certificate to delete"});

        repo.Remove(certificate);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest(new {success= false, message = "Problem delete certificate"});
    }
}
