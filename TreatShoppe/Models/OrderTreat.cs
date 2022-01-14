using System.Collections.Generic;

namespace TreatShoppe.Models
{
  public class OrderTreat
  {
    public int OrderTreatId { get; set; }
    public int OrderId { get; set; }
    public int TreatId { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
    public virtual ICollection<Treat> Treats { get; set; }

    public OrderTreat()
    {
      Orders = new HashSet<Order>();
      Treats = new HashSet<Treat>();
    }
  }
}