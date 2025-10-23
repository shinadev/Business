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
    public class FooterSocialsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FooterSocialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FooterSocials
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.FooterSocial.Include(f => f.FooterMenu);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: FooterSocials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var footerSocial = await _context.FooterSocial
                .Include(f => f.FooterMenu)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (footerSocial == null)
            {
                return NotFound();
            }

            return View(footerSocial);
        }

        // GET: FooterSocials/Create
        public IActionResult Create()
        {
            ViewData["FooterMenuId"] = new SelectList(_context.FooterMenus, "Id", "Id");
            return View();
        }

        // POST: FooterSocials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Platform,IconClass,Url,FooterMenuId")] FooterSocial footerSocial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(footerSocial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FooterMenuId"] = new SelectList(_context.FooterMenus, "Id", "Id", footerSocial.FooterMenuId);
            return View(footerSocial);
        }

        // GET: FooterSocials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var footerSocial = await _context.FooterSocial.FindAsync(id);
            if (footerSocial == null)
            {
                return NotFound();
            }
            ViewData["FooterMenuId"] = new SelectList(_context.FooterMenus, "Id", "Id", footerSocial.FooterMenuId);
            return View(footerSocial);
        }

        // POST: FooterSocials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Platform,IconClass,Url,FooterMenuId")] FooterSocial footerSocial)
        {
            if (id != footerSocial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(footerSocial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FooterSocialExists(footerSocial.Id))
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
            ViewData["FooterMenuId"] = new SelectList(_context.FooterMenus, "Id", "Id", footerSocial.FooterMenuId);
            return View(footerSocial);
        }

        // GET: FooterSocials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var footerSocial = await _context.FooterSocial
                .Include(f => f.FooterMenu)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (footerSocial == null)
            {
                return NotFound();
            }

            return View(footerSocial);
        }

        // POST: FooterSocials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var footerSocial = await _context.FooterSocial.FindAsync(id);
            if (footerSocial != null)
            {
                _context.FooterSocial.Remove(footerSocial);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FooterSocialExists(int id)
        {
            return _context.FooterSocial.Any(e => e.Id == id);
        }
    }
}
