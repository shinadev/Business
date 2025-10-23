using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Business.Data;
using Business.Models.Page;

namespace Business.Controllers.Page
{
    public class DynamicPagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DynamicPagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DynamicPages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DynamicPages.Include(d => d.PageCategory).Include(d => d.Status);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DynamicPages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dynamicPage = await _context.DynamicPages
                .Include(d => d.PageCategory)
                .Include(d => d.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dynamicPage == null)
            {
                return NotFound();
            }

            return View(dynamicPage);
        }

        // GET: DynamicPages/Create
        public IActionResult Create()
        {
            ViewData["PageCategoryId"] = new SelectList(_context.PageCategory, "Id", "Name");
            ViewData["PageStatusId"] = new SelectList(_context.PageStatuses, "Id", "Name");
            return View();
        }

        // POST: DynamicPages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Slug,Content,MetaDescription,MetaKeywords,PageStatusId,IsPublished,CreatedAt,UpdatedAt,PageCategoryId")] DynamicPage dynamicPage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dynamicPage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PageCategoryId"] = new SelectList(_context.PageCategory, "Id", "Name", dynamicPage.PageCategoryId);
            ViewData["PageStatusId"] = new SelectList(_context.PageStatuses, "Id", "Name", dynamicPage.PageStatusId);
            return View(dynamicPage);
        }

        // GET: DynamicPages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dynamicPage = await _context.DynamicPages.FindAsync(id);
            if (dynamicPage == null)
            {
                return NotFound();
            }
            ViewData["PageCategoryId"] = new SelectList(_context.PageCategory, "Id", "Name", dynamicPage.PageCategoryId);
            ViewData["PageStatusId"] = new SelectList(_context.PageStatuses, "Id", "Name", dynamicPage.PageStatusId);
            return View(dynamicPage);
        }

        // POST: DynamicPages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Slug,Content,MetaDescription,MetaKeywords,PageStatusId,IsPublished,CreatedAt,UpdatedAt,PageCategoryId")] DynamicPage dynamicPage)
        {
            if (id != dynamicPage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dynamicPage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DynamicPageExists(dynamicPage.Id))
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
            ViewData["PageCategoryId"] = new SelectList(_context.PageCategory, "Id", "Name", dynamicPage.PageCategoryId);
            ViewData["PageStatusId"] = new SelectList(_context.PageStatuses, "Id", "Name", dynamicPage.PageStatusId);
            return View(dynamicPage);
        }

        // GET: DynamicPages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dynamicPage = await _context.DynamicPages
                .Include(d => d.PageCategory)
                .Include(d => d.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dynamicPage == null)
            {
                return NotFound();
            }

            return View(dynamicPage);
        }

        // POST: DynamicPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dynamicPage = await _context.DynamicPages.FindAsync(id);
            if (dynamicPage != null)
            {
                _context.DynamicPages.Remove(dynamicPage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DynamicPageExists(int id)
        {
            return _context.DynamicPages.Any(e => e.Id == id);
        }
    }
}
