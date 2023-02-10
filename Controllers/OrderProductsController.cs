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
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;

namespace GibiSu.Controllers
{
    [Authorize]
    public class OrderProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        UserManager<ApplicationUser> _userManager;

        public OrderProductsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: OrderProducts
        public async Task<IActionResult> Index()
        {
            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var applicationDbContext = _context.OrderProducts.Include(o => o.Order).Include(o => o.Product).Where(o => o.Order.OrderDate == null).Where(o => o.Order.UserId == userName);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<PartialViewResult> Reports(int? SelectProductId, string? SelectUser, DateTime? FirstOrderDate, DateTime? LastOrderDate)
        {
            //Seçili herhangi bir seçenek varsa
            if (SelectUser != "undefined" || SelectProductId != null || FirstOrderDate != null || LastOrderDate != null)
            {
                //Sadece Ürün Seçiliyse
                if (SelectProductId != null && SelectUser == "undefined" && FirstOrderDate == null && LastOrderDate == null) {
                    var applicationDbContext = _context.OrderProducts.Include(o => o.Order).Include(o => o.Order.User)
                        .Where(o => o.ProductId == SelectProductId).Where(o=>o.Order.OrderDate != null);
                    return PartialView(await applicationDbContext.ToListAsync());
                }
                //Sadece Kullanıcı Seçiliyse
                if (SelectProductId == null && SelectUser != "undefined" && FirstOrderDate == null && LastOrderDate == null)
                {
                    var applicationDbContext = _context.OrderProducts.Include(o => o.Order).Include(o => o.Order.User)
                        .Where(o => o.Order.UserId == SelectUser).Where(o => o.Order.OrderDate != null);
                    return PartialView(await applicationDbContext.ToListAsync());
                }
                //Sadece Başlangıç Tarihi Seçiliyse
                if (SelectProductId == null && SelectUser == "undefined" && FirstOrderDate != null && LastOrderDate == null)
                {
                    var applicationDbContext = _context.OrderProducts.Include(o => o.Order).Include(o => o.Order.User)
                        .Where(o => o.Order.OrderDate >= FirstOrderDate).Where(o => o.Order.OrderDate != null);
                    return PartialView(await applicationDbContext.ToListAsync());
                }
                //Sadece Bitiş Tarihi Seçiliyse
                if (SelectProductId == null && SelectUser == "undefined" && FirstOrderDate == null && LastOrderDate != null)
                {
                    var applicationDbContext = _context.OrderProducts.Include(o => o.Order).Include(o => o.Order.User)
                        .Where(o => o.Order.OrderDate <= LastOrderDate).Where(o => o.Order.OrderDate != null);
                    return PartialView(await applicationDbContext.ToListAsync());
                }
                //Sadece Kullanıcı ve Ürün Seçiliyse
                if (SelectProductId != null && SelectUser != "undefined" && FirstOrderDate == null && LastOrderDate == null)
                {
                    var applicationDbContext = _context.OrderProducts.Include(o => o.Order).Include(o => o.Order.User)
                        .Where(o => o.Order.UserId == SelectUser).Where(o => o.ProductId == SelectProductId).Where(o => o.Order.OrderDate != null);
                    return PartialView(await applicationDbContext.ToListAsync());
                }
                //Sadece Kullanıcı ve Başlangıç Tarihi Seçiliyse
                if (SelectProductId == null && SelectUser != "undefined" && FirstOrderDate != null && LastOrderDate == null)
                {
                    var applicationDbContext = _context.OrderProducts.Include(o => o.Order).Include(o => o.Order.User)
                        .Where(o => o.Order.UserId == SelectUser).Where(o => o.Order.OrderDate >= FirstOrderDate).Where(o => o.Order.OrderDate != null);
                    return PartialView(await applicationDbContext.ToListAsync());
                }
                //Sadece Kullanıcı ve Bitiş Tarihi Seçiliyse
                if (SelectProductId == null && SelectUser != "undefined" && FirstOrderDate == null && LastOrderDate != null)
                {
                    var applicationDbContext = _context.OrderProducts.Include(o => o.Order).Include(o => o.Order.User)
                        .Where(o => o.Order.UserId == SelectUser).Where(o => o.Order.OrderDate <= LastOrderDate).Where(o => o.Order.OrderDate != null);
                    return PartialView(await applicationDbContext.ToListAsync());
                }
                //Sadece Ürün ve Başlangıç Tarihi Seçiliyse
                if (SelectProductId != null && SelectUser == "undefined" && FirstOrderDate != null && LastOrderDate == null)
                {
                    var applicationDbContext = _context.OrderProducts.Include(o => o.Order).Include(o => o.Order.User)
                        .Where(o => o.ProductId == SelectProductId).Where(o => o.Order.OrderDate >= FirstOrderDate).Where(o => o.Order.OrderDate != null);
                    return PartialView(await applicationDbContext.ToListAsync());
                }
                //Sadece Ürün ve Bitiş Tarihi Seçiliyse
                if (SelectProductId != null && SelectUser == "undefined" && FirstOrderDate == null && LastOrderDate != null)
                {
                    var applicationDbContext = _context.OrderProducts.Include(o => o.Order).Include(o => o.Order.User)
                        .Where(o => o.ProductId == SelectProductId).Where(o => o.Order.OrderDate <= LastOrderDate).Where(o => o.Order.OrderDate != null);
                    return PartialView(await applicationDbContext.ToListAsync());
                }
                //Sadece Başlangıç ve Bitiş Tarihi Seçiliyse
                if (SelectProductId == null && SelectUser == "undefined" && FirstOrderDate != null && LastOrderDate != null)
                {
                    var applicationDbContext = _context.OrderProducts.Include(o => o.Order).Include(o => o.Order.User)
                        .Where(o => o.Order.OrderDate >= FirstOrderDate).Where(o => o.Order.OrderDate <= LastOrderDate).Where(o => o.Order.OrderDate != null);
                    return PartialView(await applicationDbContext.ToListAsync());
                }
                //Sadece Kullanıcı, Ürün, Başlangıç Tarihi Seçiliyse
                if (SelectProductId != null && SelectUser != "undefined" && FirstOrderDate != null && LastOrderDate == null)
                {
                    var applicationDbContext = _context.OrderProducts.Include(o => o.Order).Include(o => o.Order.User)
                        .Where(o => o.Order.UserId == SelectUser).Where(o => o.ProductId == SelectProductId)
                        .Where(o => o.Order.OrderDate >= FirstOrderDate).Where(o => o.Order.OrderDate != null);
                    return PartialView(await applicationDbContext.ToListAsync());
                }
                //Sadece Kullanıcı, Ürün, Bitiş Tarihi Seçiliyse
                if (SelectProductId != null && SelectUser != "undefined" && FirstOrderDate == null && LastOrderDate != null)
                {
                    var applicationDbContext = _context.OrderProducts.Include(o => o.Order).Include(o => o.Order.User)
                        .Where(o => o.Order.UserId == SelectUser).Where(o => o.ProductId == SelectProductId)
                        .Where(o => o.Order.OrderDate <= LastOrderDate).Where(o => o.Order.OrderDate != null);
                    return PartialView(await applicationDbContext.ToListAsync());
                }
                //Sadece Ürün, Başlangıç ve Bitiş Tarihi Seçiliyse
                if (SelectProductId != null && SelectUser == "undefined" && FirstOrderDate != null && LastOrderDate != null)
                {
                    var applicationDbContext = _context.OrderProducts.Include(o => o.Order).Include(o => o.Order.User)
                        .Where(o => o.Order.OrderDate >= FirstOrderDate).Where(o => o.Order.OrderDate <= LastOrderDate)
                        .Where(o => o.ProductId == SelectProductId).Where(o => o.Order.OrderDate != null);
                    return PartialView(await applicationDbContext.ToListAsync());
                }
                //Sadece Kullanıcı, Başlangıç ve Bitiş Tarihi Seçiliyse
                if (SelectProductId == null && SelectUser != "undefined" && FirstOrderDate != null && LastOrderDate != null)
                {
                    var applicationDbContext = _context.OrderProducts.Include(o => o.Order).Include(o => o.Order.User)
                        .Where(o => o.Order.UserId == SelectUser).Where(o => o.Order.OrderDate <= LastOrderDate)
                        .Where(o => o.Order.OrderDate >= FirstOrderDate).Where(o => o.Order.OrderDate != null);
                    return PartialView(await applicationDbContext.ToListAsync());
                }
                //Sadece Kullanıcı, Ürün, Başlangıç ve Bitiş Tarihi Seçiliyse
                if (SelectProductId != null && SelectUser != "undefined" && FirstOrderDate != null && LastOrderDate != null)
                {
                    var applicationDbContext = _context.OrderProducts.Include(o => o.Order).Include(o => o.Order.User)
                        .Where(o => o.Order.UserId == SelectUser).Where(o => o.Order.OrderDate <= LastOrderDate)
                        .Where(o => o.Order.OrderDate >= FirstOrderDate).Where(o => o.ProductId == SelectProductId).Where(o => o.Order.OrderDate != null);
                    return PartialView(await applicationDbContext.ToListAsync());
                }
            }
            var applicationDbContext2 = _context.OrderProducts.Include(o => o.Order).Include(o => o.Order.User).Where(o => o.Order.OrderDate != null);
            return PartialView(await applicationDbContext2.ToListAsync());
        }

        public async Task<IActionResult> SellingProducts()
        {
            return View();
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
        public void AddCart(int productId, int amount)
        {
            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Order newOrder = _context.Orders.Include(o=> o.OrderProducts).Where(o => o.UserId == userName).Where(o => o.OrderDate == null).FirstOrDefault();
            Product product = _context.Products.Where(p => p.Id == productId).FirstOrDefault();

            if (newOrder == null)
            {
                ApplicationUser applicationUser = _userManager.FindByIdAsync(userName).Result;
                newOrder = new Order();
                newOrder.TotalPrice = 0;
                newOrder.OrderDate = null;
                newOrder.Address = applicationUser.Address;
                if (applicationUser.PhoneNumber == null)
                {
                    newOrder.PhoneNumber = "0";
                }
                else
                {
                    newOrder.PhoneNumber = applicationUser.PhoneNumber;
                }
                newOrder.UserId = userName;
                newOrder.OrderProducts = new List<OrderProduct>();
                _context.Add(newOrder);
            }
            OrderProduct orderProduct = newOrder.OrderProducts.Where(o => o.ProductId == productId).FirstOrDefault();
            if (orderProduct == null)
            {
                orderProduct = new OrderProduct();
                orderProduct.ProductId = productId;
                orderProduct.OrderId = newOrder.Id;
                orderProduct.Price = product.Price;
                orderProduct.Amount = amount;
                newOrder.TotalPrice = newOrder.TotalPrice + product.Price * orderProduct.Amount;
                newOrder.OrderProducts.Add(orderProduct);
            }
            else
            {
                newOrder.TotalPrice = newOrder.TotalPrice + product.Price * amount;
                orderProduct.Amount += amount;
            }
            orderProduct.TotalPrice = product.Price * orderProduct.Amount;
            _context.SaveChanges();
            Response.Redirect("/urunlerimiz");
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

        public async Task<IActionResult> DeleteProduct(int productid, long orderid)
        {
            Order newOrder = _context.Orders.Where(o => o.Id == orderid).FirstOrDefault();
            if (productid == null || orderid==null || _context.OrderProducts == null)
            {
                return NotFound();
            }

            var orderProduct = await _context.OrderProducts
                .Include(o => o.Order)
                .Include(o => o.Product).Where(o=> o.OrderId==orderid)
                .FirstOrDefaultAsync(m => m.ProductId == productid);
            if (orderProduct == null)
            {
                return NotFound();
            }
            if (_context.OrderProducts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.OrderProducts'  is null.");
            }
            newOrder.TotalPrice = newOrder.TotalPrice - orderProduct.TotalPrice;
            newOrder.OrderProducts.Remove(orderProduct);
            _context.OrderProducts.Remove(orderProduct);
            _context.SaveChanges();
            return View();
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
            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Order newOrder = _context.Orders.Where(o => o.UserId == userName).Where(o => o.OrderDate == null).FirstOrDefault();
            OrderProduct orderProduct = _context.OrderProducts.Where(o => o.OrderId == newOrder.Id).Where(p => p.ProductId == productid).FirstOrDefault();
            orderProduct.Amount = orderProduct.Amount + 1;
            orderProduct.TotalPrice = orderProduct.Price * orderProduct.Amount;
            newOrder.TotalPrice = newOrder.TotalPrice + orderProduct.Price ;
            _context.SaveChanges();
            return orderProduct.Amount;
        }
        public int CountMinus(int productid)
        {
            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Order newOrder = _context.Orders.Where(o => o.UserId == userName).Where(o => o.OrderDate == null).FirstOrDefault();
            OrderProduct orderProduct = _context.OrderProducts.Where(o => o.OrderId == newOrder.Id).Where(p => p.ProductId == productid).FirstOrDefault();
            if (orderProduct.Amount > 1)
            {
                orderProduct.Amount = orderProduct.Amount - 1;
                orderProduct.TotalPrice = orderProduct.Price * orderProduct.Amount;
                newOrder.TotalPrice = newOrder.TotalPrice - orderProduct.Price ;
                _context.SaveChanges();
            }
            return orderProduct.Amount;
        }
    }
}
