using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Business.Data;
using Business.Models.Page;

namespace Business.Controllers.Page
{
    public class PageLayoutSectionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PageLayoutSectionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PageLayoutSections
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PageLayoutSections
                .Include(p => p.LayoutSection)
                .Include(p => p.Page);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PageLayoutSections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var pageLayoutSection = await _context.PageLayoutSections
                .Include(p => p.LayoutSection)
                .Include(p => p.Page)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pageLayoutSection == null) return NotFound();

            return View(pageLayoutSection);
        }

        // GET: PageLayoutSections/Create
        public IActionResult Create()
        {
            ViewData["LayoutSectionId"] = new SelectList(_context.LayoutSections, "Id", "Name");
            ViewData["DynamicPageId"] = new SelectList(_context.DynamicPages, "Id", "Slug");
            return View();
        }

        // POST: PageLayoutSections/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DynamicPageId,LayoutSectionId,Title,Content,SortOrder,IsActive")] PageLayoutSection pageLayoutSection)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                ViewBag.ModelErrors = errors;

                ViewData["LayoutSectionId"] = new SelectList(_context.LayoutSections, "Id", "Name", pageLayoutSection.LayoutSectionId);
                ViewData["DynamicPageId"] = new SelectList(_context.DynamicPages, "Id", "Slug", pageLayoutSection.DynamicPageId);

                return View(pageLayoutSection);
            }

            // Set dates here, not in the form
            pageLayoutSection.CreatedAt = DateTime.UtcNow;
            pageLayoutSection.UpdatedAt = DateTime.UtcNow;

            _context.Add(pageLayoutSection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: PageLayoutSections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var pageLayoutSection = await _context.PageLayoutSections.FindAsync(id);
            if (pageLayoutSection == null) return NotFound();

            ViewData["LayoutSectionId"] = new SelectList(_context.LayoutSections, "Id", "Name", pageLayoutSection.LayoutSectionId);
            ViewData["DynamicPageId"] = new SelectList(_context.DynamicPages, "Id", "Slug", pageLayoutSection.DynamicPageId);
            return View(pageLayoutSection);
        }

        // POST: PageLayoutSections/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DynamicPageId,LayoutSectionId,Title,Content,SortOrder,IsActive,CreatedAt")] PageLayoutSection pageLayoutSection)
        {
            if (id != pageLayoutSection.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                ViewBag.ModelErrors = errors;

                ViewData["LayoutSectionId"] = new SelectList(_context.LayoutSections, "Id", "Name", pageLayoutSection.LayoutSectionId);
                ViewData["DynamicPageId"] = new SelectList(_context.DynamicPages, "Id", "Slug", pageLayoutSection.DynamicPageId);

                return View(pageLayoutSection);
            }

            try
            {
                pageLayoutSection.UpdatedAt = DateTime.UtcNow;
                _context.Update(pageLayoutSection);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PageLayoutSectionExists(pageLayoutSection.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: PageLayoutSections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var pageLayoutSection = await _context.PageLayoutSections
                .Include(p => p.LayoutSection)
                .Include(p => p.Page)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pageLayoutSection == null) return NotFound();

            return View(pageLayoutSection);
        }

        // POST: PageLayoutSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pageLayoutSection = await _context.PageLayoutSections.FindAsync(id);
            if (pageLayoutSection != null)
            {
                _context.PageLayoutSections.Remove(pageLayoutSection);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PageLayoutSectionExists(int id)
        {
            return _context.PageLayoutSections.Any(e => e.Id == id);
        }
    }
}
