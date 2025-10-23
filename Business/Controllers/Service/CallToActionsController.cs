using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Business.Data;
using Business.Models.Service;

namespace Business.Controllers
{
    public class CallToActionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CallToActionsController> _logger;

        public CallToActionsController(ApplicationDbContext context, ILogger<CallToActionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: CallToActions
        public async Task<IActionResult> Index()
        {
            var data = await _context.CallToAction
                .Include(c => c.ServiceSection) // Include related ServiceSection if needed
                .AsNoTracking()
                .ToListAsync();
            return View(data);
        }

        // GET: CallToActions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var callToAction = await _context.CallToAction
                .Include(c => c.ServiceSection)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (callToAction == null) return NotFound();

            return View(callToAction);
        }

        // GET: CallToActions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CallToActions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CallToAction model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                _context.CallToAction.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Call To Action created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating CallToAction");
                ModelState.AddModelError("", "Unable to save changes: " + ex.Message);
                return View(model);
            }
        }

        // GET: CallToActions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var callToAction = await _context.CallToAction.FindAsync(id);
            if (callToAction == null) return NotFound();

            return View(callToAction);
        }

        // POST: CallToActions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CallToAction model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid) return View(model);

            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Call To Action updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CallToActionExists(model.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing CallToAction");
                ModelState.AddModelError("", "Unable to save changes: " + ex.Message);
                return View(model);
            }
        }

        // GET: CallToActions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var callToAction = await _context.CallToAction
                .Include(c => c.ServiceSection)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (callToAction == null) return NotFound();

            return View(callToAction);
        }

        // POST: CallToActions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var callToAction = await _context.CallToAction.FindAsync(id);
            if (callToAction == null) return NotFound();

            try
            {
                _context.CallToAction.Remove(callToAction);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Call To Action deleted successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting CallToAction");
                TempData["ErrorMessage"] = "Error deleting record: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CallToActionExists(int id)
        {
            return _context.CallToAction.Any(e => e.Id == id);
        }
    }
}
