using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GibiSu.Data;
using GibiSu.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Imaging;

namespace GibiSu.Controllers
{
    public class ContentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        System.Drawing.Imaging.EncoderParameter qualityParameter;
        System.Drawing.Imaging.ImageCodecInfo[] allCoDecs;
        System.Drawing.Imaging.EncoderParameters encoderParameters;
        System.Drawing.Imaging.ImageCodecInfo jPEGCodec;
        System.Drawing.Image streamImage;
        System.Drawing.Image bLogImage;
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
        //[Authorize(Roles = "Administrator")]
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
            qualityParameter = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 60L);
            allCoDecs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            encoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
            foreach (System.Drawing.Imaging.ImageCodecInfo coDec in allCoDecs)
            {
                if (coDec.FormatDescription == "JPEG")
                {
                    jPEGCodec = coDec;
                }
            }

            ModelState.Remove("Image");

            MemoryStream memoryStream = new MemoryStream();
            encoderParameters.Param[0] = qualityParameter;
            if (content.FormImage != null)
            {
                content.FormImage.CopyTo(memoryStream);
                streamImage = System.Drawing.Image.FromStream(memoryStream);
                bLogImage = ReSize(streamImage, 435, 595);
                bLogImage.Save(memoryStream, jPEGCodec, encoderParameters);
                if (ModelState.IsValid)
                {
                    content.Image = memoryStream.ToArray();
                    _context.Add(content);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["PageUrl"] = new SelectList(_context.Pages, "Url", "Url", content.PageUrl);
            return View(content);
        }

        private System.Drawing.Image ReSize(System.Drawing.Image originalImage, int newWidth, int newHeight)
        {
            System.Drawing.Graphics graphicsHandle;
            double targetRatio = (double)newWidth / (double)newHeight;
            double newRatio = (double)originalImage.Width / (double)originalImage.Height;
            int targetWidth = newWidth;
            int targetHeight = newHeight;
            int newOriginX = 0;
            int newOriginY = 0;
            System.Drawing.Image newImage = new System.Drawing.Bitmap(newWidth, newHeight);

            if (newRatio > targetRatio)
            {
                targetWidth = (int)(originalImage.Width / ((double)originalImage.Height / newHeight));
                newOriginX = (newWidth - targetWidth) / 2;
            }
            else
            {
                if (newRatio < targetRatio)
                {
                    targetHeight = (int)(originalImage.Height / ((double)originalImage.Width / newWidth));
                    newOriginY = (newHeight - targetHeight) / 2;
                }
            }
            graphicsHandle = System.Drawing.Graphics.FromImage(newImage);
            graphicsHandle.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphicsHandle.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphicsHandle.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphicsHandle.DrawImage(originalImage, newOriginX, newOriginY, targetWidth, targetHeight);
            return newImage;
        }

        // GET: Contents/Edit/5
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
