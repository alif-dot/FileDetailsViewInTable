using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FileDetailsViewTable.Models;

public partial class ProjectstestdbContext : DbContext
{
    public ProjectstestdbContext()
    {
    }

    public ProjectstestdbContext(DbContextOptions<ProjectstestdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FileTable> FileTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=ALIF\\SQLEXPRESS01;Database=projectstestdb;Trusted_Connection=true;Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FileTabl__3214EC078133D19A");

            entity.ToTable("FileTable");

            entity.Property(e => e.DateModified).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
