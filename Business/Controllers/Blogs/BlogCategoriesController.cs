using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Business.Data;
using Business.Models.Blog;

namespace Business.Controllers.Blogs
{
    public class BlogCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlogCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BlogCategories
        public async Task<IActionResult> Index()
        {
            var categories = _context.BlogCategories.Include(b => b.BlogDetail);
            return View(await categories.ToListAsync());
        }

        // GET: BlogCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.BlogCategories
                .Include(b => b.BlogDetail)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (category == null) return NotFound();
            return View(category);
        }

        // GET: BlogCategories/Create
        public IActionResult Create()
        {
            // Ensure the dropdown has Blog titles
            var blogs = _context.BlogDetails
                                .Select(b => new { b.Id, b.Title })
                                .ToList();
            ViewData["BlogDetailId"] = new SelectList(blogs, "Id", "Title");
            return View();
        }

        // POST: BlogCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Link,BlogDetailId")] BlogCategory blogCategory)
        {
            // DEBUG: log ModelState
            Console.WriteLine("🛠 DEBUG: Create POST hit");

            if (blogCategory.BlogDetailId == 0)
            {
                ModelState.AddModelError("BlogDetailId", "Please select a blog.");
                Console.WriteLine("❌ BlogDetailId not selected");
            }

            if (!ModelState.IsValid)
            {
                // Log all ModelState errors
                foreach (var error in ModelState)
                {
                    foreach (var err in error.Value.Errors)
                    {
                        Console.WriteLine($"Field: {error.Key} | Error: {err.ErrorMessage}");
                    }
                }

                // Re-populate dropdown
                var blogs = _context.BlogDetails
                                    .Select(b => new { b.Id, b.Title })
                                    .ToList();
                ViewData["BlogDetailId"] = new SelectList(blogs, "Id", "Title", blogCategory.BlogDetailId);
                return View(blogCategory);
            }

            // Everything valid → save
            _context.Add(blogCategory);
            await _context.SaveChangesAsync();
            Console.WriteLine("✅ BlogCategory created successfully");
            return RedirectToAction(nameof(Index));
        }

        // GET: BlogCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.BlogCategories.FindAsync(id);
            if (category == null) return NotFound();

            var blogs = _context.BlogDetails.Select(b => new { b.Id, b.Title }).ToList();
            ViewData["BlogDetailId"] = new SelectList(blogs, "Id", "Title", category.BlogDetailId);

            return View(category);
        }

        // POST: BlogCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Link,BlogDetailId")] BlogCategory blogCategory)
        {
            if (id != blogCategory.Id) return NotFound();

            if (blogCategory.BlogDetailId == 0)
                ModelState.AddModelError("BlogDetailId", "Please select a blog.");

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    foreach (var err in error.Value.Errors)
                    {
                        Console.WriteLine($"Field: {error.Key} | Error: {err.ErrorMessage}");
                    }
                }

                var blogs = _context.BlogDetails.Select(b => new { b.Id, b.Title }).ToList();
                ViewData["BlogDetailId"] = new SelectList(blogs, "Id", "Title", blogCategory.BlogDetailId);
                return View(blogCategory);
            }

            try
            {
                _context.Update(blogCategory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.BlogCategories.Any(e => e.Id == id)) return NotFound();
                else throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: BlogCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.BlogCategories
                .Include(b => b.BlogDetail)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (category == null) return NotFound();
            return View(category);
        }

        // POST: BlogCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.BlogCategories.FindAsync(id);
            if (category != null)
            {
                _context.BlogCategories.Remove(category);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
