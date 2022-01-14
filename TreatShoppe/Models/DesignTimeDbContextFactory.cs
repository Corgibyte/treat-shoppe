using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TreatShoppe.Models
{
  public class TreatShoppeContextFactory : IDesignTimeDbContextFactory<TreatShoppeContext>
  {
    TreatShoppeContext IDesignTimeDbContextFactory<TreatShoppeContext>.CreateDbContext(string[] args)
    {
      IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();
      var builder = new DbContextOptionsBuilder<TreatShoppeContext>();

      builder.UseMySql(configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(configuration["ConnectionStrings:DefaultConnection"]));
      return new TreatShoppeContext(builder.Options);
    }
  }
}