using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Business.Data;
using Business.Models.Page;

namespace Business.Controllers.Page
{
    public class PageCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PageCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PageCategories
        public async Task<IActionResult> Index()
        {
            var categories = await _context.PageCategory.ToListAsync();
            return View(categories);
        }

        // GET: PageCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var pageCategory = await _context.PageCategory.FirstOrDefaultAsync(m => m.Id == id);
            if (pageCategory == null) return NotFound();

            return View(pageCategory);
        }

        // GET: PageCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PageCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] PageCategory pageCategory)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                ViewBag.ModelErrors = errors;
                return View(pageCategory);
            }

            _context.Add(pageCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: PageCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var pageCategory = await _context.PageCategory.FindAsync(id);
            if (pageCategory == null) return NotFound();

            return View(pageCategory);
        }

        // POST: PageCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] PageCategory pageCategory)
        {
            if (id != pageCategory.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(pageCategory);
            }

            try
            {
                _context.Update(pageCategory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.PageCategory.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: PageCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var pageCategory = await _context.PageCategory.FirstOrDefaultAsync(m => m.Id == id);
            if (pageCategory == null) return NotFound();

            return View(pageCategory);
        }

        // POST: PageCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pageCategory = await _context.PageCategory.FindAsync(id);
            if (pageCategory != null)
            {
                _context.PageCategory.Remove(pageCategory);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
