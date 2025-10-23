using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Business.Data;
using Business.Models.Testimonial;

namespace Business.Controllers.Testimonial
{
    public class TestimonialSectionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestimonialSectionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TestimonialSections
        public async Task<IActionResult> Index()
        {
            return View(await _context.TestimonialSection.ToListAsync());
        }

        // GET: TestimonialSections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonialSection = await _context.TestimonialSection
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testimonialSection == null)
            {
                return NotFound();
            }

            return View(testimonialSection);
        }

        // GET: TestimonialSections/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TestimonialSections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SectionTitle,MainHeading")] TestimonialSection testimonialSection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testimonialSection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testimonialSection);
        }

        // GET: TestimonialSections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonialSection = await _context.TestimonialSection.FindAsync(id);
            if (testimonialSection == null)
            {
                return NotFound();
            }
            return View(testimonialSection);
        }

        // POST: TestimonialSections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SectionTitle,MainHeading")] TestimonialSection testimonialSection)
        {
            if (id != testimonialSection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testimonialSection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialSectionExists(testimonialSection.Id))
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
            return View(testimonialSection);
        }

        // GET: TestimonialSections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonialSection = await _context.TestimonialSection
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testimonialSection == null)
            {
                return NotFound();
            }

            return View(testimonialSection);
        }

        // POST: TestimonialSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testimonialSection = await _context.TestimonialSection.FindAsync(id);
            if (testimonialSection != null)
            {
                _context.TestimonialSection.Remove(testimonialSection);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestimonialSectionExists(int id)
        {
            return _context.TestimonialSection.Any(e => e.Id == id);
        }
    }
}
