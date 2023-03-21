using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Standups.Models;

namespace Standups.Data;

public partial class StandupsDbContext : DbContext
{
    public StandupsDbContext()
    {
    }

    public StandupsDbContext(DbContextOptions<StandupsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SupacademicYear> SupacademicYears { get; set; }

    public virtual DbSet<Supadvisor> Supadvisors { get; set; }

    public virtual DbSet<Supcomment> Supcomments { get; set; }

    public virtual DbSet<SupcommentRating> SupcommentRatings { get; set; }

    public virtual DbSet<Supgroup> Supgroups { get; set; }

    public virtual DbSet<Supmeeting> Supmeetings { get; set; }

    public virtual DbSet<Supquestion> Supquestions { get; set; }

    public virtual DbSet<Supuser> Supusers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ApplicationConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SupacademicYear>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.SUPAcademicYears");

            entity.Property(e => e.Year).IsFixedLength();
        });

        modelBuilder.Entity<Supadvisor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.SUPAdvisors");
        });

        modelBuilder.Entity<Supcomment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.SUPComments");

            entity.HasOne(d => d.Supquestion).WithMany(p => p.Supcomments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.SUPComments_dbo.SUPQuestions_ID");
        });

        modelBuilder.Entity<SupcommentRating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.SUPCommentRatings");

            entity.HasOne(d => d.Supcomment).WithMany(p => p.SupcommentRatings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.SUPCommentRatings_dbo.SUPComments_ID");

            entity.HasOne(d => d.SupraterUser).WithMany(p => p.SupcommentRatings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.SUPCommentRatings_dbo.SUPUsers_ID");
        });

        modelBuilder.Entity<Supgroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.SUPGroups");

            entity.HasOne(d => d.SupacademicYear).WithMany(p => p.Supgroups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.SUPGroups_dbo.SUPAcademicYears_ID");

            entity.HasOne(d => d.Supadvisor).WithMany(p => p.Supgroups).HasConstraintName("FK_dbo.SUPGroups_dbo.SUPAdvisors_ID");
        });

        modelBuilder.Entity<Supmeeting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.SUPMeetings");

            entity.HasOne(d => d.Supuser).WithMany(p => p.Supmeetings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.SUPMeetings_dbo.SUPUsers_ID");
        });

        modelBuilder.Entity<Supquestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.SUPQuestions");
        });

        modelBuilder.Entity<Supuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.SUPUsers");

            entity.HasOne(d => d.Supgroup).WithMany(p => p.Supusers).HasConstraintName("FK_dbo.SUPUsers_dbo.SUPGroups_ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
