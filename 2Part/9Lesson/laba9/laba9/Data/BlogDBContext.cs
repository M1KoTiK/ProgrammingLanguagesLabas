using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using laba9.Models;

namespace laba9.Data;

public partial class BlogDBContext : DbContext
{
    public BlogDBContext()
    {
    }

    public BlogDBContext(DbContextOptions<BlogDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BlogRecord> BlogRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlogRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BlogReco__3214EC079C60AAC3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
