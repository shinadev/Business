using Business.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🧩 Add services to the container
builder.Services.AddControllersWithViews();

// 🗄️ Configure Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 💾 Add Session and Caching
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // 30-minute timeout
    options.Cookie.HttpOnly = true;                  // Prevent JS access
    options.Cookie.IsEssential = true;               // Required for GDPR compliance
});

var app = builder.Build();

// 🚀 Apply Database Migrations Automatically
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();  // Ensures DB is created/updated
}

// 🌐 Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();         // Enable session support
app.UseAuthentication();  // Enable authentication if configured
app.UseAuthorization();   // Enable authorization

// 🧭 Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
