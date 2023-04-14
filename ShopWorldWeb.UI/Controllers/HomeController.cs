using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ShopWorld.Shared;
using ShopWorld.Shared.Entities;
using ShopWorldWeb.UI.Models;
using ShopWorldWeb.UI.Services;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace ShopWorldWeb.UI.Controllers
{
    public class HomeController : Controller
    {
        private ShopWorldClient _shopWorldClient;
        private readonly ILogger<HomeController> _logger;
        private IMapper _mapper { get; set; }
        public HomeController(ILogger<HomeController> logger,ShopWorldClient shopWorldClient,IMapper mapper)
        {
            _logger = logger;
            _shopWorldClient = shopWorldClient;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult LoginAsUser()
        {
            return RedirectToAction("Index", "Customer");
        }

        [HttpPost]
        public async Task<IActionResult> Login(CustomerLoginModel CustomerLoginModel)
        {
            LoginResult loginResult = await _shopWorldClient.Authorization_LoginAsync(new MobileLoginInputModel { MobileNumber=CustomerLoginModel.Mobile });
            JwtSecurityToken jwtSecurityToken = JwtTokenReader.GetJwtToken(loginResult.JwtToken);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                // Refreshing the authentication session should be allowed.

                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
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
            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(jwtSecurityToken.Claims, CookieAuthenticationDefaults.AuthenticationScheme)),
            authProperties);
            Response.Cookies.Append("login_token", loginResult.JwtToken);
            return RedirectToAction("LoginAsUser");
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(CustomerModel CustomerModel)
        {
            Customer customer = await _shopWorldClient.Customer_AddCustomerAsync(_mapper.Map<Customer>(CustomerModel));
            LoginResult loginResult=await _shopWorldClient.Authorization_LoginAsync(new MobileLoginInputModel { MobileNumber = CustomerModel.Mobile });
            JwtSecurityToken jwtSecurityToken = JwtTokenReader.GetJwtToken(loginResult.JwtToken);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                // Refreshing the authentication session should be allowed.

                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
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
            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(jwtSecurityToken.Claims, CookieAuthenticationDefaults.AuthenticationScheme)),
            authProperties);
            Response.Cookies.Append("login_token", loginResult.JwtToken);
            return RedirectToAction("LoginAsUser");
        }

        public async Task<IActionResult> LoginAsAdmin()
        {
            LoginResult loginResult=await _shopWorldClient.Authorization_LoginAsAdminAsync();
            JwtSecurityToken jwtSecurityToken = JwtTokenReader.GetJwtToken(loginResult.JwtToken);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                // Refreshing the authentication session should be allowed.

                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
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
            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(jwtSecurityToken.Claims, CookieAuthenticationDefaults.AuthenticationScheme)),
            authProperties);
            Response.Cookies.Append("login_token", loginResult.JwtToken);
            return RedirectToAction("Index", "Item");
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Unauthorized()
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            ViewBag.Status = (int)HttpStatusCode.Unauthorized;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}