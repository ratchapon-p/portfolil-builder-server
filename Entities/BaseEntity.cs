using System;

namespace portfolio_builder_server.Entities;

public class BaseEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
}
