using System.Diagnostics;
using Business.Data;
using Business.Models;
using Business.Models.About;
using Business.Models.Blog;
using Business.Models.Contact;
using Business.Models.Home;
using Business.Models.Service;
using Business.Models.Team;
using Business.Models.Testimonial;
using Business.Models.Vendor;
using Business.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Business.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Homepage
        public IActionResult Index()
        {
            var viewModel = new HomePageViewModel
            {
                HomeSections = _context.HomeSection
                    .Where(h => h.IsActive)
                    .OrderBy(h => h.DisplayOrder)
                    .ToList(),

                AboutSection = _context.About.FirstOrDefault() ?? new AboutSection(),

                Vendors = _context.Vendors.ToList(),

                ServiceSection = _context.ServiceSection
                    .Include(s => s.Services)
                    .FirstOrDefault() ?? new ServiceSection()
            };

            return View(viewModel);
        }

        // About page
        public IActionResult About()
        {
            var viewModel = new AboutPageViewModel
            {
                Header = _context.AboutHeader.FirstOrDefault(),
                Section = _context.About.FirstOrDefault() ?? new AboutSection()
            };

            return View(viewModel);
        }

        // Service page
        public IActionResult Service()
        {
            var section = _context.ServiceSection
                .Include(s => s.Services)
                .Include(s => s.CallUsForQuote)
                .FirstOrDefault();

            var header = _context.ServiceHeader.FirstOrDefault();

            var viewModel = new ServicePageViewModel
            {
                Header = header,
                Section = section,
                Services = section?.Services ?? new List<ServiceItem>(),
                CallToAction = section?.CallUsForQuote
            };

            return View(viewModel);
        }

        // Blog detail page
        public IActionResult BlogDetail(int id)
        {
            var blog = _context.BlogDetails
                .Include(b => b.Comments)
                .Include(b => b.Categories)
                .Include(b => b.RecentPosts)
                .Include(b => b.Tags)
                .Include(b => b.PlainText)
                .FirstOrDefault(b => b.Id == id);

            if (blog == null)
                return NotFound();

            var viewModel = new BlogDetailViewModel
            {
                Blog = blog,
                Comments = blog.Comments.OrderByDescending(c => c.Date).ToList(),
                Categories = blog.Categories.ToList(),
                RecentPosts = blog.RecentPosts.ToList(),
                Tags = blog.Tags.ToList(),
                PlainText = blog.PlainText,
                BlogHeader = _context.BlogHeaders.FirstOrDefault() ?? new BlogHeader()
            };

            return View(viewModel);
        }

        // Team page
        public IActionResult Team()
        {
            // Fetch header
            var header = _context.TeamHeader.FirstOrDefault() ?? new TeamHeader
            {
                Title = "Our Team",
                BreadcrumbHomeText = "Home",
                BreadcrumbHomeUrl = "/",
                BreadcrumbCurrentText = "Team",
                BreadcrumbCurrentUrl = "/team",
                ImageUrl = "/img/team-header.jpg"
            };

            // Fetch the team section and all members
            var section = _context.TeamSection
                .Include(s => s.TeamMember)
                .FirstOrDefault(); // or .FirstOrDefault(s => s.Id == 1) if you have multiple sections

            if (section == null)
            {
                section = new TeamSection
                {
                    SectionTitle = "Team Members",
                    MainHeading = "Professional Staff Ready to Help Your Business",
                    TeamMember = new List<TeamMember>()
                };
            }

            // Build view model
            var viewModel = new TeamPageViewModel
            {
                Header = header,
                Section = section
            };

            return View(viewModel);
        }

        public IActionResult Testimonial()
        {
            // Load section & testimonials
            var testimonialSection = _context.TestimonialSection
                .Include(ts => ts.Testimonials)
                .FirstOrDefault();

            if (testimonialSection == null)
            {
                testimonialSection = new TestimonialSection
                {
                    SectionTitle = "Testimonials",
                    MainHeading = "What our clients say",
                    Testimonials = new List<Testimonials>()
                };
            }

            // Load vendors
            var vendors = _context.Vendors.ToList();

            var vm = new TestimonialPageViewModel
            {
                TestimonialSection = testimonialSection,
                Vendors = vendors
            };

            return View(vm);
        }

        // Contact page
        public IActionResult Contact()
        {
            var sections = _context.ContactSections.ToList();

            var viewModel = new ContactPageViewModel
            {
                Header = _context.ContactHeaders.FirstOrDefault(),
                Sections = sections,
                MapUrl = sections.FirstOrDefault()?.MapUrl
            };

            return View(viewModel);
        }

        // Error page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
