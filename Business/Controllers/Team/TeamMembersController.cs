using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Business.Data;
using Business.Models.Team;

namespace Business.Controllers.Team
{
    public class TeamMembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeamMembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TeamMembers
        public async Task<IActionResult> Index()
        {
            var teamMembers = await _context.TeamMembers
                .Include(t => t.TeamSection) // Include section for display
                .ToListAsync();
            return View(teamMembers);
        }

        // GET: TeamMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var teamMember = await _context.TeamMembers
                .Include(t => t.TeamSection)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (teamMember == null) return NotFound();
            return View(teamMember);
        }

        // GET: TeamMembers/Create
        public IActionResult Create()
        {
            ViewData["TeamSections"] = _context.TeamSection.ToList();
            return View();
        }

        // POST: TeamMembers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamMember teamMember)
        {
            if (ModelState.IsValid)
            {
                // Upload image if provided
                if (teamMember.ImageFile != null && teamMember.ImageFile.Length > 0)
                {
                    teamMember.ImagePath = await SaveImageAsync(teamMember.ImageFile);
                }

                // Ensure TeamSectionId is set (link to a section)
                if (teamMember.TeamSectionId == 0)
                    teamMember.TeamSectionId = _context.TeamSection.FirstOrDefault()?.Id ?? 1;

                _context.Add(teamMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["TeamSections"] = _context.TeamSection.ToList();
            return View(teamMember);
        }

        // GET: TeamMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var teamMember = await _context.TeamMembers.FindAsync(id);
            if (teamMember == null) return NotFound();

            ViewData["TeamSections"] = _context.TeamSection.ToList();
            return View(teamMember);
        }

        // POST: TeamMembers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TeamMember teamMember)
        {
            if (id != teamMember.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.TeamMembers.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
                    if (existing == null) return NotFound();

                    // Handle image upload
                    if (teamMember.ImageFile != null && teamMember.ImageFile.Length > 0)
                    {
                        DeleteImageIfExists(existing.ImagePath);
                        teamMember.ImagePath = await SaveImageAsync(teamMember.ImageFile);
                    }
                    else
                    {
                        teamMember.ImagePath = existing.ImagePath;
                    }

                    _context.Update(teamMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamMemberExists(teamMember.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["TeamSections"] = _context.TeamSection.ToList();
            return View(teamMember);
        }

        // GET: TeamMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var teamMember = await _context.TeamMembers
                .Include(t => t.TeamSection)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teamMember == null) return NotFound();

            return View(teamMember);
        }

        // POST: TeamMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamMember = await _context.TeamMembers.FindAsync(id);
            if (teamMember != null)
            {
                DeleteImageIfExists(teamMember.ImagePath);
                _context.TeamMembers.Remove(teamMember);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TeamMemberExists(int id)
        {
            return _context.TeamMembers.Any(e => e.Id == id);
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "team");
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadDir, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return $"/uploads/team/{fileName}";
        }

        private void DeleteImageIfExists(string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return;

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}
