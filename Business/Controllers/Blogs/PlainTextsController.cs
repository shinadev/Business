using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Business.Data;
using Business.Models.Blog;

namespace Business.Controllers.Blogs
{
    public class PlainTextsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlainTextsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PlainTexts
        public async Task<IActionResult> Index()
        {
            var plainTexts = _context.PlainTexts.Include(p => p.BlogDetail);
            return View(await plainTexts.ToListAsync());
        }

        // GET: PlainTexts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var plainText = await _context.PlainTexts
                .Include(p => p.BlogDetail)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (plainText == null) return NotFound();

            return View(plainText);
        }

        // GET: PlainTexts/Create
        public IActionResult Create()
        {
            ViewData["BlogDetailId"] = new SelectList(_context.BlogDetails, "Id", "Title");
            return View();
        }

        // POST: PlainTexts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,ReadMoreLink,BlogDetailId")] PlainText plainText)
        {
            if (ModelState.IsValid)
            {
                if (_context.PlainTexts.Any(p => p.BlogDetailId == plainText.BlogDetailId))
                {
                    ModelState.AddModelError("BlogDetailId", "This blog already has a PlainText.");
                    ViewData["BlogDetailId"] = new SelectList(_context.BlogDetails, "Id", "Title", plainText.BlogDetailId);
                    return View(plainText);
                }

                _context.Add(plainText);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogDetailId"] = new SelectList(_context.BlogDetails, "Id", "Title", plainText.BlogDetailId);
            return View(plainText);
        }

        // GET: PlainTexts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var plainText = await _context.PlainTexts.FindAsync(id);
            if (plainText == null) return NotFound();

            ViewData["BlogDetailId"] = new SelectList(_context.BlogDetails, "Id", "Title", plainText.BlogDetailId);
            return View(plainText);
        }

        // POST: PlainTexts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,ReadMoreLink,BlogDetailId")] PlainText plainText)
        {
            if (id != plainText.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plainText);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlainTextExists(plainText.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["BlogDetailId"] = new SelectList(_context.BlogDetails, "Id", "Title", plainText.BlogDetailId);
            return View(plainText);
        }

        // GET: PlainTexts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var plainText = await _context.PlainTexts
                .Include(p => p.BlogDetail)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (plainText == null) return NotFound();

            return View(plainText);
        }

        // POST: PlainTexts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plainText = await _context.PlainTexts.FindAsync(id);
            if (plainText != null)
            {
                _context.PlainTexts.Remove(plainText);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PlainTextExists(int id)
        {
            return _context.PlainTexts.Any(e => e.Id == id);
        }
    }
}
