using Barabas.Models;
using Barabas.Repositories.TicketRepository;
using Barabas.Services.EventCategoryService;
using Barabas.Services.EventService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Barabas.Areas.Manager.Controllers
{
    [Authorize(Roles = "Manager,Admin")]
    [Area("Manager")]
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ITicketRepository _ticketRepository;
        private readonly IEventCategoryService _eventCategoryService;

        public EventsController(IEventService eventService, ITicketRepository ticketRepository, IEventCategoryService eventCategoryService)
        {
            _eventService = eventService;
            _ticketRepository = ticketRepository;
            _eventCategoryService = eventCategoryService;
        }

        // GET: Manager/Events
        public async Task<IActionResult> Index()
        {
            return View(await _eventService.GetEventsAsync());
        }

        // GET: Manager/Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _eventService.GetEventById((int)id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        public IActionResult Create()
        {
            var categories = _eventCategoryService.GetEventsCategories();
            ViewBag.EventCategories = categories;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
           [Bind("Id,Name,Description,Location,Date,Price,EventCategoryId")] Event @event,
           int TicketCount,
           IFormFile ImageFile)
        {
            if (@event.Date.Kind == DateTimeKind.Unspecified)
            {
                @event.Date = DateTime.SpecifyKind(@event.Date, DateTimeKind.Utc);
            }

            @event.CreatedBy = 0;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/events");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                @event.Image = "/images/events/" + fileName;
            }

            if (ModelState.IsValid)
            {
                await _eventService.Add(@event);

                for (int i = 1; i <= TicketCount; i++)
                {
                    Ticket ticket = new()
                    {
                        EventId = @event.Id,
                        SeatNumber = i
                    };
                    await _ticketRepository.Add(ticket);
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.EventCategories = _eventCategoryService.GetEventsCategories();
            return View(@event);
        }

        // POST: Manager/Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, [Bind("Id,Name,Description,Location,Date,Image,CreatedBy,Price,EventCategoryId")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _eventService.Update(@event);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await EventExists(@event.Id))
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
            return View(@event);
        }

        // GET: Manager/Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _eventService.GetEventById((int)id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Manager/Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _eventService.GetEventById((int)id);
            if (@event != null)
            {
                await _eventService.Remove(@event);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> EventExists(int id)
        {
            return await _eventService.GetEventById(id) != null;
        }


    }
}
