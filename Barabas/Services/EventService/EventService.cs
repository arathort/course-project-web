using Barabas.Models;
using Barabas.Repositories.EventRepository;
using Microsoft.EntityFrameworkCore;

namespace Barabas.Services.EventService
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _repository;

        public EventService(IEventRepository repository)
        {
            _repository = repository;
        }

        public async Task Add(Event entity)
        {
            await _repository.Add(entity);
        }


        public Task<Event> GetEventById(int id)
        {
            return _repository.GetByID(id);
        }

        public async Task<List<Event>> GetEventsAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<IEnumerable<Event>> GetEventsByCategoryAsync(int categoryId)
        {
            var list = await _repository.GetAll(); 
            return list.Where(item => item.EventCategoryId == categoryId).ToList();
        }

        public async Task Remove(Event entity)
        {
            await _repository.Delete(entity.Id);
        }

        public async Task Update(Event entity)
        {
             await _repository.Update(entity);
        }
    }
}
