﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyLab.EmailManager.Domain.ValueObjects;
using MyLab.EmailManager.Infrastructure.Db.EfConverters;

namespace MyLab.EmailManager.Infrastructure.Db.EfTypeConfigurations;

class EmailLabelEntityTypeConfiguration : IEntityTypeConfiguration<EmailLabel>
{
    public void Configure(EntityTypeBuilder<EmailLabel> builder)
    {
        builder.ToTable("label");
        builder.Property<int>("id").ValueGeneratedOnAdd();
        builder.Property("email_id")
            .IsRequired();
        builder.HasKey("id");
        builder.Property(l => l.Name)
            .HasConversion<FilledStringToStringConverter>()
            .IsRequired()
            .HasColumnName("name");
        builder.Property(l => l.Value)
            .HasConversion<FilledStringToStringConverter>()
            .IsRequired()
            .HasColumnName("value");
    }
}