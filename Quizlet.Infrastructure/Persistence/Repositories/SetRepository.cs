using Microsoft.EntityFrameworkCore;
using Quizlet.Application.Interfaces;
using Quizlet.Domain.Entities;

namespace Quizlet.Infrastructure.Persistence.Repositories
{
    public class SetRepository : ISetRepository
    {
        private readonly AppDbContext _context;

        public SetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Set?> GetByIdAsync(Guid id, Guid userId)
        {
            return await _context.Sets
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
        }

        public async Task<IEnumerable<Set>> GetAllByUserAsync(Guid userId)
        {
            return await _context.Sets
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Set set)
        {
            await _context.Sets.AddAsync(set);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Set set)
        {
            _context.Sets.Update(set);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            var set = await GetByIdAsync(id, userId);
            if (set != null)
            {
                _context.Sets.Remove(set);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Guid id, Guid userId)
        {
            return await _context.Sets
                .AnyAsync(s => s.Id == id && s.UserId == userId);
        }
    }
}
