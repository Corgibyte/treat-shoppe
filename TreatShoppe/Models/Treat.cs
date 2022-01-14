using System.Collections.Generic;

namespace TreatShoppe.Models
{
  public class Treat
  {
    public int TreatId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public virtual ICollection<OrderTreat> OrderTreats { get; set; }
    public virtual ICollection<FlavorTreat> FlavorTreats { get; set; }

    public Treat()
    {
      OrderTreats = new HashSet<OrderTreat>();
      FlavorTreats = new HashSet<FlavorTreat>();
    }
  }
}