using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Business.Data;
using Business.Models.Blog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Business.Controllers.Blogs
{
    public class RecentPostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecentPostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RecentPosts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RecentPosts.Include(r => r.BlogDetail);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RecentPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var recentPost = await _context.RecentPosts
                .Include(r => r.BlogDetail)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (recentPost == null) return NotFound();

            return View(recentPost);
        }

        // GET: RecentPosts/Create
        public IActionResult Create()
        {
            ViewData["BlogDetailId"] = new SelectList(_context.BlogDetails, "Id", "Title");
            return View();
        }

        // POST: RecentPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecentPost recentPost)
        {
            if (ModelState.IsValid)
            {
                // Handle image upload
                if (recentPost.ImageFile != null)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/RecentPosts");
                    Directory.CreateDirectory(uploads);

                    var fileName = Path.GetFileName(recentPost.ImageFile.FileName);
                    var filePath = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await recentPost.ImageFile.CopyToAsync(stream);
                    }

                    recentPost.ImageUrl = "/uploads/RecentPosts/" + fileName;
                }

                _context.Add(recentPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BlogDetailId"] = new SelectList(_context.BlogDetails, "Id", "Title", recentPost.BlogDetailId);
            return View(recentPost);
        }

        // GET: RecentPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var recentPost = await _context.RecentPosts.FindAsync(id);
            if (recentPost == null) return NotFound();

            ViewData["BlogDetailId"] = new SelectList(_context.BlogDetails, "Id", "Title", recentPost.BlogDetailId);
            return View(recentPost);
        }

        // POST: RecentPosts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RecentPost recentPost)
        {
            if (id != recentPost.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle new image upload
                    if (recentPost.ImageFile != null)
                    {
                        var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/RecentPosts");
                        Directory.CreateDirectory(uploads);

                        var fileName = Path.GetFileName(recentPost.ImageFile.FileName);
                        var filePath = Path.Combine(uploads, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await recentPost.ImageFile.CopyToAsync(stream);
                        }

                        recentPost.ImageUrl = "/uploads/RecentPosts/" + fileName;
                    }

                    _context.Update(recentPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.RecentPosts.Any(e => e.Id == recentPost.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["BlogDetailId"] = new SelectList(_context.BlogDetails, "Id", "Title", recentPost.BlogDetailId);
            return View(recentPost);
        }

        // GET: RecentPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var recentPost = await _context.RecentPosts
                .Include(r => r.BlogDetail)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (recentPost == null) return NotFound();
            return View(recentPost);
        }

        // POST: RecentPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recentPost = await _context.RecentPosts.FindAsync(id);
            if (recentPost != null) _context.RecentPosts.Remove(recentPost);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecentPostExists(int id)
        {
            return _context.RecentPosts.Any(e => e.Id == id);
        }
    }
}
