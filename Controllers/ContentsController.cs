using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GibiSu.Data;
using GibiSu.Models;

namespace GibiSu.Controllers
{
    public class ContentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contents
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Contents.Include(c => c.Page);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Contents/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Contents == null)
            {
                return NotFound();
            }

            var content = await _context.Contents
                .Include(c => c.Page)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        // GET: Contents/Create
        public IActionResult Create()
        {
            ViewData["PageUrl"] = new SelectList(_context.Pages, "Url", "Url");
            return View();
        }

        // POST: Contents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Text,FormImage,Order,PageUrl,Type")] Content content)
        {
            MemoryStream memoryStream;
            ModelState.Remove("Image");
            if (ModelState.IsValid)
            {
                if(content.FormImage != null)
                {
                    memoryStream = new MemoryStream();
                    content.FormImage.CopyTo(memoryStream);
                    content.Image = memoryStream.ToArray();
                }
                _context.Add(content);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PageUrl"] = new SelectList(_context.Pages, "Url", "Url", content.PageUrl);
            return View(content);
        }

        // GET: Contents/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Contents == null)
            {
                return NotFound();
            }

            var content = await _context.Contents.FindAsync(id);
            if (content == null)
            {
                return NotFound();
            }
            ViewData["PageUrl"] = new SelectList(_context.Pages, "Url", "Url", content.PageUrl);
            return View(content);
        }

        // POST: Contents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Title,Text,Image,Order,PageUrl,Type")] Content content)
        {
            if (id != content.Id)
            {
                return NotFound();
            }
            MemoryStream memoryStream;
            ModelState.Remove("Image");

            if (ModelState.IsValid)
            {
                if (content.FormImage != null)
                {
                    memoryStream = new MemoryStream();
                    content.FormImage.CopyTo(memoryStream);
                    content.Image = memoryStream.ToArray();
                }
                try
                {
                    _context.Update(content);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentExists(content.Id))
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
            ViewData["PageUrl"] = new SelectList(_context.Pages, "Url", "Url", content.PageUrl);
            return View(content);
        }

        // GET: Contents/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Contents == null)
            {
                return NotFound();
            }

            var content = await _context.Contents
                .Include(c => c.Page)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        // POST: Contents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Contents == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Contents'  is null.");
            }
            var content = await _context.Contents.FindAsync(id);
            if (content != null)
            {
                _context.Contents.Remove(content);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContentExists(long id)
        {
          return _context.Contents.Any(e => e.Id == id);
        }
    }
}
