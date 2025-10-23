using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Business.Data;
using Business.Models.Team;

namespace Business.Controllers.Team
{
    public class TeamHeadersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeamHeadersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TeamHeaders
        public async Task<IActionResult> Index()
        {
            var teamHeaders = await _context.TeamHeader.ToListAsync();
            return View(teamHeaders);
        }

        // GET: TeamHeaders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var teamHeader = await _context.TeamHeader.FirstOrDefaultAsync(th => th.Id == id);
            if (teamHeader == null) return NotFound();

            return View(teamHeader);
        }

        // GET: TeamHeaders/Create
        public IActionResult Create()
        {
            return View(new TeamHeader());
        }

        // POST: TeamHeaders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamHeader teamHeader)
        {
            if (ModelState.IsValid)
            {
                if (teamHeader.ImageFile != null && teamHeader.ImageFile.Length > 0)
                {
                    teamHeader.ImageUrl = await SaveImageAsync(teamHeader.ImageFile);
                }

                _context.Add(teamHeader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teamHeader);
        }

        // GET: TeamHeaders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var teamHeader = await _context.TeamHeader.FindAsync(id);
            if (teamHeader == null) return NotFound();

            return View(teamHeader);
        }

        // POST: TeamHeaders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TeamHeader teamHeader)
        {
            if (id != teamHeader.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.TeamHeader.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
                    if (existing == null) return NotFound();

                    if (teamHeader.ImageFile != null && teamHeader.ImageFile.Length > 0)
                    {
                        DeleteImageIfExists(existing.ImageUrl);
                        teamHeader.ImageUrl = await SaveImageAsync(teamHeader.ImageFile);
                    }
                    else
                    {
                        teamHeader.ImageUrl = existing.ImageUrl; // keep old image if no new upload
                    }

                    _context.Update(teamHeader);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamHeaderExists(teamHeader.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(teamHeader);
        }

        // GET: TeamHeaders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var teamHeader = await _context.TeamHeader.FirstOrDefaultAsync(m => m.Id == id);
            if (teamHeader == null) return NotFound();

            return View(teamHeader);
        }

        // POST: TeamHeaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamHeader = await _context.TeamHeader.FindAsync(id);
            if (teamHeader != null)
            {
                DeleteImageIfExists(teamHeader.ImageUrl);

                _context.TeamHeader.Remove(teamHeader);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Check if TeamHeader exists by Id
        private bool TeamHeaderExists(int id)
        {
            return _context.TeamHeader.Any(e => e.Id == id);
        }

        // Save uploaded image to wwwroot/uploads/Hteam and return relative URL
        private async Task<string> SaveImageAsync(Microsoft.AspNetCore.Http.IFormFile imageFile)
        {
            string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "Hteam");

            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            string fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            string filePath = Path.Combine(uploadDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/uploads/Hteam/{fileName}";
        }

        // Delete image file from server if exists
        private void DeleteImageIfExists(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }
    }
}
