using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SimpleApp.Models;

namespace SimpleApp.Data;

public partial class SimpleDbContext : DbContext
{
    public SimpleDbContext()
    {
    }

    public SimpleDbContext(DbContextOptions<SimpleDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserLog> UserLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=AppConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserLog__3214EC27DA8CCA6C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
