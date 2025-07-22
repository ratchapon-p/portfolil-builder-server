using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portfolio_builder_server.Entities;

namespace portfolio_builder_server.Config;

public class EducationConfiguration : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.Property(x => x.Gpax).HasColumnType("decimal(3,2)");
    }
}
