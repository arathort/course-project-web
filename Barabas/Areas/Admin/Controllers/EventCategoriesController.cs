using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Barabas.Models;
using Barabas.Services.EventCategoryService;
using Microsoft.AspNetCore.Authorization;

namespace Barabas.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class EventCategoriesController : Controller
    {
        private readonly IEventCategoryService _eventCategoryService;

        public EventCategoriesController(IEventCategoryService eventCategoryService)
        {
            _eventCategoryService = eventCategoryService;
        }

        public IActionResult Index()
        {
            return View(_eventCategoryService.GetEventsCategories());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventCategory = _eventCategoryService.GetByID((int)id);
            if (eventCategory == null)
            {
                return NotFound();
            }

            return View(eventCategory);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/EventCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name")] EventCategory eventCategory)
        {
            if (ModelState.IsValid)
            {
                _eventCategoryService.Add(eventCategory);
                return RedirectToAction(nameof(Index));
            }
            return View(eventCategory);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventCategory = _eventCategoryService.GetByID((int)id);
            if (eventCategory == null)
            {
                return NotFound();
            }
            return View(eventCategory);
        }

        // POST: Admin/EventCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name")] EventCategory eventCategory)
        {
            if (id != eventCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _eventCategoryService.Update(eventCategory);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventCategoryExists(eventCategory.Id))
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
            return View(eventCategory);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventCategory = _eventCategoryService.GetByID((int)id);
            if (eventCategory == null)
            {
                return NotFound();
            }

            return View(eventCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {

            _eventCategoryService.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        private bool EventCategoryExists(int id)
        {
            return _eventCategoryService.GetByID((int)id)!=null;
        }
    }
}
