using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Business.Data;
using Business.Models.Contact;

namespace Business.Controllers.Contact
{
    public class ContactSectionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactSectionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ContactSections
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContactSections.ToListAsync());
        }

        // GET: ContactSections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactSection = await _context.ContactSections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactSection == null)
            {
                return NotFound();
            }

            return View(contactSection);
        }

        // GET: ContactSections/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContactSections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SmallTitle,MainTitle,PhoneTitle,PhoneNumber,EmailTitle,EmailAddress,OfficeTitle,OfficeAddress,NamePlaceholder,EmailPlaceholder,SubjectPlaceholder,MessagePlaceholder,ButtonText,MapUrl")] ContactSection contactSection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactSection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactSection);
        }

        // GET: ContactSections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactSection = await _context.ContactSections.FindAsync(id);
            if (contactSection == null)
            {
                return NotFound();
            }
            return View(contactSection);
        }

        // POST: ContactSections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SmallTitle,MainTitle,PhoneTitle,PhoneNumber,EmailTitle,EmailAddress,OfficeTitle,OfficeAddress,NamePlaceholder,EmailPlaceholder,SubjectPlaceholder,MessagePlaceholder,ButtonText,MapUrl")] ContactSection contactSection)
        {
            if (id != contactSection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactSection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactSectionExists(contactSection.Id))
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
            return View(contactSection);
        }

        // GET: ContactSections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactSection = await _context.ContactSections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactSection == null)
            {
                return NotFound();
            }

            return View(contactSection);
        }

        // POST: ContactSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactSection = await _context.ContactSections.FindAsync(id);
            if (contactSection != null)
            {
                _context.ContactSections.Remove(contactSection);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactSectionExists(int id)
        {
            return _context.ContactSections.Any(e => e.Id == id);
        }
    }
}
