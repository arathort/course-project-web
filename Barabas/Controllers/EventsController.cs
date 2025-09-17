using Barabas.Data;
using Barabas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Barabas.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        public IActionResult Create()
        {
            ViewBag.EventCategories = new SelectList(_context.EventCategories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Location,Date,Image,Price,CreatedBy,EventCategoryId")] Event @event)
        {
            if (@event.Date.Kind == DateTimeKind.Unspecified)
            {
                @event.Date = DateTime.SpecifyKind(@event.Date, DateTimeKind.Utc);
            }
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.EventCategories = new SelectList(_context.EventCategories, "Id", "Name");
            }
            return View(@event);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            ViewBag.EventCategories = new SelectList(_context.EventCategories, "Id", "Name", @event.EventCategoryId);

            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Location,Date,Image,Price,CreatedBy,EventCategoryId")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (@event.Date.Kind == DateTimeKind.Unspecified)
            {
                @event.Date = DateTime.SpecifyKind(@event.Date, DateTimeKind.Utc);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.EventCategories = new SelectList(_context.EventCategories, "Id", "Name", @event.EventCategoryId);

            return View(@event);
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.EventCategories = new SelectList(_context.EventCategories, "Id", "Name");

            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
