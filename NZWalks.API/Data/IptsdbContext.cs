using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domains;

namespace NZWalks.API.Data;

public partial class IptsdbContext : DbContext
{
    public IptsdbContext()
    {
    }

    public IptsdbContext(DbContextOptions<IptsdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DateConstant> DateConstants { get; set; }

    public virtual DbSet<HoldDate> HoldDates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=EG-3ZSJRG3;Initial Catalog=IPTSDb;Integrated Security=True;Pooling=False;Encrypt=False;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DateConstant>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<HoldDate>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BeginHold).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Release).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
