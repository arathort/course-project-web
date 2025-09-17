using Barabas.Models;

namespace Barabas.Repositories.EventCategoryRepository
{
    public interface IEventCategoryRepository
    {
        public Task<List<EventCategory>> GetAll();
        public Task<EventCategory> GetByID(int id);
        public void Add(EventCategory e);
        public void Update(EventCategory e);
        public void Delete(int id);

    }
}
