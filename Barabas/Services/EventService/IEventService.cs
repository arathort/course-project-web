using Barabas.Models;

namespace Barabas.Services.EventService
{
    public interface IEventService
    {
        public Task<List<Event>> GetEventsAsync();
        public Task<IEnumerable<Event>> GetEventsByCategoryAsync(int categoryId);
        public Task<Event> GetEventById(int id);
        public Task Add(Event entity);
        public Task Remove(Event entity);
        public Task Update(Event entity);
    }
}
