using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Business.Data;
using Business.Models.Menu;

namespace Business.Controllers.Menu
{
    public class FooterLinksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FooterLinksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FooterLinks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.FooterLink.Include(f => f.FooterMenu);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: FooterLinks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var footerLink = await _context.FooterLink
                .Include(f => f.FooterMenu)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (footerLink == null)
            {
                return NotFound();
            }

            return View(footerLink);
        }

        // GET: FooterLinks/Create
        public IActionResult Create()
        {
            ViewData["FooterMenuId"] = new SelectList(_context.FooterMenus, "Id", "Id");
            return View();
        }

        // POST: FooterLinks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Url,LinkType,FooterMenuId")] FooterLink footerLink)
        {
            if (ModelState.IsValid)
            {
                _context.Add(footerLink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FooterMenuId"] = new SelectList(_context.FooterMenus, "Id", "Id", footerLink.FooterMenuId);
            return View(footerLink);
        }

        // GET: FooterLinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var footerLink = await _context.FooterLink.FindAsync(id);
            if (footerLink == null)
            {
                return NotFound();
            }
            ViewData["FooterMenuId"] = new SelectList(_context.FooterMenus, "Id", "Id", footerLink.FooterMenuId);
            return View(footerLink);
        }

        // POST: FooterLinks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Url,LinkType,FooterMenuId")] FooterLink footerLink)
        {
            if (id != footerLink.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(footerLink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FooterLinkExists(footerLink.Id))
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
            ViewData["FooterMenuId"] = new SelectList(_context.FooterMenus, "Id", "Id", footerLink.FooterMenuId);
            return View(footerLink);
        }

        // GET: FooterLinks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var footerLink = await _context.FooterLink
                .Include(f => f.FooterMenu)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (footerLink == null)
            {
                return NotFound();
            }

            return View(footerLink);
        }

        // POST: FooterLinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var footerLink = await _context.FooterLink.FindAsync(id);
            if (footerLink != null)
            {
                _context.FooterLink.Remove(footerLink);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FooterLinkExists(int id)
        {
            return _context.FooterLink.Any(e => e.Id == id);
        }
    }
}
