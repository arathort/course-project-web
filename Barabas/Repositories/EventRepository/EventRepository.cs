using Barabas.Data;
using Barabas.Models;
using Microsoft.EntityFrameworkCore;

namespace Barabas.Repositories.EventRepository
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Event e)
        {
            _context.Events.Add(e);
            await _context.SaveChangesAsync(); 
        }

        public async Task Delete(int id)
        {
            var eventToDelete = await GetByID(id);
            if (eventToDelete != null)
            {
                _context.Events.Remove(eventToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Event>> GetAll()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<Event> GetByID(int id)
        {
            return await _context.Events.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Update(Event e)
        {
            _context.Events.Update(e);
            await _context.SaveChangesAsync(); 
        }
    }
}
