using Barabas.Models;

namespace Barabas.Repositories.EventRepository
{
    public interface IEventRepository
    {
        public Task<List<Event>> GetAll();
        public Task<Event> GetByID(int id);
        public Task Add(Event e);
        public Task Update(Event e);
        public Task Delete(int id);
    }
}
