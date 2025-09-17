using Barabas.Models;
using Barabas.Repositories.EventCategoryRepository;

namespace Barabas.Services.EventCategoryService
{
    public class EventCategoryService : IEventCategoryService
    {
        private readonly IEventCategoryRepository _eventCategoryRepository;

        public EventCategoryService(IEventCategoryRepository eventCategoryRepository)
        {
            _eventCategoryRepository = eventCategoryRepository;
        }

        public void Add(EventCategory e)
        {
            _eventCategoryRepository.Add(e);
        }

        public void Delete(int id)
        {
            _eventCategoryRepository.Delete(id);
        }

        public EventCategory GetByID(int id)
        {
            return _eventCategoryRepository.GetByID(id).Result;
        }

        public List<EventCategory> GetEventsCategories()
        {
            return _eventCategoryRepository.GetAll().Result;
        }

        public void Update(EventCategory e)
        {
            _eventCategoryRepository.Update(e);
        }
    }
}
