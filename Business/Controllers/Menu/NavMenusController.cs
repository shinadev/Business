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
    public class NavMenusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NavMenusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NavMenus
        public async Task<IActionResult> Index()
        {
            return View(await _context.NavMenus.ToListAsync());
        }

        // GET: NavMenus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navMenu = await _context.NavMenus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (navMenu == null)
            {
                return NotFound();
            }

            return View(navMenu);
        }

        // GET: NavMenus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NavMenus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MenuName,Controller,Action,RouteId,RouteSlug,DropdownGroup,Order,IsButton")] NavMenu navMenu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(navMenu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(navMenu);
        }

        // GET: NavMenus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navMenu = await _context.NavMenus.FindAsync(id);
            if (navMenu == null)
            {
                return NotFound();
            }
            return View(navMenu);
        }

        // POST: NavMenus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MenuName,Controller,Action,RouteId,RouteSlug,DropdownGroup,Order,IsButton")] NavMenu navMenu)
        {
            if (id != navMenu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(navMenu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NavMenuExists(navMenu.Id))
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
            return View(navMenu);
        }

        // GET: NavMenus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navMenu = await _context.NavMenus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (navMenu == null)
            {
                return NotFound();
            }

            return View(navMenu);
        }

        // POST: NavMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var navMenu = await _context.NavMenus.FindAsync(id);
            if (navMenu != null)
            {
                _context.NavMenus.Remove(navMenu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NavMenuExists(int id)
        {
            return _context.NavMenus.Any(e => e.Id == id);
        }
    }
}
