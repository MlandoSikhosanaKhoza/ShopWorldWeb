using Microsoft.AspNetCore.Mvc;

namespace ShopWorldWeb.UI.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
