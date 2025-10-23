using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Business.Data;
using Business.Models;// Your DbContext namespace
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class AdminDashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminDashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var adminEmail = HttpContext.Session.GetString("AdminEmail");
        if (string.IsNullOrEmpty(adminEmail))
        {
            return RedirectToAction("Login", "AdminAuth");
        }

        ViewBag.AdminUsername = HttpContext.Session.GetString("AdminUsername");
        ViewBag.AdminRole = HttpContext.Session.GetString("AdminRole");

        // Get counts
        ViewBag.UserCount = await _context.AdminUsers.CountAsync();
        ViewBag.BlogCount = await _context.BlogDetails.CountAsync();
        ViewBag.TestimonialCount = await _context.Testimonial.CountAsync();
        ViewBag.PageCount = await _context.DynamicPages.CountAsync();

        return View();
    }
}
