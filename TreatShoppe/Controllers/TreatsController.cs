using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TreatShoppe.Models;

namespace TreatShoppe.Controllers
{
  public class TreatsController : Controller
  {
    private readonly TreatShoppeContext _db;

    public TreatsController(TreatShoppeContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Treats.ToList());
    }

    public ActionResult Details(int id)
    {
      Treat thisTreat = _db.Treats
        .Include(treat => treat.FlavorTreats)
        .ThenInclude(flavorTreat => flavorTreat.Flavor)
        .FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Treat treat)
    {
      _db.Treats.Add(treat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Edit(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult Edit(Treat treat)
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
