using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Business.Data;
using Business.Models.Contact;

namespace Business.Controllers.Contact
{
    public class ContactHeadersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ContactHeadersController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: ContactHeaders
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContactHeaders.ToListAsync());
        }

        // GET: ContactHeaders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var contactHeader = await _context.ContactHeaders
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contactHeader == null) return NotFound();

            return View(contactHeader);
        }

        // GET: ContactHeaders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContactHeaders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactHeader contactHeader)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload
                if (contactHeader.ImageFile != null && contactHeader.ImageFile.Length > 0)
                {
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/Hcontact");
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(contactHeader.ImageFile.FileName);
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await contactHeader.ImageFile.CopyToAsync(stream);
                    }

                    contactHeader.ImageUrl = "/uploads/Hcontact/" + uniqueFileName;
                }

                _context.Add(contactHeader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactHeader);
        }

        // GET: ContactHeaders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var contactHeader = await _context.ContactHeaders.FindAsync(id);
            if (contactHeader == null) return NotFound();

            return View(contactHeader);
        }

        // POST: ContactHeaders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContactHeader contactHeader)
        {
            if (id != contactHeader.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingHeader = await _context.ContactHeaders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                    if (existingHeader == null) return NotFound();

                    // Handle new file upload
                    if (contactHeader.ImageFile != null && contactHeader.ImageFile.Length > 0)
                    {
                        string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/Hcontact");
                        if (!Directory.Exists(uploadFolder))
                        {
                            Directory.CreateDirectory(uploadFolder);
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(contactHeader.ImageFile.FileName);
                        string filePath = Path.Combine(uploadFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await contactHeader.ImageFile.CopyToAsync(stream);
                        }

                        // Delete old file if exists
                        if (!string.IsNullOrEmpty(existingHeader.ImageUrl))
                        {
                            string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, existingHeader.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        contactHeader.ImageUrl = "/uploads/Hcontact/" + uniqueFileName;
                    }
                    else
                    {
                        // Keep old image if new one not uploaded
                        contactHeader.ImageUrl = existingHeader.ImageUrl;
                    }

                    _context.Update(contactHeader);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactHeaderExists(contactHeader.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contactHeader);
        }

        // GET: ContactHeaders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var contactHeader = await _context.ContactHeaders
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contactHeader == null) return NotFound();

            return View(contactHeader);
        }

        // POST: ContactHeaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactHeader = await _context.ContactHeaders.FindAsync(id);
            if (contactHeader != null)
            {
                // Delete file from folder
                if (!string.IsNullOrEmpty(contactHeader.ImageUrl))
                {
                    string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, contactHeader.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                _context.ContactHeaders.Remove(contactHeader);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ContactHeaderExists(int id)
        {
            return _context.ContactHeaders.Any(e => e.Id == id);
        }
    }
}
