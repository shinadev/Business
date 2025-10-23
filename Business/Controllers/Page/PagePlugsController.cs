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
    public class PagePlugsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagePlugsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PagePlugs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PagePlugs.Include(p => p.Page);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PagePlugs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var pagePlug = await _context.PagePlugs
                .Include(p => p.Page)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pagePlug == null) return NotFound();

            return View(pagePlug);
        }

        // GET: PagePlugs/Create
        public IActionResult Create()
        {
            ViewData["DynamicPageId"] = new SelectList(_context.DynamicPages, "Id", "Slug");
            return View();
        }

        // POST: PagePlugs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Slug,Description,Content,DynamicPageId")] PagePlug pagePlug)
        {
            // If Slug is empty from form, generate it BEFORE ModelState validation
            if (string.IsNullOrWhiteSpace(pagePlug.Slug) && !string.IsNullOrWhiteSpace(pagePlug.Title))
            {
                pagePlug.Slug = GenerateSlug(pagePlug.Title);

                // Optionally clear Slug validation errors from ModelState so it won't complain
                ModelState.Remove("Slug");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    pagePlug.CreatedAt = DateTime.Now;
                    pagePlug.UpdatedAt = DateTime.Now;

                    _context.Add(pagePlug);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error saving data: {ex.Message}");
                }
            }

            ViewData["DynamicPageId"] = new SelectList(_context.DynamicPages, "Id", "Slug", pagePlug.DynamicPageId);
            return View(pagePlug);
        }

        // GET: PagePlugs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var pagePlug = await _context.PagePlugs.FindAsync(id);
            if (pagePlug == null) return NotFound();

            ViewData["DynamicPageId"] = new SelectList(_context.DynamicPages, "Id", "Slug", pagePlug.DynamicPageId);
            return View(pagePlug);
        }

        // POST: PagePlugs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Slug,Description,Content,CreatedAt,DynamicPageId")] PagePlug pagePlug)
        {
            if (id != pagePlug.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Update date
                    pagePlug.UpdatedAt = DateTime.Now;

                    // Auto-generate slug if empty
                    if (string.IsNullOrWhiteSpace(pagePlug.Slug))
                    {
                        pagePlug.Slug = GenerateSlug(pagePlug.Title);
                    }

                    _context.Update(pagePlug);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagePlugExists(pagePlug.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["DynamicPageId"] = new SelectList(_context.DynamicPages, "Id", "Slug", pagePlug.DynamicPageId);
            return View(pagePlug);
        }

        // GET: PagePlugs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var pagePlug = await _context.PagePlugs
                .Include(p => p.Page)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pagePlug == null) return NotFound();

            return View(pagePlug);
        }

        // POST: PagePlugs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pagePlug = await _context.PagePlugs.FindAsync(id);
            if (pagePlug != null)
            {
                _context.PagePlugs.Remove(pagePlug);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PagePlugExists(int id)
        {
            return _context.PagePlugs.Any(e => e.Id == id);
        }

        // Helper method to generate slug from title
        private string GenerateSlug(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return "";

            return title.ToLower()
                        .Replace(" ", "-")
                        .Replace("--", "-")
                        .Replace(",", "")
                        .Replace(".", "")
                        .Replace(":", "")
                        .Replace(";", "")
                        .Trim();
        }
    }
}
