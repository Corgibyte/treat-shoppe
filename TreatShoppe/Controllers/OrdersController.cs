using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TreatShoppe.Models;

namespace TreatShoppe.Controllers
{
  public class OrdersController : Controller
  {
    private readonly TreatShoppeContext _db;

    public OrdersController(TreatShoppeContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Orders.ToList());
    }

    public ActionResult Details(int id)
    {
      Order thisOrder = _db.Orders
        .Include(order => order.OrderTreats)
        .ThenInclude(orderTreat => orderTreat.Treat)
        .FirstOrDefault(order => order.OrderId == id);
      return View(thisOrder);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Order order)
    {
      _db.Orders.Add(order);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Edit(int id)
    {
      Order thisOrder = _db.Orders.FirstOrDefault(order => order.OrderId == id);
      return View(thisOrder);
    }

    [HttpPost]
    public ActionResult Edit(Order order)
    {
      _db.Entry(order).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = order.OrderId });
    }

    public ActionResult Delete(int id)
    {
      Order thisOrder = _db.Orders.FirstOrDefault(order => order.OrderId == id);
      return View(thisOrder);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Order thisOrder = _db.Orders.FirstOrDefault(order => order.OrderId == id);
      _db.Orders.Remove(thisOrder);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddTreat(int id)
    {
      Order thisOrder = _db.Orders.FirstOrDefault(order => order.OrderId == id);
      ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
      return View(thisOrder);
    }

    [HttpPost]
    public ActionResult AddTreat(Order order, int treatId)
    {
      if (treatId != 0)
      {
        _db.OrderTreats.Add(new OrderTreat() { TreatId = treatId, OrderId = order.OrderId });
        order.RecalculateTotalPrice();
        _db.Entry(order).State = EntityState.Modified;
      }
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = order.OrderId });
    }

    [HttpPost]
    public ActionResult DeleteTreat(int orderTreatId)
    {
      OrderTreat thisOrderTreat = _db.OrderTreats.FirstOrDefault(orderTreat => orderTreat.OrderTreatId == orderTreatId);
      _db.OrderTreats.Remove(thisOrderTreat);
      thisOrderTreat.Order.RecalculateTotalPrice();
      _db.Entry(thisOrderTreat.Order).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = thisOrderTreat.OrderId });
    }
  }
}
