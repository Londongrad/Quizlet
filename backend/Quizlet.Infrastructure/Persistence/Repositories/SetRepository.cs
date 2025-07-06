using Microsoft.EntityFrameworkCore;
using Quizlet.Application.Interfaces;
using Quizlet.Domain.Entities;

namespace Quizlet.Infrastructure.Persistence.Repositories
{
    public class SetRepository(AppDbContext context) : ISetRepository
    {
        private readonly AppDbContext _context = context;

        private IQueryable<Set> GetUserSets(Guid userId)
        {
            return _context.Sets.Where(s => s.UserId == userId);
        }

        public async Task<Set?> GetByIdAsync(Guid id, Guid userId)
        {
            return await GetUserSets(userId)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Set>> GetAllByUserAsync(Guid userId)
        {
            return await GetUserSets(userId)
                .AsNoTracking()
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
            await GetUserSets(set.UserId)
                .Where(s => s.Id == set.Id)
                .ExecuteUpdateAsync(sp => sp
                    .SetProperty(s => s.Title, set.Title)
                    .SetProperty(s => s.Description, set.Description)
                );
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            await GetUserSets(userId)
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<bool> ExistsAsync(Guid id, Guid userId)
        {
            return await GetUserSets(userId)
                .AnyAsync(s => s.Id == id);
        }
    }
}
