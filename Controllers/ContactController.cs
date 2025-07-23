using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using portfolio_builder_server.Entities;
using portfolio_builder_server.Interfaces;

namespace portfolio_builder_server.Controllers;

public class ContactController(IContactRepository contact) : BaseApiController
{

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Contact>> GetContact(int id)
    {
        var contactResult = await contact.GetContactByIdAsync(id);

        if (contactResult == null) return NotFound(new { success = false, message = "Not found contact" });

        return Ok(new {success = true, data = contactResult});
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateContact(Contact contactItem)
    {
        var userId = GetUserId();

        contactItem.UserId = userId;

        contact.AddContact(contactItem);

        if (await contact.SaveContactAsync())
        {
            return CreatedAtAction("Getcontact", new { id = contactItem.Id }, contactItem);
        }
        return BadRequest(new { success = false, message = "Problem Create Contact" });
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateContact(int id, Contact contactItem)
    {
        var userId = GetUserId();

        contactItem.UserId = userId;

        if (contactItem.Id != id || !contact.ContactExists(id)) return BadRequest(new { success = false, message = "Cannot update contact, Contact not exists" });

        contact.UpdateContact(contactItem);

        if (await contact.SaveContactAsync())
        {
            return NoContent();
        }
        return BadRequest(new { success = false, message = "Problem Update Contact" });
    }

    

}
