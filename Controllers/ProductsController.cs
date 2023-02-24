using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GibiSu.Data;
using GibiSu.Models;
using System.IO;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace GibiSu.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        System.Drawing.Imaging.EncoderParameter qualityParameter;
        System.Drawing.Imaging.ImageCodecInfo[] allCoDecs;
        System.Drawing.Imaging.EncoderParameters encoderParameters;
        System.Drawing.Imaging.ImageCodecInfo jPEGCodec;
        System.Drawing.Image streamImage;
        System.Drawing.Image bLogImage;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("tr-TR");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("tr-TR");

        }

        // GET: Products
        public async Task<IActionResult> Index(string search)
        {
            ViewData["Search"] = search;
            var urun = from b in _context.Products
                       select b;
            if (!String.IsNullOrEmpty(search))
            {
                urun = urun.Where(x => x.Name.Contains(search) || x.Volume.ToString().Contains(search) || x.Price.ToString().Contains(search));
            }
            return View(urun);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> ProductsEdit()
        {

            return View(await _context.Products.ToListAsync());
        }
        // GET: Products/Create
        //[Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,IsInactive,Volume,Material,FormImage,SmallFormImage")] Product product)
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
            MemoryStream memoryStream =  new MemoryStream(); 
            MemoryStream memoryStream2 =  new MemoryStream();

            encoderParameters.Param[0] = qualityParameter;
            if (product.FormImage != null && product.SmallFormImage != null)
            {
                product.FormImage.CopyTo(memoryStream);
                product.SmallFormImage.CopyTo(memoryStream2);
                streamImage = System.Drawing.Image.FromStream(memoryStream);
                bLogImage = ReSize(streamImage, 300, 250);
                bLogImage.Save(memoryStream, jPEGCodec, encoderParameters);
                streamImage = System.Drawing.Image.FromStream(memoryStream2);
                bLogImage = ReSize(streamImage, 80, 80);
                bLogImage.Save(memoryStream2, jPEGCodec, encoderParameters);
                ModelState.Remove("Image");
                ModelState.Remove("SmallImage");
                if (ModelState.IsValid)
                {
                    product.Image = memoryStream.ToArray();
                    product.SmallImage = memoryStream2.ToArray();
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,FormImage,IsInactive,Volume,Material")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
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

        public async Task<IActionResult> AdminUrunler(string search)
        {
            ViewData["Search"] = search;
            var urun = from b in _context.Products
                       select b;
            if (!String.IsNullOrEmpty(search))
            {
                urun = urun.Where(x => x.Name.Contains(search) || x.Volume.ToString().Contains(search) || x.Price.ToString().Contains(search));
            }
            return View(urun);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> DetailsProductAdmin(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
