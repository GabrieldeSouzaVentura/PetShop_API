using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetShop.Models;

namespace PetShop.PetShop.Infrastructure.Context;

public class AppDbContext : IdentityDbContext <ApplicationUser, IdentityRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tutor>? Tutors { get; set; }
    public DbSet<Pet>? Pets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tutor>()
            .HasOne(t => t.User)
            .WithOne(u => u.Tutor)
            .HasForeignKey<Tutor>(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}
