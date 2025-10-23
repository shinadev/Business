using Business.Data;
using Business.Models.Testimonial;
using Business.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Controllers
{
    public class TestimonialsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TestimonialsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ================= Index =================
        public async Task<IActionResult> Index()
        {
            var section = await _context.TestimonialSection
                .Include(s => s.Testimonials)
                .FirstOrDefaultAsync();

            var model = new TestimonialIndexViewModel
            {
                Section = section,
                Testimonials = section?.Testimonials ?? new List<Testimonials>()
            };

            return View(model);
        }

        // ================= Details =================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var testimonial = await _context.Testimonial
                .Include(t => t.TestimonialSection)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (testimonial == null) return NotFound();
            return View(testimonial);
        }

        // ================= Create =================
        public IActionResult Create()
        {
            ViewBag.Sections = new SelectList(_context.TestimonialSection, "Id", "SectionTitle");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Testimonials model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageFile != null)
                {
                    string uploads = Path.Combine(_env.WebRootPath, "uploads/testimonials");
                    Directory.CreateDirectory(uploads);

                    string fileName = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);
                    string filePath = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    model.ImageUrl = "/uploads/testimonials/" + fileName; // notice the slash
                }

                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Re-populate dropdown if ModelState is invalid
            ViewBag.Sections = _context.TestimonialSection.ToList();
            return View(model);
        }


        // ================= Edit =================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var testimonial = await _context.Testimonial.FindAsync(id);
            if (testimonial == null) return NotFound();

            ViewBag.Sections = new SelectList(_context.TestimonialSection, "Id", "SectionTitle", testimonial.TestimonialSectionId);
            return View(testimonial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Testimonials model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.ImageFile != null)
                        model.ImageUrl = await UploadImage(model.ImageFile);

                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(model.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Sections = new SelectList(_context.TestimonialSection, "Id", "SectionTitle", model.TestimonialSectionId);
            return View(model);
        }

        // ================= Delete =================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var testimonial = await _context.Testimonial
                .Include(t => t.TestimonialSection)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (testimonial == null) return NotFound();
            return View(testimonial);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testimonial = await _context.Testimonial.FindAsync(id);
            if (testimonial != null)
            {
                DeleteImageFile(testimonial.ImageUrl);

                _context.Testimonial.Remove(testimonial);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // ================= Helpers =================
        private bool TestimonialExists(int id) =>
            _context.Testimonial.Any(e => e.Id == id);

        private async Task<string> UploadImage(Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            string uploads = Path.Combine(_env.WebRootPath, "uploads/testimonials");
            Directory.CreateDirectory(uploads);

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploads, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return "/uploads/testimonials/" + fileName;
        }

        private void DeleteImageFile(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            var filePath = Path.Combine(_env.WebRootPath, imageUrl.TrimStart('/').Replace("/", "\\"));
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }
    }
}
