using Microsoft.AspNetCore.Mvc;

namespace ShopWorldWeb.UI.Controllers
{
    public class ItemController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
