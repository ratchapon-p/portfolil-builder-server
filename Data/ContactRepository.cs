using System;
using Microsoft.EntityFrameworkCore;
using portfolio_builder_server.Entities;
using portfolio_builder_server.Interfaces;

namespace portfolio_builder_server.Data;

public class ContactRepository(StoreContext context) : IContactRepository
{
    public void AddContact(Contact contact)
    {
        context.Set<Contact>().Add(contact);
    }

    public bool ContactExists(int id)
    {
        return context.Contacts.Any(x => x.Id == id);
    }

    public async Task<Contact?> GetContactByIdAsync(int id)
    {
        return await context.Contacts.FindAsync(id).AsTask();
    }

    public async Task<bool> SaveContactAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void UpdateContact(Contact contact)
    {
        context.Entry(contact).State = EntityState.Modified;
    }
}
