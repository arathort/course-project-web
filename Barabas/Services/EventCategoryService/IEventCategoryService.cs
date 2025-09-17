using Barabas.Models;

namespace Barabas.Services.EventCategoryService
{
    public interface IEventCategoryService
    {
        public List<EventCategory> GetEventsCategories();
        public EventCategory GetByID(int id);
        public void Add(EventCategory e);
        public void Update(EventCategory e);
        public void Delete(int id);
    }
}
