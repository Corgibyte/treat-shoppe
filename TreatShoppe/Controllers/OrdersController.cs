using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TreatShoppe.Models;

namespace TreatShoppe.Controllers
{
  [Authorize]
  public class OrdersController : Controller
  {
    private readonly TreatShoppeContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrdersController(UserManager<ApplicationUser> userManager, TreatShoppeContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public async Task<ActionResult> Index()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      var userOrders = _db.Orders.Where(order => order.User.Id == currentUser.Id).ToList();
      return View(userOrders);
    }

    [Authorize(Roles = "admin")]
    public ActionResult Manage()
    {
      return View(_db.Orders.OrderBy(order => order.DeliveryDate));
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
    public async Task<ActionResult> Create(Order order)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      order.User = currentUser;
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
        _db.SaveChanges();
        int id = order.OrderId;
        order = _db.Orders.Include(order => order.OrderTreats)
          .ThenInclude(orderTreat => orderTreat.Treat)
          .FirstOrDefault(order => order.OrderId == id);
        order.RecalculateTotalPrice();
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = order.OrderId });
    }

    [HttpPost]
    public ActionResult DeleteTreat(int orderTreatId)
    {
      OrderTreat thisOrderTreat = _db.OrderTreats
        .Include(orderTreat => orderTreat.Order)
        .ThenInclude(order => order.OrderTreats)
        .ThenInclude(orderTreat => orderTreat.Treat)
        .FirstOrDefault(orderTreat => orderTreat.OrderTreatId == orderTreatId);
      _db.OrderTreats.Remove(thisOrderTreat);
      _db.SaveChanges();
      thisOrderTreat.Order.RecalculateTotalPrice();
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = thisOrderTreat.OrderId });
    }
  }
}
