using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GibiSu.Data;
using GibiSu.Models;
using System.Security.Claims;

namespace GibiSu.Controllers
{
    public class OrderProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationUser _context2;

        public OrderProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OrderProducts
        public async Task<IActionResult> Index()
        {
            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var applicationDbContext = _context.OrderProducts.Include(o => o.Order).Include(o => o.Product).Where(o => o.Order.OrderDate == null).Where(o => o.Order.UserId == userName);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: OrderProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderProducts == null)
            {
                return NotFound();
            }

            var orderProduct = await _context.OrderProducts
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (orderProduct == null)
            {
                return NotFound();
            }

            return View(orderProduct);
        }

        // GET: OrderProducts/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Address");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description");
            return View();
        }

        // GET: OrderProducts/Create
        public IActionResult AddCart(int productId, int amount)
        {
            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Order newOrder = _context.Orders.Where(o => o.UserId == userName).Where(o => o.OrderDate == null).FirstOrDefault();
            Product product = _context.Products.Where(p => p.Id == productId).FirstOrDefault();
            if (newOrder == null)
            {
                newOrder = new Order();
                newOrder.TotalPrice = 0;
                newOrder.OrderDate = null;
                newOrder.Address = "boş";
                newOrder.PhoneNumber = "boş";
                newOrder.UserId = userName;
                if (ModelState.IsValid)
                {
                    _context.Add(newOrder);
                    _context.SaveChanges();
                }
            }
            //int ProductCount = _context.OrderProducts.Where(o => o.OrderId == newOrder.Id).Count();
            OrderProduct cart = _context.OrderProducts.Where(o => o.ProductId == productId).Where(o => o.OrderId == newOrder.Id).FirstOrDefault();
            if (cart == null)
            {
                cart = new OrderProduct();
                cart.ProductId = productId;
                cart.OrderId = newOrder.Id;
                cart.Price = product.Price;
                cart.Amount = amount;
                if (ModelState.IsValid)
                {
                    _context.Add(cart);
                }
            }
            else
            {
                cart.Amount+=amount;
            }
            cart.TotalPrice = product.Price * cart.Amount;
            newOrder.TotalPrice = newOrder.TotalPrice + product.Price;
            _context.SaveChanges();

            return View();
        }

        // POST: OrderProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,OrderId,Amount,Price,TotalPrice")] OrderProduct orderProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Address", orderProduct.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description", orderProduct.ProductId);
            return View(orderProduct);
        }

        // GET: OrderProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrderProducts == null)
            {
                return NotFound();
            }

            var orderProduct = await _context.OrderProducts.FindAsync(id);
            if (orderProduct == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Address", orderProduct.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description", orderProduct.ProductId);
            return View(orderProduct);
        }

        // POST: OrderProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,OrderId,Amount,Price,TotalPrice")] OrderProduct orderProduct)
        {
            if (id != orderProduct.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderProductExists(orderProduct.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Address", orderProduct.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description", orderProduct.ProductId);
            return View(orderProduct);
        }

        // GET: OrderProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrderProducts == null)
            {
                return NotFound();
            }

            var orderProduct = await _context.OrderProducts
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (orderProduct == null)
            {
                return NotFound();
            }

            return View(orderProduct);
        }

        // POST: OrderProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrderProducts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.OrderProducts'  is null.");
            }
            var orderProduct = await _context.OrderProducts.FindAsync(id);
            if (orderProduct != null)
            {
                _context.OrderProducts.Remove(orderProduct);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderProductExists(int id)
        {
          return _context.OrderProducts.Any(e => e.ProductId == id);
        }
        public int CountPlus(int productid)
        {
            OrderProduct orderProduct = _context.OrderProducts.Where(p => p.ProductId == productid).FirstOrDefault();

            orderProduct.Amount = orderProduct.Amount + 1;
            _context.Update(orderProduct);
            _context.SaveChanges();
            return orderProduct.Amount;
        } 
        public int CountMinus(int productid)
        {
            OrderProduct orderProduct = _context.OrderProducts.Where(p => p.ProductId == productid).FirstOrDefault();
            if (orderProduct.Amount > 0)
            {
                orderProduct.Amount = orderProduct.Amount - 1;
            }
            _context.Update(orderProduct);
            _context.SaveChanges();
            return orderProduct.Amount;
        }
    }
}
