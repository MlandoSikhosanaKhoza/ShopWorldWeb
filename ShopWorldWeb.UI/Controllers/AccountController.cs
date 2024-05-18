using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopWorldWeb.UI.Security;
using System.Security.Claims;

namespace ShopWorldWeb.UI.Controllers
{
    [CustomAuthorization]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public IActionResult Login()
        {
            ViewBag.UserString = User.FindFirstValue(ClaimTypes.Role);
            return View();
        }
    }
}
