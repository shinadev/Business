using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Business.Data;
using Business.Models.Blog;

namespace Business.Controllers.Blog
{
    public class BlogDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogDetailsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var blog = await _context.BlogDetails
                .Include(b => b.Comments)
                .Include(b => b.Categories)
                .Include(b => b.Tags)
                .Include(b => b.RecentPosts)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (blog == null) return NotFound();

            return View(blog);
        }


        // GET: BlogDetails
        public async Task<IActionResult> Index()
        {
            var blogs = await _context.BlogDetails
                                      .Include(b => b.Comments)
                                      .Include(b => b.Categories)
                                      .Include(b => b.Tags)
                                      .ToListAsync();
            return View(blogs);
        }

        // GET: BlogDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BlogDetails/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogDetail blog, IFormFile MainImage, IFormFile SidebarImage)
        {
            Console.WriteLine("🛠 DEBUG: Create action hit");

            // Remove validation for image URLs and PlainText (they will be set in code)
            ModelState.Remove(nameof(BlogDetail.MainImageUrl));
            ModelState.Remove(nameof(BlogDetail.SidebarImageUrl));
            ModelState.Remove(nameof(BlogDetail.PlainText));

            if (!ModelState.IsValid)
            {
                Console.WriteLine("❌ ModelState is still invalid after removing some fields.");
                foreach (var error in ModelState)
                {
                    foreach (var err in error.Value.Errors)
                    {
                        Console.WriteLine($"   Field: {error.Key} | Error: {err.ErrorMessage}");
                    }
                }
                return View(blog);
            }

            try
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/Blogs");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                // Upload Main Image
                if (MainImage != null)
                {
                    string mainImageFileName = Guid.NewGuid() + Path.GetExtension(MainImage.FileName);
                    string mainImagePath = Path.Combine(uploadDir, mainImageFileName);
                    using (var stream = new FileStream(mainImagePath, FileMode.Create))
                    {
                        await MainImage.CopyToAsync(stream);
                    }
                    blog.MainImageUrl = "/uploads/Blogs/" + mainImageFileName;
                }

                // Upload Sidebar Image
                if (SidebarImage != null)
                {
                    string sidebarImageFileName = Guid.NewGuid() + Path.GetExtension(SidebarImage.FileName);
                    string sidebarImagePath = Path.Combine(uploadDir, sidebarImageFileName);
                    using (var stream = new FileStream(sidebarImagePath, FileMode.Create))
                    {
                        await SidebarImage.CopyToAsync(stream);
                    }
                    blog.SidebarImageUrl = "/uploads/Blogs/" + sidebarImageFileName;
                }

                _context.Add(blog);
                int result = await _context.SaveChangesAsync();
                Console.WriteLine($"💾 SaveChangesAsync result: {result} rows affected");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 ERROR in Create: {ex.Message}");
                return View(blog);
            }
        }

        // GET: BlogDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var blog = await _context.BlogDetails.FindAsync(id);
            if (blog == null) return NotFound();

            return View(blog);
        }

        // POST: BlogDetails/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogDetail blog, IFormFile MainImage, IFormFile SidebarImage)
        {
            if (id != blog.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingBlog = await _context.BlogDetails.FindAsync(id);

                    existingBlog.Title = blog.Title;
                    existingBlog.Paragraphs = blog.Paragraphs;
                    existingBlog.PlainText = blog.PlainText;
                    existingBlog.Comments = blog.Comments;
                    existingBlog.Categories = blog.Categories;
                    existingBlog.RecentPosts = blog.RecentPosts;
                    existingBlog.Tags = blog.Tags;

                    // Update images if new ones are uploaded
                    if (MainImage != null)
                    {
                        string mainImageFileName = Guid.NewGuid() + Path.GetExtension(MainImage.FileName);
                        string mainImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/Blogs", mainImageFileName);
                        using (var stream = new FileStream(mainImagePath, FileMode.Create))
                        {
                            await MainImage.CopyToAsync(stream);
                        }
                        existingBlog.MainImageUrl = "/uploads/Blogs/" + mainImageFileName;
                    }

                    if (SidebarImage != null)
                    {
                        string sidebarImageFileName = Guid.NewGuid() + Path.GetExtension(SidebarImage.FileName);
                        string sidebarImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/Blogs", sidebarImageFileName);
                        using (var stream = new FileStream(sidebarImagePath, FileMode.Create))
                        {
                            await SidebarImage.CopyToAsync(stream);
                        }
                        existingBlog.SidebarImageUrl = "/uploads/Blogs/" + sidebarImageFileName;
                    }

                    _context.Update(existingBlog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.BlogDetails.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: BlogDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var blog = await _context.BlogDetails
                                     .Include(b => b.Categories)
                                     .Include(b => b.Tags)
                                     .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null) return NotFound();

            return View(blog);
        }

        // POST: BlogDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _context.BlogDetails.FindAsync(id);

            if (!string.IsNullOrEmpty(blog.MainImageUrl))
            {
                var mainImagePath = Path.Combine(_webHostEnvironment.WebRootPath, blog.MainImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(mainImagePath)) System.IO.File.Delete(mainImagePath);
            }

            if (!string.IsNullOrEmpty(blog.SidebarImageUrl))
            {
                var sidebarImagePath = Path.Combine(_webHostEnvironment.WebRootPath, blog.SidebarImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(sidebarImagePath)) System.IO.File.Delete(sidebarImagePath);
            }

            _context.BlogDetails.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
