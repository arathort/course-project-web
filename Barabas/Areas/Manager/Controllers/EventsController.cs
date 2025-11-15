using Barabas.Models;
using Barabas.Repositories.TicketRepository;
using Barabas.Services.EventCategoryService;
using Barabas.Services.EventService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Barabas.Areas.Manager.Controllers
{
    [Authorize(Roles = "Manager,Admin")]
    [Area("Manager")]
    public class EventsController(
        IEventService eventService,
        ITicketRepository ticketRepository,
        IEventCategoryService eventCategoryService) : Controller
    {
        private readonly IEventService _eventService = eventService;
        private readonly ITicketRepository _ticketRepository = ticketRepository;
        private readonly IEventCategoryService _eventCategoryService = eventCategoryService;

        public async Task<IActionResult> Index()
        {
            return View(await _eventService.GetEventsAsync());
        }

        public IActionResult Create()
        {
            ViewBag.EventCategories = _eventCategoryService.GetEventsCategories();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Name,Description,Location,Date,Price,EventCategoryId")] Event @event,
            int TicketCount,
            IFormFile? ImageFile)
        {
            if (TicketCount <= 0)
            {
                ModelState.AddModelError("TicketCount", "Ticket count must be greater than zero.");
            }

            if (@event.Date <= DateTime.UtcNow)
            {
                ModelState.AddModelError("Date", "Event date must be in the future.");
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var ext = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("Image", "Invalid image format. Allowed: JPG, PNG, GIF.");
                }

                if (ImageFile.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("Image", "Image size must be less than 2 MB.");
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.EventCategories = _eventCategoryService.GetEventsCategories();
                return View(@event);
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/events");
                Directory.CreateDirectory(uploads);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                @event.Image = "/images/events/" + fileName;
            }

            @event.CreatedBy = 0;
            @event.Date = DateTime.SpecifyKind(@event.Date, DateTimeKind.Utc);

            await _eventService.Add(@event);

            for (int i = 1; i <= TicketCount; i++)
            {
                Ticket ticket = new()
                {
                    EventId = @event.Id,
                    SeatNumber = i,
                    IsActive = true,
                };
                await _ticketRepository.Add(ticket);
            }

            TempData["Success"] = "Event created successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var @event = await _eventService.GetEventById((int)id);
            if (@event == null)
                return NotFound();

            ViewBag.EventCategories = _eventCategoryService.GetEventsCategories();
            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id,
            [Bind("Id,Name,Description,Location,Date,Image,CreatedBy,Price,EventCategoryId")] Event @event)
        {
            if (id != @event.Id)
                return NotFound();

            if (@event.Date <= DateTime.UtcNow)
            {
                ModelState.AddModelError("Date", "Event date must be in the future.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.EventCategories = _eventCategoryService.GetEventsCategories();
                return View(@event);
            }

            @event.Date = DateTime.SpecifyKind(@event.Date, DateTimeKind.Utc);

            try
            {
                await _eventService.Update(@event);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EventExists(@event.Id))
                    return NotFound();
                else
                    throw;
            }

            TempData["Success"] = "Event updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> EventExists(int id)
        {
            return await _eventService.GetEventById(id) != null;
        }
    }
}
