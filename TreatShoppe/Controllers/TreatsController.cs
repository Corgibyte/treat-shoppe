using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TreatShoppe.Models;

namespace TreatShoppe.Controllers
{
  [Authorize]
  public class TreatsController : Controller
  {
    private readonly TreatShoppeContext _db;

    public TreatsController(TreatShoppeContext db)
    {
      _db = db;
    }

    [AllowAnonymous]
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

    [Authorize(Roles = "admin")]
    public ActionResult Create()
    {
      return View();
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public ActionResult Create(Treat treat)
    {
      _db.Treats.Add(treat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [Authorize(Roles = "admin")]
    public ActionResult Edit(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public ActionResult Edit(Treat treat)
    {
      _db.Entry(treat).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = treat.TreatId });
    }

    [Authorize(Roles = "admin")]
    public ActionResult Delete(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }

    [Authorize(Roles = "admin")]
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      _db.Treats.Remove(thisTreat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [Authorize(Roles = "admin")]
    public ActionResult AddFlavor(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      var query = _db.Flavors.Include(flavor => flavor.FlavorTreats)
        .ThenInclude(flavorTreat => flavorTreat.Treat)
        .Where(flavor => !flavor.FlavorTreats.Any(flavorTreat => flavorTreat.TreatId == id));
      ViewBag.FlavorId = new SelectList(query, "FlavorId", "Name");
      return View(thisTreat);
    }

    [Authorize(Roles = "admin")]
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

    [Authorize(Roles = "admin")]
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
