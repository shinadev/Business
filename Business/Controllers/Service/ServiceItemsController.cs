using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Business.Data;
using Business.Models.Service;

namespace Business.Controllers.Service
{
    public class ServiceItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ServiceItems
        public async Task<IActionResult> Index()
        {
            var serviceItems = _context.ServiceItem.
                Include(si => si.ServiceSection);
            return View(await serviceItems.ToListAsync());
        }

        // GET: ServiceItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var serviceItem = await _context.ServiceItem
                .Include(si => si.ServiceSection)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (serviceItem == null) return NotFound();

            return View(serviceItem);
        }

        // GET: ServiceItems/Create
        public IActionResult Create()
        {
            ViewData["ServiceSectionId"] = new SelectList(_context.ServiceSection, "Id", "MainTitle");
            return View();
        }

        // POST: ServiceItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IconClass,Title,Description,LinkUrl,ServiceSectionId")] ServiceItem serviceItem)
        {
            if (ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                _context.Add(serviceItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            ViewData["ServiceSectionId"] = new SelectList(_context.ServiceSection, "Id", "MainTitle", serviceItem.ServiceSectionId);
            return View(serviceItem);



        }

        // GET: ServiceItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var serviceItem = await _context.ServiceItem.FindAsync(id);
            if (serviceItem == null) return NotFound();

            ViewData["ServiceSectionId"] = new SelectList(_context.ServiceSection, "Id", "MainTitle", serviceItem.ServiceSectionId);
            return View(serviceItem);
        }

        // POST: ServiceItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IconClass,Title,Description,LinkUrl,ServiceSectionId")] ServiceItem serviceItem)
        {
            if (id != serviceItem.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceItemExists(serviceItem.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceSectionId"] = new SelectList(_context.ServiceSection, "Id", "MainTitle", serviceItem.ServiceSectionId);
            return View(serviceItem);
        }

        // GET: ServiceItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var serviceItem = await _context.ServiceItem
                .Include(si => si.ServiceSection)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (serviceItem == null) return NotFound();

            return View(serviceItem);
        }

        // POST: ServiceItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceItem = await _context.ServiceItem.FindAsync(id);
            if (serviceItem != null)
            {
                _context.ServiceItem.Remove(serviceItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceItemExists(int id)
        {
            return _context.ServiceItem.Any(e => e.Id == id);
        }
    }
}
