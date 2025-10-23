using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Business.Data;
using Business.Models.Service;
using Business.ViewModels;
using Microsoft.Extensions.Logging;

namespace Business.Controllers.Service
{
    public class ServiceSectionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ServiceSectionsController> _logger;

        public ServiceSectionsController(ApplicationDbContext context, ILogger<ServiceSectionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ServiceSections
        public async Task<IActionResult> Index()
        {
            var list = await _context.ServiceSection
                                     .Include(s => s.Services)
                                     .ToListAsync();
            return View(list);
        }

        // GET: ServiceSections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var section = await _context.ServiceSection
                                        .Include(s => s.Services)
                                        .Include(s => s.CallUsForQuote)
                                        .FirstOrDefaultAsync(m => m.Id == id);

            if (section == null) return NotFound();

            var vm = new ServiceSectionViewModel
            {
                ServiceSection = section,
                CallToAction = section.CallUsForQuote
            };

            return View(vm);
        }

        // GET: ServiceSections/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceSections/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SmallTitle,MainTitle")] ServiceSection serviceSection)
        {
            if (ModelState.IsValid)
            {
                _context.ServiceSection.Add(serviceSection);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Service Section created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(serviceSection);
        }

        // GET: ServiceSections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var section = await _context.ServiceSection.FindAsync(id);
            if (section == null) return NotFound();

            var vm = new ServiceSectionViewModel
            {
                ServiceSection = section,
                CallToAction = await _context.CallToAction
                                             .FirstOrDefaultAsync(c => c.ServiceSectionId == id)
            };

            return View(vm);
        }

        // POST: ServiceSections/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceSectionViewModel vm)
        {
            if (id != vm.ServiceSection.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vm.ServiceSection);

                    if (vm.CallToAction != null)
                    {
                        if (vm.CallToAction.Id > 0)
                            _context.Update(vm.CallToAction);
                        else
                        {
                            vm.CallToAction.ServiceSectionId = id;
                            _context.Add(vm.CallToAction);
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Service Section updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ServiceSection.Any(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(vm);
        }

        // GET: ServiceSections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var section = await _context.ServiceSection
                                        .Include(s => s.CallUsForQuote)
                                        .FirstOrDefaultAsync(m => m.Id == id);

            if (section == null) return NotFound();

            var vm = new ServiceSectionViewModel
            {
                ServiceSection = section,
                CallToAction = section.CallUsForQuote
            };

            return View(vm);
        }

        // POST: ServiceSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var section = await _context.ServiceSection
                                        .Include(s => s.CallUsForQuote)
                                        .FirstOrDefaultAsync(s => s.Id == id);

            if (section != null)
            {
                if (section.CallUsForQuote != null)
                    _context.CallToAction.Remove(section.CallUsForQuote);

                _context.ServiceSection.Remove(section);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Service Section deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
