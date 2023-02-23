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
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection.Metadata;

namespace GibiSu.Controllers
{
    public class PagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        System.Drawing.Imaging.EncoderParameter qualityParameter;
        System.Drawing.Imaging.ImageCodecInfo[] allCoDecs;
        System.Drawing.Imaging.EncoderParameters encoderParameters;
        System.Drawing.Imaging.ImageCodecInfo jPEGCodec;
        System.Drawing.Image streamImage;
        System.Drawing.Image bLogImage;
        public PagesController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Pages
        public void kuki(string a)
        {
            if (a=="kabul")
            {
                var cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddDays(1);
                cookieOptions.Path = "/";
                Response.Cookies.Append("isim1", "isim2", cookieOptions);
            }
            else if (a=="red")
            {
                var cookieOptions = new CookieOptions();
                cookieOptions.Path = "/#";
                Response.Cookies.Delete("isim1");
                
            }
        }

    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.Pages.Include(p => p.Menu);
        Page page = _context.Pages.Include(p => p.Contents.OrderBy(c => c.Order)).Where(d => d.Url == "Index").FirstOrDefault();
        return View(page);
    }
       

        public async Task<IActionResult> Sayfalar()
    {
        var applicationDbContext = _context.Pages.Include(p => p.Menu);
        return View(await applicationDbContext.ToListAsync());
    }
        public async Task<PartialViewResult> NewContent(int i)
        {
            ViewData["contentCount"] = i;
            return PartialView();
        }

        public async Task<PartialViewResult> Preview([Bind("Url,FormImage,MenuId,Title,Contents")] Page page )
        {
            ViewData["pageName"] = Url;
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
            ModelState.Remove("Banner");
            ModelState.Remove("Contents");

            //page.Contents[0].PageUrl=page.Url;
            MemoryStream memoryStream = new MemoryStream();
            encoderParameters.Param[0] = qualityParameter;
            if (page.FormImage != null)
            {
                page.FormImage.CopyTo(memoryStream);
                streamImage = System.Drawing.Image.FromStream(memoryStream);
                bLogImage = ReSize(streamImage, 1175, 540);
                bLogImage.Save(memoryStream, jPEGCodec, encoderParameters);
                if (ModelState.IsValid)
                {
                    page.Banner = memoryStream.ToArray();
                    foreach (Content content in page.Contents)
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
                        MemoryStream memoryStream2 = new MemoryStream();
                        encoderParameters.Param[0] = qualityParameter;
                        if (content.FormImage != null)
                        {
                            content.FormImage.CopyTo(memoryStream2);
                            streamImage = System.Drawing.Image.FromStream(memoryStream2);
                            bLogImage = ReSize(streamImage, 435, 595);
                            bLogImage.Save(memoryStream2, jPEGCodec, encoderParameters);
                            //content.PageUrl = page.Url;
                            if (ModelState.IsValid)
                            {
                                content.Image = memoryStream2.ToArray();
                                //page.Contents.Add(content);
                            }
                        }

                        ViewData["PageUrl"] = new SelectList(_context.Pages, "Url", "Url", content.PageUrl);
                    }
                }
            }
            return PartialView(page);
        }

        // GET: Pages/Details/5
        public async Task<IActionResult> Details(string id)
    {
        ViewData["pageName"] = id;
        Page page = _context.Pages.Where(p => p.Url == id).Include(p => p.Contents.OrderBy(c => c.Order)).FirstOrDefault();
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
    //[Authorize(Roles = "Administrator")]
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
    public async Task<IActionResult> Create([Bind("Url,FormImage,MenuId,Title,Contents")] Page page)
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
            ModelState.Remove("Banner");
        ModelState.Remove("Contents");

        MemoryStream memoryStream = new MemoryStream();
            encoderParameters.Param[0] = qualityParameter;
            if (page.FormImage != null)
            {
                page.FormImage.CopyTo(memoryStream);
                streamImage = System.Drawing.Image.FromStream(memoryStream);
                bLogImage = ReSize(streamImage, 1175, 540);
                bLogImage.Save(memoryStream, jPEGCodec, encoderParameters);
                if (ModelState.IsValid)
                {
                    page.Banner = memoryStream.ToArray();
                    if (page.Contents != null) {
                        foreach (Content content in page.Contents)
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
                            MemoryStream memoryStream2 = new MemoryStream();
                            encoderParameters.Param[0] = qualityParameter;
                            if (content.FormImage != null)
                            {
                                content.FormImage.CopyTo(memoryStream2);
                                streamImage = System.Drawing.Image.FromStream(memoryStream2);
                                bLogImage = ReSize(streamImage, 435, 595);
                                bLogImage.Save(memoryStream2, jPEGCodec, encoderParameters);
                                //content.PageUrl = page.Url;
                                if (ModelState.IsValid)
                                {
                                    content.Image = memoryStream2.ToArray();
                                    //page.Contents.Add(content);
                                }
                            }

                            ViewData["PageUrl"] = new SelectList(_context.Pages, "Url", "Url", content.PageUrl);
                        }
                    }
                    _context.Add(page);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
        ViewData["MenuId"] = new SelectList(_context.Menus, "Id", "Name", page.MenuId);
        return View(page);
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

        // GET: Pages/Edit/5
        [Authorize(Roles = "Administrator")]
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
    [Authorize(Roles = "Administrator")]
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