using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrainingManagement.AuthenticationApi.Models;

namespace TrainingManagement.AuthenticationApi.Data;

/// <summary>
/// Authentication database context
/// </summary>
public class ApplicationDbContext : IdentityDbContext<Microsoft.AspNetCore.Identity.IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    //public DbSet<User> CustomUsers { get; set; }
    //public DbSet<Role> Roles { get; set; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    base.OnModelCreating(modelBuilder);

    //    // Configure User entity
    //    modelBuilder.Entity<User>()
    //        .HasKey(u => u.Id);

    //    modelBuilder.Entity<User>()
    //        .Property(u => u.Username)
    //        .IsRequired()
    //        .HasMaxLength(100);

    //    modelBuilder.Entity<User>()
    //        .Property(u => u.Email)
    //        .IsRequired()
    //        .HasMaxLength(100);

    //    // Configure Role entity
    //    modelBuilder.Entity<Role>()
    //        .HasKey(r => r.Id);

    //    modelBuilder.Entity<Role>()
    //        .Property(r => r.Name)
    //        .IsRequired()
    //        .HasMaxLength(50);
    //}
}
