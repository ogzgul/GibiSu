using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GibiSu.Data;
using GibiSu.Models;
using Microsoft.Extensions.Hosting;

namespace GibiSu.Controllers
{
    public class PagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Pages.Include(p => p.Menu);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Pages/Details/5
        public async Task<IActionResult> Details(string id)
        {
            Page page = context.Pages.Where(p=>p.Url == id).Include(p => p.Contents.OrderBy(c=>c.Order)).FirstOrDefault();
            if (id == null || _context.Pages == null)
            {
                return NotFound();
            }

            page = await _context.Pages
               .Include(p => p.Menu)
               .FirstOrDefaultAsync(m => m.Url == id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // GET: Pages/Create
        public IActionResult Create()
        {
            ViewData["MenuId"] = new SelectList(_context.Menus, "Id", "Name");
            return View();
        }

        // POST: Pages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Url,FormImage,MenuId")] Page page)
        {
            ModelState.Remove("Banner");
            ModelState.Remove("Contents");

            MemoryStream memoryStream;

            if (ModelState.IsValid)
            {
                if (page.FormImage != null)
                {
                    memoryStream = new MemoryStream();
                    page.FormImage.CopyTo(memoryStream);
                    page.Banner = memoryStream.ToArray();
                }

                _context.Add(page);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuId"] = new SelectList(_context.Menus, "Id", "Name", page.MenuId);
            return View(page);
        }

        // GET: Pages/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Pages == null)
            {
                return NotFound();
            }

            var page = await _context.Pages.FindAsync(id);
            if (page == null)
            {
                return NotFound();
            }
            ViewData["MenuId"] = new SelectList(_context.Menus, "Id", "Name", page.MenuId);
            return View(page);
        }

        // POST: Pages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Url,Banner,MenuId")] Page page)
        {
            if (id != page.Url)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(page);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PageExists(page.Url))
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
            ViewData["MenuId"] = new SelectList(_context.Menus, "Id", "Name", page.MenuId);
            return View(page);
        }

        // GET: Pages/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Pages == null)
            {
                return NotFound();
            }

            var page = await _context.Pages
                .Include(p => p.Menu)
                .FirstOrDefaultAsync(m => m.Url == id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // POST: Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Pages == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Pages'  is null.");
            }
            var page = await _context.Pages.FindAsync(id);
            if (page != null)
            {
                _context.Pages.Remove(page);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PageExists(string id)
        {
            return _context.Pages.Any(e => e.Url == id);
        }
    }
}