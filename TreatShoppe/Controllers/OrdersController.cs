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
      _db.Entry(treat).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = treat.TreatId });
    }

    public ActionResult Delete(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      _db.Treats.Remove(thisTreat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddFlavor(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult AddFlavor(Treat treat, int flavorId)
    {
      if (flavorId != 0)
      {
        _db.FlavorTreats.Add(new FlavorTreat() { FlavorId = flavorId, TreatId = treat.TreatId });
      }
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = treat.TreatId });
    }

    [HttpPost]
    public ActionResult DeleteFlavor(int flavorTreatId)
    {
      FlavorTreat thisFlavorTreat = _db.FlavorTreats.FirstOrDefault(flavorTreat => flavorTreat.FlavorTreatId == flavorTreatId);
      _db.FlavorTreats.Remove(thisFlavorTreat);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = thisFlavorTreat.TreatId });
    }
  }
}
