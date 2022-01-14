using Microsoft.AspNetCore.Mvc;

namespace TreatShoppe.Controllers
{
  public class HomeController : Controller
  {

    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }
  }
}