using Barabas.Data;
using Barabas.Models;
using Barabas.Services.EventCategoryService;
using Barabas.Services.EventService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Barabas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEventService _eventService;
        private readonly IEventCategoryService _eventCategoryService;

        public HomeController(ILogger<HomeController> logger, IEventService eventService, IEventCategoryService eventCategoryService)
        {
            _logger = logger;
            _eventService = eventService;
            _eventCategoryService = eventCategoryService;
        }

        public async Task<IActionResult> Index(
            int? categoryId,
            DateTime? dateFrom,
            DateTime? dateTo,
            float? minPrice,
            float? maxPrice,
            string searchQuery,
            string sortBy = "date_asc" 
        )
        {
            IEnumerable<Event> events;

            IEnumerable<EventCategory> categories = _eventCategoryService.GetEventsCategories();
            ViewBag.Categories = categories;
            ViewBag.SortBy = sortBy;
            events = await _eventService.GetEventsAsync();

            if (categoryId.HasValue) events = events.Where(e => e.EventCategoryId == categoryId.Value);
            if (dateFrom.HasValue) events = events.Where(e => e.Date >= dateFrom.Value);
            if (dateTo.HasValue) events = events.Where(e => e.Date <= dateTo.Value);
            if (minPrice.HasValue) events = events.Where(e => e.Price >= minPrice.Value);
            if (maxPrice.HasValue) events = events.Where(e => e.Price <= maxPrice.Value);
            if (!string.IsNullOrEmpty(searchQuery))
                events = events.Where(e => e.Name.Contains(searchQuery));
            events = sortBy switch
            {
                "date_desc" => events.OrderByDescending(e => e.Date),
                "price_asc" => events.OrderBy(e => e.Price),
                "price_desc" => events.OrderByDescending(e => e.Price),
                _ => events.OrderBy(e => e.Date) 
            };
            return View(events);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
