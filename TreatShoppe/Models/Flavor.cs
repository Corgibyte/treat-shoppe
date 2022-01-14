using System.Collections.Generic;

namespace TreatShoppe.Models
{
  public class Flavor
  {
    public int FlavorId { get; set; }
    public string Name { get; set; }
    public ICollection<FlavorTreat> FlavorTreats { get; set; }

    public Flavor()
    {
      FlavorTreats = new HashSet<FlavorTreat>();
    }
  }
}