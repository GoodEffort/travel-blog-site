using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using travel_blog_api.Models;

namespace travel_blog_api.Context;

public partial class TravelblogContext : DbContext
{
    public TravelblogContext()
    {
    }

    public TravelblogContext(DbContextOptions<TravelblogContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Photo> Photos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("photos");

            entity.Property(e => e.Id)
                .HasColumnType("mediumint")
                .HasColumnName("id");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.PhotoName).HasMaxLength(255);
            entity.Property(e => e.TripName).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
