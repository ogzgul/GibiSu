using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GibiSu.Data;
using GibiSu.Models;

namespace GibiSu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Pages1Controller : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public Pages1Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Pages1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Page>>> GetPages()
        {
            return await _context.Pages.ToListAsync();
        }

        // GET: api/Pages1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Page>> GetPage(string id)
        {
            var page = await _context.Pages.FindAsync(id);

            if (page == null)
            {
                return NotFound();
            }

            return page;
        }

        // PUT: api/Pages1/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPage(string id, Page page)
        {
            if (id != page.Url)
            {
                return BadRequest();
            }

            _context.Entry(page).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Pages1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Page>> PostPage(Page page)
        {
            _context.Pages.Add(page);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PageExists(page.Url))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPage", new { id = page.Url }, page);
        }

        // DELETE: api/Pages1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePage(string id)
        {
            var page = await _context.Pages.FindAsync(id);
            if (page == null)
            {
                return NotFound();
            }

            _context.Pages.Remove(page);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PageExists(string id)
        {
            return _context.Pages.Any(e => e.Url == id);
        }
    }
}
