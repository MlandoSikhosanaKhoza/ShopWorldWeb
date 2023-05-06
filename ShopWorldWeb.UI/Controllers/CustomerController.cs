using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ShopWorld.Shared;
using ShopWorld.Shared.Entities;
using ShopWorldWeb.UI.Models;
using ShopWorldWeb.UI.Services;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ShopWorldWeb.UI.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class CustomerController : Controller
    {
        private ShopWorldClient _shopWorldClient;
        private IMapper _mapper { get; set; }
        public CustomerController(ShopWorldClient shopWorldClient, IMapper mapper)
        {
            _shopWorldClient = shopWorldClient;
            _mapper = mapper;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            OrderModel orderModel = new OrderModel();
            if (User.IsInRole("Customer"))
            {
                
                if (User.Claims.Any(c=>c.Type=="CustomerId"))
                {
                    int CustomerId = int.Parse(User.Claims.First(c=>c.Type=="CustomerId").Value);

                    orderModel.Customer = _mapper.Map<CustomerModel>(await _shopWorldClient.Customer_GetCustomerByIdAsync(CustomerId));
                }
                else
                {
                    await HttpContext.SignOutAsync();
                }
            }
            orderModel.Items = (List<Item>)await _shopWorldClient.Item_GetAllItemsAsync();
            orderModel.OrderReference = Guid.NewGuid();
            orderModel.DateCreated = DateTime.Now;
            return View("MakeAnOrder", orderModel);
        }

        [AllowAnonymous]
        public async Task<IActionResult> CheckIfMobileExist(string Mobile, int CustomerId)
        {
            if (CustomerId != 0)
            {
                return Json(true);
            }
            return Json(!await _shopWorldClient.Customer_MobileNumberExistsAsync(Mobile));
        }

        [AllowAnonymous]
        public async Task<IActionResult> CheckIfMobileDoesntExistAsync(string Mobile)
        {
            return Json((await _shopWorldClient.Customer_MobileNumberExistsAsync(Mobile)));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PurchaseItems(OrderModel OrderModel, CustomerModel CustomerModel, int[] ItemId, int[] Quantity)
        {
            Customer customer = await _shopWorldClient.Customer_ConfigureCustomerAsync(_mapper.Map<Customer>(CustomerModel));
            ViewBag.NameSurname = customer.Name + " " + customer.Surname;
            OrderModel.CustomerId = customer.CustomerId;
           
            if (!User.IsInRole("Customer"))
            {
                LoginResult loginResult = await _shopWorldClient.Authorization_LoginAsync(new MobileLoginInputModel { MobileNumber = customer.Mobile });
                _shopWorldClient.GetHttpClient().DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult.JwtToken);
                JwtSecurityToken jwtSecurityToken = JwtTokenReader.GetJwtToken(loginResult.JwtToken);
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    // Refreshing the authentication session should be allowed.

                    ExpiresUtc = DateTime.UtcNow.AddDays(7),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    IssuedUtc = DateTime.UtcNow,
                    // The time at which the authentication ticket was issued.

                    RedirectUri = "/"
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };
                List<Claim> claims = new List<Claim>();
                claims.AddRange(jwtSecurityToken.Claims);
                claims.Add(new Claim("login_token", loginResult.JwtToken));
                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
                authProperties);
            }
            Order order = await _shopWorldClient.Order_AddOrderAsync(_mapper.Map<Order>(OrderModel));
            await _shopWorldClient.OrderItem_AddOrderItemsAsync(new OrderItemInputModel { OrderId = order.OrderId, ItemId = ItemId, Quantity = Quantity });
            OrderModel.OrderItemsView = (await _shopWorldClient.OrderItem_GetOrderViewItemsAsync(order.OrderId)).Select(oiv => new OrderItemsViewModel { Description = oiv.Description, Quantity = oiv.Quantity, Price = oiv.Price }).ToList();
            
            return RedirectToAction("ShowReceipt",new { id=order.OrderId });
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Customer")]
        public async Task<IActionResult> MyOrders()
        {
            int CustomerId = int.Parse(User.FindFirstValue("CustomerId"));
            Customer customer = await _shopWorldClient.Customer_GetCustomerByIdAsync(CustomerId);
            CustomerModel customerModel = new CustomerModel();
            CustomerHistoryModel customerHistoryModel = new CustomerHistoryModel();
            if (customer != null)
            {
                ViewBag.NameSurname = customer.Name + " " + customer.Surname;
                customerModel = _mapper.Map<CustomerModel>(customer);
                customerHistoryModel = new CustomerHistoryModel { CustomerId = customer.CustomerId, Name = customerModel.Name, Surname = customerModel.Surname, Mobile = customerModel.Mobile };
                customerHistoryModel.OngoingOrders = (List<Order>)await _shopWorldClient.Order_GetOngoingOrdersForCustomerAsync(customer.CustomerId);
                customerHistoryModel.CompleteOrders = (List<Order>)await _shopWorldClient.Order_GetCompleteOrdersForCustomerAsync(customer.CustomerId);
            }
            else
            {
                customerHistoryModel.OngoingOrders = new List<Order>();
                customerHistoryModel.CompleteOrders = new List<Order>();
            }
            return View("MyOrders", customerHistoryModel);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> MyOrders(CustomerModel CustomerModel)
        {
            Customer customer = await _shopWorldClient.Customer_GetCustomerByMobileNumberAsync(CustomerModel.Mobile);
            CustomerHistoryModel customerHistoryModel = new CustomerHistoryModel();
            if (customer != null)
            {
                CustomerModel = _mapper.Map<CustomerModel>(customer);
                customerHistoryModel = new CustomerHistoryModel { Name = CustomerModel.Name, Surname = CustomerModel.Surname, Mobile = CustomerModel.Mobile };
                customerHistoryModel.OngoingOrders = (List<Order>)await _shopWorldClient.Order_GetOngoingOrdersForCustomerAsync(customer.CustomerId);
                customerHistoryModel.CompleteOrders = (List<Order>)await _shopWorldClient.Order_GetCompleteOrdersForCustomerAsync(customer.CustomerId);
            }
            else
            {
                customerHistoryModel.OngoingOrders = new List<Order>();
                customerHistoryModel.CompleteOrders = new List<Order>();
            }
            return View("MyOrders", customerHistoryModel);
        }
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Customer")]
        public async Task<IActionResult> ShowReceipt(int id)
        {
            Order order = await _shopWorldClient.Order_GetOrderAsync(id);
            Customer customer = await _shopWorldClient.Customer_GetCustomerByIdAsync(order.CustomerId);
            ViewBag.NameSurname = customer.Name + " " + customer.Surname;
            List<OrderItemsViewModel> orderItems = (await _shopWorldClient.OrderItem_GetOrderViewItemsAsync(order.OrderId)).Select(oiv => new OrderItemsViewModel { Description = oiv.Description, Quantity = oiv.Quantity, Price = oiv.Price }).ToList();
            OrderModel orderModel = _mapper.Map<OrderModel>(order);
            orderModel.Customer = _mapper.Map<CustomerModel>(customer);
            orderModel.OrderItemsView = orderItems;
            return View("Reciept", orderModel);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
