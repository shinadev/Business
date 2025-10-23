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
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var comments = _context.Comments.Include(c => c.BlogDetail);
            return View(await comments.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var comment = await _context.Comments
                .Include(c => c.BlogDetail)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (comment == null) return NotFound();
            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["BlogDetailId"] = new SelectList(_context.BlogDetails, "Id", "Title");
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                if (comment.AuthorImageFile != null)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/Comments");
                    Directory.CreateDirectory(uploads);

                    var fileName = Path.GetFileName(comment.AuthorImageFile.FileName);
                    var filePath = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await comment.AuthorImageFile.CopyToAsync(stream);
                    }

                    comment.AuthorImageUrl = "/uploads/Comments/" + fileName;
                }

                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BlogDetailId"] = new SelectList(_context.BlogDetails, "Id", "Title", comment.BlogDetailId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound();

            ViewData["BlogDetailId"] = new SelectList(_context.BlogDetails, "Id", "Title", comment.BlogDetailId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AuthorName,AuthorImageUrl,Date,Content,IsReply,BlogDetailId")] Comment comment, IFormFile AuthorImage)
        {
            if (id != comment.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle new image upload
                    if (AuthorImage != null)
                    {
                        var fileName = Path.GetFileName(AuthorImage.FileName);
                        var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/Comments");
                        Directory.CreateDirectory(uploads);
                        var filePath = Path.Combine(uploads, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await AuthorImage.CopyToAsync(stream);
                        }

                        comment.AuthorImageUrl = "/uploads/Comments/" + fileName;
                    }
                    // If no new image, keep old AuthorImageUrl

                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Comments.Any(e => e.Id == comment.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["BlogDetailId"] = new SelectList(_context.BlogDetails, "Id", "Title", comment.BlogDetailId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var comment = await _context.Comments
                .Include(c => c.BlogDetail)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (comment == null) return NotFound();
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null) _context.Comments.Remove(comment);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
