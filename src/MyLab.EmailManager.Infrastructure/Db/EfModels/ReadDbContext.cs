using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyLab.EmailManager.Infrastructure.Db.EfModels;

public partial class ReadDbContext : DbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DbConfirmation> Confirmations { get; set; }

    public virtual DbSet<DbEmail> Emails { get; set; }

    public virtual DbSet<DbLabel> Labels { get; set; }

    public virtual DbSet<DbMessage> Messages { get; set; }

    public virtual DbSet<DbSending> Sendings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<DbConfirmation>(entity =>
        {
            entity.HasKey(e => e.EmailId).HasName("PRIMARY");

            entity.Property(e => e.EmailId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<DbEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<DbLabel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<DbMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<DbSending>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
