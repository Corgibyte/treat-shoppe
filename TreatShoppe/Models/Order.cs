using System.Collections.Generic;

namespace TreatShoppe.Models
{
  public class Order
  {
    public int OrderId { get; set; }
    public virtual ICollection<OrderTreat> OrderTreats { get; set; }
    public int TotalPrice { get; set; }

    public Order()
    {
      OrderTreats = new HashSet<OrderTreat>();
    }
  }
}