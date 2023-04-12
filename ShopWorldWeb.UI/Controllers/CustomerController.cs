using Microsoft.AspNetCore.Mvc;

namespace ShopWorldWeb.UI.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
