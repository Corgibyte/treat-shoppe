using Microsoft.AspNetCore.Identity;

namespace TreatShoppe.Models
{
  public class ApplicationUser : IdentityUser
  {
    public virtual IdentityRole role { get; set; }
  }
}