using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopWorld.Shared;
using ShopWorld.Shared.Entities;
using ShopWorldWeb.UI.Models;
using ShopWorldWeb.UI.Services;

namespace ShopWorldWeb.UI.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
    public class ItemController : Controller
    {
        private ShopWorldClient _shopWorldClient;
        private IMapper _mapper { get; set; }
        public ItemController(ShopWorldClient shopWorldClient,IMapper mapper)
        {
            _shopWorldClient= shopWorldClient;
            _mapper=mapper;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _shopWorldClient.Item_GetAllItemsAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Item item = await _shopWorldClient.Item_GetItemAsync((int)id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemModel item)
        {
            if (ModelState.IsValid)
            {
                ItemInputModel itemInput=_mapper.Map<ItemInputModel>(item);
                await _shopWorldClient.Item_AddItemAsync(itemInput);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Item item = await _shopWorldClient.Item_GetItemAsync((int)id);
            
            if (item == null)
            {
                return NotFound();
            }
            ItemModel itemModel = _mapper.Map<ItemModel>(item);
            return View(itemModel);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ItemModel item)
        {
            if (ModelState.IsValid)
            {
                await _shopWorldClient.Item_UpdateItemAsync(_mapper.Map<ItemInputModel>(item));
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Item item = await _shopWorldClient.Item_GetItemAsync((int)id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _shopWorldClient.Item_DeleteItemAsync(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
