using System;
using portfolio_builder_server.Entities;

namespace portfolio_builder_server.Interfaces;

public interface IContactRepository
{
    Task<Contact?> GetContactByIdAsync(int id);
    void AddContact(Contact contact);
    void UpdateContact(Contact contact);
    Task<bool> SaveContactAsync();
    bool ContactExists(int id);
}
