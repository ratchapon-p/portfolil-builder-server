using System;
using Microsoft.EntityFrameworkCore;
using portfolio_builder_server.Config;
using portfolio_builder_server.Entities;

namespace portfolio_builder_server.Data;

public class StoreContext(DbContextOptions options) : DbContext(options)
{
    public required DbSet<Profile> Profiles { get; set; }
    public required DbSet<UserAuth> UserAuthen { get; set; }
    public required DbSet<Contact> Contacts { get; set; }
    public required DbSet<Education> Educations { get; set; }
    public required DbSet<Experience> Experiences { get; set; }
    public required DbSet<Project> Projects { get; set; }
    public required DbSet<Skill> Skills { get; set; }
    public required DbSet<Certificate> Certificates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProfileConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EducationConfiguration).Assembly);

    }
}
