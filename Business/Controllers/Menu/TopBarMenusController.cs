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
    public class TopBarMenusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TopBarMenusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TopBarMenus
        public async Task<IActionResult> Index()
        {
            return View(await _context.TopBarMenus.ToListAsync());
        }

        // GET: TopBarMenus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topBarMenu = await _context.TopBarMenus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (topBarMenu == null)
            {
                return NotFound();
            }

            return View(topBarMenu);
        }

        // GET: TopBarMenus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TopBarMenus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Address,Phone,Email,TwitterUrl,FacebookUrl,LinkedInUrl,InstagramUrl,YouTubeUrl")] TopBarMenu topBarMenu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(topBarMenu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(topBarMenu);
        }

        // GET: TopBarMenus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topBarMenu = await _context.TopBarMenus.FindAsync(id);
            if (topBarMenu == null)
            {
                return NotFound();
            }
            return View(topBarMenu);
        }

        // POST: TopBarMenus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Address,Phone,Email,TwitterUrl,FacebookUrl,LinkedInUrl,InstagramUrl,YouTubeUrl")] TopBarMenu topBarMenu)
        {
            if (id != topBarMenu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topBarMenu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopBarMenuExists(topBarMenu.Id))
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
            return View(topBarMenu);
        }

        // GET: TopBarMenus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topBarMenu = await _context.TopBarMenus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (topBarMenu == null)
            {
                return NotFound();
            }

            return View(topBarMenu);
        }

        // POST: TopBarMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var topBarMenu = await _context.TopBarMenus.FindAsync(id);
            if (topBarMenu != null)
            {
                _context.TopBarMenus.Remove(topBarMenu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TopBarMenuExists(int id)
        {
            return _context.TopBarMenus.Any(e => e.Id == id);
        }
    }
}
