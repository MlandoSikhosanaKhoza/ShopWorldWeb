using Microsoft.AspNetCore.Mvc;

namespace ShopWorldWeb.UI.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
