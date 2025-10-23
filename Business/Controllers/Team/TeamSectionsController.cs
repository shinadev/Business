using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Business.Data;
using Business.Models.Team;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Business.Controllers.Team
{
    public class TeamSectionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeamSectionsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: TeamSections
        public async Task<IActionResult> Index()
        {
            var sections = await _context.TeamSection.Include(s => s.TeamMember).ToListAsync();
            return View(sections);
        }

        // GET: TeamSections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var section = await _context.TeamSection
                .Include(s => s.TeamMember)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (section == null) return NotFound();

            return View(section);
        }

        // GET: TeamSections/Create
        public IActionResult Create() => View();

        // POST: TeamSections/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SectionTitle,MainHeading")] TeamSection section)
        {
            if (ModelState.IsValid)
            {
                _context.Add(section);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(section);
        }

        // GET: TeamSections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var section = await _context.TeamSection
                .Include(s => s.TeamMember)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (section == null) return NotFound();

            return View(section);
        }

        // POST: TeamSections/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SectionTitle,MainHeading")] TeamSection section)
        {
            if (id != section.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(section);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.TeamSection.Any(e => e.Id == section.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(section);
        }

        // GET: TeamSections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var section = await _context.TeamSection
                .FirstOrDefaultAsync(s => s.Id == id);

            if (section == null) return NotFound();

            return View(section);
        }

        // POST: TeamSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var section = await _context.TeamSection.FindAsync(id);
            if (section != null)
            {
                _context.TeamSection.Remove(section);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TeamSectionExists(int id) => _context.TeamSection.Any(e => e.Id == id);

        // ========== TEAM MEMBER CRUD ==========

        // GET: TeamSections/AddMember/5
        public async Task<IActionResult> AddMember(int sectionId)
        {
            ViewBag.SectionId = sectionId;
            return View();
        }

        // POST: TeamSections/AddMember/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMember(int sectionId, TeamMember member)
        {
            if (ModelState.IsValid)
            {
                member.TeamSectionId = sectionId;

                // Handle Image Upload
                if (member.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/team");
                    Directory.CreateDirectory(uploadsFolder);
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(member.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await member.ImageFile.CopyToAsync(fileStream);
                    }
                    member.ImagePath = "/images/team/" + fileName;
                }

                _context.TeamMembers.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = sectionId });
            }
            ViewBag.SectionId = sectionId;
            return View(member);
        }

        // GET: TeamSections/EditMember/5
        public async Task<IActionResult> EditMember(int? id)
        {
            if (id == null) return NotFound();

            var member = await _context.TeamMembers.FindAsync(id);
            if (member == null) return NotFound();

            return View(member);
        }

        // POST: TeamSections/EditMember/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMember(int id, TeamMember member)
        {
            if (id != member.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (member.ImageFile != null)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/team");
                        Directory.CreateDirectory(uploadsFolder);
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(member.ImageFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await member.ImageFile.CopyToAsync(fileStream);
                        }
                        member.ImagePath = "/images/team/" + fileName;
                    }

                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.TeamMembers.Any(e => e.Id == member.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Edit), new { id = member.TeamSectionId });
            }
            return View(member);
        }

        // GET: TeamSections/DeleteMember/5
        public async Task<IActionResult> DeleteMember(int? id)
        {
            if (id == null) return NotFound();

            var member = await _context.TeamMembers.FindAsync(id);
            if (member == null) return NotFound();

            return View(member);
        }

        // POST: TeamSections/DeleteMember/5
        [HttpPost, ActionName("DeleteMember")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMemberConfirmed(int id)
        {
            var member = await _context.TeamMembers.FindAsync(id);
            if (member != null)
            {
                int? sectionId = member.TeamSectionId;
                _context.TeamMembers.Remove(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = sectionId });
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
