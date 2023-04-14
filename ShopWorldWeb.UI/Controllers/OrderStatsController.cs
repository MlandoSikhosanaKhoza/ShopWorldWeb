using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopWorldWeb.UI.Services;

namespace ShopWorldWeb.UI.Controllers
{
    public class OrderStatsController : Controller
    {
        private ShopWorldClient _shopWorldClient;
        public OrderStatsController(ShopWorldClient shopWorldClient)
        {
            _shopWorldClient = shopWorldClient;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.CustomerNumOrdersList = await _shopWorldClient.Order_GetNumberOfCustomerOrdersAsync();
            ViewBag.CustomerTotalSpentList = await _shopWorldClient.Order_GetTotalSpentOfCustomerOrdersAsync();
            ViewBag.CustomerAverageSpentList = await _shopWorldClient.Order_GetAverageSpentOfCustomerOrdersAsync();
            return View();
        }
    }
}
