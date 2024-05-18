using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopWorld.Shared.Entities;
using ShopWorldWeb.UI.Models;
using ShopWorldWeb.UI.Security;
using ShopWorldWeb.UI.Services;
using System.Data;

namespace ShopWorldWeb.UI.Controllers
{
    [CustomAuthorization(Roles = "Admin")]
    public class OrderController : Controller
    {
        private ShopWorldClient _shopWorldClient;
        private IMapper _mapper { get; set; }
        public OrderController(ShopWorldClient shopWorldClient, IMapper mapper)
        {
            _shopWorldClient = shopWorldClient;
            _mapper = mapper;
        }
        public async  Task<IActionResult> Index()
        {
            ViewBag.OngoingOrders  = await _shopWorldClient.Order_GetOngoingOrdersAsync();
            ViewBag.CompleteOrders = await _shopWorldClient.Order_GetCompleteOrdersAsync();
            return View("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Order order                          = await _shopWorldClient.Order_GetOrderAsync((int)id);
            Customer customer                    = await _shopWorldClient.Customer_GetCustomerByIdAsync(order.CustomerId);
            ViewBag.NameSurname                  = customer.Name + " " + customer.Surname;
            List<OrderItemsViewModel> orderItems = (await _shopWorldClient.OrderItem_GetOrderViewItemsAsync(order.OrderId)).Select(oiv => new OrderItemsViewModel { Description = oiv.Description, Quantity = oiv.Quantity, Price = oiv.Price }).ToList();
            OrderModel orderModel                = _mapper.Map<OrderModel>(order);
            orderModel.Customer                  = _mapper.Map<CustomerModel>(customer);
            orderModel.OrderItemsView            = orderItems;
            ViewBag.EmployeeId                   = new SelectList((await _shopWorldClient.Employee_GetAllEmployeesAsync()).Select(n => new { Id = n.EmployeeId, FullName = n.Name + " " + n.Surname }), "Id", "FullName", order.EmployeeId);
            return View("Fulfill", orderModel);
        }
        [HttpPost]
        public async Task<IActionResult> FulfillOrder(int OrderId, int EmployeeId)
        {
            Order order         = await _shopWorldClient.Order_GetOrderAsync(OrderId);
            order.EmployeeId    = EmployeeId;
            order.DateFulfilled = DateTime.Now;
            await _shopWorldClient.Order_UpdateOrderAsync(order);
            return RedirectToAction("Index");
        }
    }
}
