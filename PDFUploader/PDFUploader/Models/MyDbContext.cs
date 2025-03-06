using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PDFUploader.Models;

public partial class MyDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public MyDbContext(IConfiguration configuration, DbContextOptions<MyDbContext> options)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<FileMetadatum> FileMetadata { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration.GetConnectionString("ERP");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileMetadatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FileMeta__3214EC0745735B8B");

            entity.Property(e => e.DateUploaded).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
