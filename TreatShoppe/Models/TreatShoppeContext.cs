using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TreatShoppe.Models
{
  public class TreatShoppeContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<Treat> Treats { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Flavor> Flavors { get; set; }
    public DbSet<FlavorTreat> FlavorTreats { get; set; }
    public DbSet<OrderTreat> OrderTreats { get; set; }

    public TreatShoppeContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      string adminRoleId = "22e5205f-9162-400e-8421-b05e265d248d";
      string userRoleId = "4735b809-a800-4ed1-80fc-f4a54737485f";
      builder.Entity<IdentityRole>()
        .HasData(
          new IdentityRole
          {
            Id = adminRoleId,
            Name = "admin",
            NormalizedName = "ADMIN"
          },
          new IdentityRole
          {
            Id = userRoleId,
            Name = "user",
            NormalizedName = "USER"
          }
        );
    }
  }
}