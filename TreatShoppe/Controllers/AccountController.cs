using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TreatShoppe.Models;
using TreatShoppe.ViewModels;

namespace TreatShoppe.Controllers
{
  public class AccountController : Controller
  {
    private readonly TreatShoppeContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, TreatShoppeContext db)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _roleManager = roleManager;
      _db = db;
    }

    public ActionResult Index()
    {
      return View();
    }

    public IActionResult Register()
    {
      ViewBag.RoleName = new SelectList(_roleManager.Roles, "Name", "Name");
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterViewModel model, string RoleName)
    {
      ApplicationUser user = new ApplicationUser { UserName = model.Email };
      IdentityResult result = await _userManager.CreateAsync(user, model.Password);
      if (result.Succeeded)
      {
        await _userManager.AddToRoleAsync(user, RoleName);
        return RedirectToAction("Index");
      }
      else
      {
        return View();
      }
    }

    public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
      Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
      if (result.Succeeded)
      {
        return RedirectToAction("Index");
      }
      else
      {
        return View();
      }
    }

    [HttpPost]
    public async Task<ActionResult> LogOff()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index");
    }

    public ActionResult AddRole()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> AddRole(IdentityRole role)
    {
      await _roleManager.CreateAsync(role);
      return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied()
    {
      return View();
    }
  }
}