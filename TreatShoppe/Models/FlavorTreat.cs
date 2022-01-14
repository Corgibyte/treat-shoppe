using System.Collections.Generic;

namespace TreatShoppe.Models
{
  public class FlavorTreat
  {
    public int FlavorTreatId { get; set; }
    public int TreatId { get; set; }
    public int FlavorId { get; set; }
    public virtual ICollection<Treat> Treats { get; set; }
    public virtual ICollection<Flavor> Flavors { get; set; }

    public FlavorTreat()
    {
      Treats = new HashSet<Treat>();
      Flavors = new HashSet<Flavor>();
    }
  }
}