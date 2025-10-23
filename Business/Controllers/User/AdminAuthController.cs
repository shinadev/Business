using Business.Data;
using Business.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

public class AdminAuthController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminAuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var admin = await _context.AdminUsers
            .Include(a => a.Role)
            .FirstOrDefaultAsync(a => a.Email == email && a.Password == password);

        if (admin != null)
        {
            HttpContext.Session.SetString("AdminUsername", admin.Username);
            HttpContext.Session.SetString("AdminEmail", admin.Email);
            HttpContext.Session.SetString("AdminRole", admin.Role?.Name ?? "None");

            // Log login
            var history = new LoginHistory
            {
                AdminEmail = admin.Email,
                AdminUsername = admin.Username,
                Role = admin.Role?.Name,
                Action = "Login",
                Timestamp = DateTime.Now,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
            };
            _context.LoginHistories.Add(history);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "AdminDashboard");
        }

        ViewBag.Error = "Invalid credentials";
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        var email = HttpContext.Session.GetString("AdminEmail");
        var username = HttpContext.Session.GetString("AdminUsername");
        var role = HttpContext.Session.GetString("AdminRole");

        var history = new LoginHistory
        {
            AdminEmail = email,
            AdminUsername = username,
            Role = role,
            Action = "Logout",
            Timestamp = DateTime.Now,
            IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
        };

        _context.LoginHistories.Add(history);
        await _context.SaveChangesAsync();

        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
