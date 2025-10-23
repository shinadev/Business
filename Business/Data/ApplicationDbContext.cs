using Business.Models.Menu;
using Business.Models.Page;
using Business.Models.Home;
using Business.Models.User;
using Microsoft.EntityFrameworkCore;
using Category = Business.Models.Menu.Category;
using Business.Models.About;
using Business.Models.Service;
using Business.Models.Team;
using Business.Models.Testimonial;
using Business.Models.Blog;
using Business.Models.Vendor;
using Business.Models.Contact;
using Business.Models.Site;

namespace Business.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // User & Auth
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<LoginHistory> LoginHistories { get; set; }

        // Menu
        public DbSet<NavMenu> NavMenus { get; set; }
        public DbSet<FooterMenu> FooterMenus { get; set; }
        public DbSet<TopBarMenu> TopBarMenus { get; set; }
        public DbSet<Category> Categories { get; set; }

        // Page
        public DbSet<DynamicPage> DynamicPages { get; set; }
        public DbSet<PageCategory> PageCategory { get; set; }
        public DbSet<PageStatus> PageStatuses { get; set; }
        public DbSet<PagePlug> PagePlugs { get; set; }
        public DbSet<PageLayoutSection> PageLayoutSections { get; set; }
        public DbSet<LayoutSection> LayoutSections { get; set; }

        // Footer
        public DbSet<FooterLink> FooterLink { get; set; } = default!;
        public DbSet<FooterSocial> FooterSocial { get; set; } = default!;

        // Home
        public DbSet<HomeSection> HomeSection { get; set; }

        // About
        public DbSet<AboutHeader> AboutHeader { get; set; }
        public DbSet<AboutSection> About { get; set; }

        // Services
        public DbSet<ServiceHeader> ServiceHeader { get; set; }
        public DbSet<ServiceSection> ServiceSection { get; set; }
        public DbSet<ServiceItem> ServiceItem { get; set; }
        
        //Team
        public DbSet<TeamHeader> TeamHeader { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<TeamSection> TeamSection { get; set; }

        // Testimonial
        // Testimonial
        public DbSet<Testimonials> Testimonial { get; set; } = null!;
        public DbSet<TestimonialHeader> TestimonialHeader { get; set; }
        public DbSet<TestimonialSection> TestimonialSection { get; set; }

        // Blog
        public DbSet<BlogHeader> BlogHeaders { get; set; }
        public DbSet<BlogDetail> BlogDetails { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<RecentPost> RecentPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PlainText> PlainTexts { get; set; }

        //Vendor
        public DbSet<Vendors> Vendors { get; set; }

        //Contact
        public DbSet<ContactSection> ContactSections { get; set; }
        public DbSet<ContactHeader> ContactHeaders { get; set; }

        // Site
        public DbSet<Logo> Logos { get; set; }
        public DbSet<Favicon> Favicons { get; set; }

        // CallToAction as a separate entity
        public DbSet<CallToAction> CallToAction { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // FooterMenu -> PopularLinks (one-to-many cascade delete)
            modelBuilder.Entity<FooterMenu>()
                .HasMany(f => f.PopularLinks)
                .WithOne(l => l.FooterMenu)
                .HasForeignKey(l => l.FooterMenuId)
                .OnDelete(DeleteBehavior.Cascade);

            // ServiceSection -> ServiceItem (one-to-many restrict delete)
            modelBuilder.Entity<ServiceSection>()
                .HasMany(s => s.Services)
                .WithOne(si => si.ServiceSection)
                .HasForeignKey(si => si.ServiceSectionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Remove owned CallToAction configuration
            // (Because CallToAction is now a separate entity)

            // Configure ServiceSection properties length constraints
            modelBuilder.Entity<ServiceSection>(entity =>
            {
                entity.Property(s => s.SmallTitle).HasMaxLength(100);
                entity.Property(s => s.MainTitle).HasMaxLength(200);
            });

            // Optional: configure CallToAction property constraints here
            modelBuilder.Entity<CallToAction>(entity =>
            {
                entity.Property(c => c.Title).HasMaxLength(100);
                entity.Property(c => c.Description).HasMaxLength(300);
                entity.Property(c => c.PhoneNumber).HasMaxLength(20);
            });
        }
    }
}
