using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuctionHouse.Models;
using AuctionHouse.DAL.Abstract;

namespace AuctionHouse.Controllers
{
    public class ItemController : Controller
    {
        private readonly IRepository<Item> _itemRepository;
        private readonly IRepository<Seller> _sellerRepository;

        public ItemController(IRepository<Item> itemRepo, IRepository<Seller> sellerRepo)
        {
            _itemRepository = itemRepo;
            _sellerRepository = sellerRepo;
        }

        // GET: Item
        public async Task<IActionResult> Index()
        {
            var items = _itemRepository.GetAll();  // assumes Lazy Loading is enabled, otherwise use the one with includes
            return View(await items.ToListAsync());
        }

        // GET: Item/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = _itemRepository
                            .GetAll(i => i.Id == id.Value)
                            .SingleOrDefault();
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Item/Create
        public IActionResult Create()
        {
            ViewData["SellerId"] = GetSellerSellectList(); ;      // Older way of using the viewbag for select lists
            return View();
        }

        // POST: Item/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]          // We do NOT want to bind an ID coming from the user when doing a Create
        public async Task<IActionResult> Create([Bind("Name,Description,Condition,SellerId")] Item item)
        {
            if (ModelState.IsValid)
            {
                // We have a model that passes any validation we've specified
                try
                {
                    _itemRepository.AddOrUpdate(item);
                }
                catch (DbUpdateConcurrencyException e)
                {
                    // Shouldn't happen during a create, but it since it could be thrown
                    // we must handle it
                    ViewBag.Message = "A concurrency error occurred while trying to create the item.  Please try again.";
                    return View(item);
                }
                catch (DbUpdateException e)
                {
                    ViewBag.Message = "An unknown database error occurred while trying to create the item.  Please try again.";
                    return View(item);
                }
                return RedirectToAction("Index");
            }
            else
            {
                // uncomment the next line if you want to see the error modal
                //ViewBag.Message = "An unknown database error occurred while trying to create the item.  Please try again.";
                // return validation errors and keep the user on this page and POST method
                ViewData["SellerId"] = GetSellerSellectList();
                return View(item);
            }
        }

        // GET: Item/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = _itemRepository
                            .GetAll(i => i.Id == id.Value)
                            .SingleOrDefault();
            ViewData["SellerId"] = GetSellerSellectList();
            return View(item);
        }

        // POST: Item/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]                  // Now we do want to bind the Id coming in so we can use it to compare to the one from the query string
        public IActionResult Edit(int id, [Bind("Id,Name,Description,Condition,SellerId")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _itemRepository.AddOrUpdate(item);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_itemRepository.Exists(item.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;      // I left this as the template produced it.  Returns 5xx Server Error in this case.  Basically
                                    // meaning you will have to figure out what to do if we get to this point
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SellerId"] = GetSellerSellectList();
            return View(item);
        }

        // GET: Item/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = _itemRepository
                            .GetAll(i => i.Id == id.Value)
                            .SingleOrDefault();
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var item = _itemRepository
                            .GetAll(i => i.Id == id)
                            .SingleOrDefault();
            if (item != null)
            {
                try
                {
                    _itemRepository.Delete(item);
                }
                catch(Exception)
                {
                    throw;      // same here, you decide how to handle this
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private SelectList GetSellerSellectList()
        {
            var selectList = new SelectList(
                _sellerRepository.GetAll().Select(s => new { Text = $"{s.FirstName} {s.LastName}", Value = s.Id }),
                "Value", "Text");
            return selectList;
        }
    }
}
