using Barabas.Data;
using Barabas.Models;
using Microsoft.EntityFrameworkCore;

namespace Barabas.Repositories.EventCategoryRepository
{
    public class EventCategoryRepository : IEventCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public EventCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(EventCategory e)
        {
            _context.EventCategories.Add(e);
        }

        public async void Delete(int id)
        {
            _context.EventCategories.Remove(await GetByID(id));
        }

        public async Task<List<EventCategory>> GetAll()
        {
            return await _context.EventCategories.ToListAsync();
        }

        public async Task<EventCategory> GetByID(int id)
        {
            return await _context.EventCategories.FirstOrDefaultAsync(m => m.Id == id);
        }

        public void Update(EventCategory e)
        {
            _context.EventCategories.Update(e);
        }
    }
}
