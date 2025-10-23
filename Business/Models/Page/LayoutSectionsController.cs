using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Business.Data;

namespace Business.Models.Page
{
    public class LayoutSectionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LayoutSectionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LayoutSections
        public async Task<IActionResult> Index()
        {
            return View(await _context.LayoutSections.ToListAsync());
        }

        // GET: LayoutSections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var layoutSection = await _context.LayoutSections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (layoutSection == null)
            {
                return NotFound();
            }

            return View(layoutSection);
        }

        // GET: LayoutSections/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LayoutSections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] LayoutSection layoutSection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(layoutSection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(layoutSection);
        }

        // GET: LayoutSections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var layoutSection = await _context.LayoutSections.FindAsync(id);
            if (layoutSection == null)
            {
                return NotFound();
            }
            return View(layoutSection);
        }

        // POST: LayoutSections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] LayoutSection layoutSection)
        {
            if (id != layoutSection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(layoutSection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LayoutSectionExists(layoutSection.Id))
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
            return View(layoutSection);
        }

        // GET: LayoutSections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var layoutSection = await _context.LayoutSections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (layoutSection == null)
            {
                return NotFound();
            }

            return View(layoutSection);
        }

        // POST: LayoutSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var layoutSection = await _context.LayoutSections.FindAsync(id);
            if (layoutSection != null)
            {
                _context.LayoutSections.Remove(layoutSection);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LayoutSectionExists(int id)
        {
            return _context.LayoutSections.Any(e => e.Id == id);
        }
    }
}
