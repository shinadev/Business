using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Business.Data;
using Business.Models.Blog;

namespace Business.Controllers.Blogs
{
    public class BlogHeadersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogHeadersController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.BlogHeaders.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var blogHeader = await _context.BlogHeaders.FirstOrDefaultAsync(m => m.Id == id);
            if (blogHeader == null) return NotFound();

            return View(blogHeader);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogHeader blogHeader, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/BlogHeaders");
                    Directory.CreateDirectory(uploadsFolder);
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }

                    blogHeader.ImageUrl = "/uploads/BlogHeaders/" + uniqueFileName;
                }

                _context.Add(blogHeader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blogHeader);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var blogHeader = await _context.BlogHeaders.FindAsync(id);
            if (blogHeader == null) return NotFound();

            return View(blogHeader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogHeader blogHeader, IFormFile ImageFile)
        {
            if (id != blogHeader.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/BlogHeaders");
                        Directory.CreateDirectory(uploadsFolder);
                        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(fileStream);
                        }

                        if (!string.IsNullOrEmpty(blogHeader.ImageUrl))
                        {
                            var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, blogHeader.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldPath))
                                System.IO.File.Delete(oldPath);
                        }

                        blogHeader.ImageUrl = "/uploads/BlogHeaders/" + uniqueFileName;
                    }

                    _context.Update(blogHeader);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogHeaderExists(blogHeader.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blogHeader);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var blogHeader = await _context.BlogHeaders.FirstOrDefaultAsync(m => m.Id == id);
            if (blogHeader == null) return NotFound();

            return View(blogHeader);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogHeader = await _context.BlogHeaders.FindAsync(id);
            if (blogHeader != null)
            {
                if (!string.IsNullOrEmpty(blogHeader.ImageUrl))
                {
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, blogHeader.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }

                _context.BlogHeaders.Remove(blogHeader);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BlogHeaderExists(int id)
        {
            return _context.BlogHeaders.Any(e => e.Id == id);
        }
    }
}
