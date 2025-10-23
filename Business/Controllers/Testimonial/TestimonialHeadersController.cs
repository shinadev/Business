using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Business.Data;
using Business.Models.Testimonial;
using Microsoft.AspNetCore.Http;

namespace Business.Controllers.Testimonial
{
    public class TestimonialHeadersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestimonialHeadersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TestimonialHeaders
        public async Task<IActionResult> Index()
        {
            return View(await _context.TestimonialHeader.ToListAsync());
        }

        // GET: TestimonialHeaders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var testimonialHeader = await _context.TestimonialHeader
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testimonialHeader == null) return NotFound();

            return View(testimonialHeader);
        }

        // GET: TestimonialHeaders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TestimonialHeaders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TestimonialHeader testimonialHeader, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    testimonialHeader.ImageUrl = await SaveImageAsync(ImageFile);
                }

                _context.Add(testimonialHeader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testimonialHeader);
        }

        // GET: TestimonialHeaders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var testimonialHeader = await _context.TestimonialHeader.FindAsync(id);
            if (testimonialHeader == null) return NotFound();

            return View(testimonialHeader);
        }

        // POST: TestimonialHeaders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TestimonialHeader testimonialHeader, IFormFile ImageFile)
        {
            if (id != testimonialHeader.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.TestimonialHeader.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
                    if (existing == null) return NotFound();

                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        DeleteImageIfExists(existing.ImageUrl);
                        testimonialHeader.ImageUrl = await SaveImageAsync(ImageFile);
                    }
                    else
                    {
                        testimonialHeader.ImageUrl = existing.ImageUrl; // keep old image if no new upload
                    }

                    _context.Update(testimonialHeader);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialHeaderExists(testimonialHeader.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(testimonialHeader);
        }

        // GET: TestimonialHeaders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var testimonialHeader = await _context.TestimonialHeader
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testimonialHeader == null) return NotFound();

            return View(testimonialHeader);
        }

        // POST: TestimonialHeaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testimonialHeader = await _context.TestimonialHeader.FindAsync(id);
            if (testimonialHeader != null)
            {
                DeleteImageIfExists(testimonialHeader.ImageUrl);
                _context.TestimonialHeader.Remove(testimonialHeader);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestimonialHeaderExists(int id)
        {
            return _context.TestimonialHeader.Any(e => e.Id == id);
        }

        // --- Image helpers ---

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "Htestimonial");
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            string filePath = Path.Combine(uploadDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/uploads/Htestimonial/{fileName}";
        }

        private void DeleteImageIfExists(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }
    }
}
