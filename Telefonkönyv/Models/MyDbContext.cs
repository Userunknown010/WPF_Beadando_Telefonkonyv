using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=MyAppDb;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__Citys__DE9CEC38B57EA806");

            entity.Property(e => e.CityId)
                .ValueGeneratedNever()
                .HasColumnName("City_id");
            entity.Property(e => e.CityName)
                .HasMaxLength(100)
                .HasColumnName("City_name");
            entity.Property(e => e.Irsz).HasMaxLength(20);
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__contact__3213E83F3E48808D");

            entity.ToTable("contact");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
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
            entity.Property(e => e.UploaderId).HasColumnName("Uploader_id");

            entity.HasOne(d => d.City).WithMany(p => p.Contacts)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__contact__City_id__403A8C7D");

            entity.HasOne(d => d.Uploader).WithMany(p => p.Contacts)
                .HasForeignKey(d => d.UploaderId)
                .HasConstraintName("FK__contact__Uploade__412EB0B6");

            entity.HasMany(d => d.Pictures).WithMany(p => p.Contacts)
                .UsingEntity<Dictionary<string, object>>(
                    "ContactPicture",
                    r => r.HasOne<Picture>().WithMany()
                        .HasForeignKey("PictureId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ContactPi__pictu__46E78A0C"),
                    l => l.HasOne<Contact>().WithMany()
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ContactPi__conta__45F365D3"),
                    j =>
                    {
                        j.HasKey("ContactId", "PictureId").HasName("PK__ContactP__D99CFB16871034DB");
                        j.ToTable("ContactPictures");
                        j.IndexerProperty<int>("ContactId").HasColumnName("contact_id");
                        j.IndexerProperty<int>("PictureId").HasColumnName("picture_id");
                    });
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__logs__9E2397E0BFAD3ED9");

            entity.ToTable("logs");

            entity.Property(e => e.LogId)
                .ValueGeneratedNever()
                .HasColumnName("log_id");
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
                .HasConstraintName("FK__logs__user_id__49C3F6B7");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__Permissi__E5331AFA2856D261");

            entity.HasIndex(e => e.PermissionName, "UQ__Permissi__81C0F5A2DC0C9CBE").IsUnique();

            entity.Property(e => e.PermissionId)
                .ValueGeneratedNever()
                .HasColumnName("permission_id");
            entity.Property(e => e.PermissionName)
                .HasMaxLength(50)
                .HasColumnName("permission_name");
        });

        modelBuilder.Entity<Picture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pictures__3213E83FFF9D979E");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Picture1).HasColumnName("picture");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user__3213E83F67FADC84");

            entity.ToTable("user");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
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
