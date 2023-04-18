using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWorld.Shared.Entities;
using ShopWorldWeb.UI.Models;
using ShopWorldWeb.UI.Services;

namespace ShopWorldWeb.UI.Controllers
{
    [Authorize(AuthenticationSchemes=CookieAuthenticationDefaults.AuthenticationScheme,Roles ="Admin")]
    public class EmployeeController : Controller
    {
        private readonly ShopWorldClient _shopWorldClient;
        private readonly IMapper _mapper;
        public EmployeeController(ShopWorldClient shopWorldClient,IMapper mapper)
        {
            _shopWorldClient = shopWorldClient;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _shopWorldClient.Employee_GetAllEmployeesAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Employee employee = await _shopWorldClient.Employee_GetEmployeeAsync((int)id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeModel employee)
        {
            if (ModelState.IsValid)
            {
                await _shopWorldClient.Employee_AddEmployeeAsync(_mapper.Map<Employee>(employee));
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Employee employee = await _shopWorldClient.Employee_GetEmployeeAsync((int)id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeModel employee)
        {
            if (ModelState.IsValid)
            {
                await _shopWorldClient.Employee_UpdateEmployeeAsync(_mapper.Map<Employee>(employee));
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Employee employee = await _shopWorldClient.Employee_GetEmployeeAsync((int)id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _shopWorldClient.Employee_DeleteEmployeeAsync((int)id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("login_token");
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
