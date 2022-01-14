using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TreatShoppe.Models
{
  public class Order
  {
    public int OrderId { get; set; }
    public virtual ICollection<OrderTreat> OrderTreats { get; set; }
    public int TotalPrice { get; set; }
    [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime DeliveryDate { get; set; }

    public Order()
    {
      OrderTreats = new HashSet<OrderTreat>();
    }

    public void RecalculateTotalPrice()
    {
      int total = 0;
      foreach (OrderTreat orderTreat in OrderTreats)
      {
        total += orderTreat.Treat.Price;
      }
      TotalPrice = total;
    }
  }
}