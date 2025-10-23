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
    public class PageStatusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PageStatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PageStatus
        public async Task<IActionResult> Index()
        {
            return View(await _context.PageStatuses.ToListAsync());
        }

        // GET: PageStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pageStatus = await _context.PageStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pageStatus == null)
            {
                return NotFound();
            }

            return View(pageStatus);
        }

        // GET: PageStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PageStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] PageStatus pageStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pageStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pageStatus);
        }

        // GET: PageStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pageStatus = await _context.PageStatuses.FindAsync(id);
            if (pageStatus == null)
            {
                return NotFound();
            }
            return View(pageStatus);
        }

        // POST: PageStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] PageStatus pageStatus)
        {
            if (id != pageStatus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pageStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PageStatusExists(pageStatus.Id))
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
            return View(pageStatus);
        }

        // GET: PageStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pageStatus = await _context.PageStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pageStatus == null)
            {
                return NotFound();
            }

            return View(pageStatus);
        }

        // POST: PageStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pageStatus = await _context.PageStatuses.FindAsync(id);
            if (pageStatus != null)
            {
                _context.PageStatuses.Remove(pageStatus);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PageStatusExists(int id)
        {
            return _context.PageStatuses.Any(e => e.Id == id);
        }
    }
}
