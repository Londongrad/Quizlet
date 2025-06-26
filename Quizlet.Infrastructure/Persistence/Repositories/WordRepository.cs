using Microsoft.EntityFrameworkCore;
using Quizlet.Application.Interfaces;
using Quizlet.Domain.Entities;

namespace Quizlet.Infrastructure.Persistence.Repositories
{
    public class WordRepository : IWordRepository
    {
        private readonly AppDbContext _context;

        public WordRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Word?> GetByIdAsync(Guid id, Guid userId)
        {
            return await _context.Words
                .Include(w => w.Set)
                .FirstOrDefaultAsync(w => w.Id == id && w.Set!.UserId == userId);
        }

        public async Task<IEnumerable<Word>> GetWordsBySetIdAsync(Guid setId, Guid userId, int skip, int take)
        {
            return await _context.Words
                .Where(w => w.SetId == setId && w.Set!.UserId == userId)
                .OrderBy(w => w.Name)
                .Skip(skip)
                .Take(take)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> CountWordsInSetAsync(Guid setId, Guid userId)
        {
            return await _context.Words
                .CountAsync(w => w.SetId == setId && w.Set!.UserId == userId);
        }

        public async Task AddAsync(Word word)
        {
            await _context.Words.AddAsync(word);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Word word)
        {
            _context.Words.Update(word);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            var word = await GetByIdAsync(id, userId);
            if (word != null)
            {
                _context.Words.Remove(word);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Word>> GetFavoriteWordsAsync(Guid userId)
        {
            return await _context.Words
                .Where(w => w.IsFavorite && w.Set!.UserId == userId)
                .OrderBy(w => w.Name)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Word>> SearchWordsAsync(Guid setId, Guid userId, string query)
        {
            return await _context.Words
                .Where(w =>
                    w.SetId == setId &&
                    w.Set!.UserId == userId &&
                    (w.Name.Contains(query) || w.Definition.Contains(query))
                )
                .OrderBy(w => w.Name)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
