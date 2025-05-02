using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Telefonkönyv.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Citys { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Picture> Pictures { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=MyAppDb;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__Citys__DE9CEC38AA071FBB");

            entity.Property(e => e.CityId).HasColumnName("City_id");
            entity.Property(e => e.CityName)
                .HasMaxLength(100)
                .HasColumnName("City_name");
            entity.Property(e => e.Irsz).HasMaxLength(20);
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__contact__3213E83FD45270D0");

            entity.ToTable("contact");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CityId).HasColumnName("City_id");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Nickname).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.PictureId).HasColumnName("picture_id");
            entity.Property(e => e.UploaderId).HasColumnName("Uploader_id");

            entity.HasOne(d => d.City).WithMany(p => p.Contacts)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__contact__City_id__4222D4EF");

            entity.HasOne(d => d.Picture).WithMany(p => p.Contacts)
                .HasForeignKey(d => d.PictureId)
                .HasConstraintName("FK__contact__picture__440B1D61");

            entity.HasOne(d => d.Uploader).WithMany(p => p.Contacts)
                .HasForeignKey(d => d.UploaderId)
                .HasConstraintName("FK__contact__Uploade__4316F928");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__logs__9E2397E01C89F417");

            entity.ToTable("logs");

            entity.Property(e => e.LogId).HasColumnName("log_id");
            entity.Property(e => e.Operation)
                .HasMaxLength(100)
                .HasColumnName("operation");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("timestamp");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Logs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__logs__user_id__46E78A0C");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__Permissi__E5331AFAFDE954B4");

            entity.HasIndex(e => e.PermissionName, "UQ__Permissi__81C0F5A27D190FAC").IsUnique();

            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.PermissionName)
                .HasMaxLength(50)
                .HasColumnName("permission_name");
        });

        modelBuilder.Entity<Picture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pictures__3213E83F4E851838");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Picture1).HasColumnName("picture");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user__3213E83FE1CA24AB");

            entity.ToTable("user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Password)
                .HasMaxLength(64)
                .HasColumnName("password");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasOne(d => d.Permission).WithMany(p => p.Users)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user__permission__3C69FB99");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
