using Microsoft.EntityFrameworkCore;
using Quizlet.Application.Interfaces;
using Quizlet.Domain.Entities;

namespace Quizlet.Infrastructure.Persistence.Repositories
{
    public class WordRepository(AppDbContext context) : IWordRepository
    {
        private readonly AppDbContext _context = context;

        private IQueryable<Word> GetUserWordsInSet(Guid setId, Guid userId)
        {
            return _context.Words.Where(w => w.SetId == setId && w.Set!.UserId == userId);
        }

        public async Task<Word?> GetByIdAsync(Guid id, Guid userId)
        {
            return await _context.Words
                .Include(w => w.Set)
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id && w.Set!.UserId == userId);
        }

        public async Task<IEnumerable<Word>> GetWordsBySetIdAsync(Guid setId, Guid userId, int skip, int take)
        {
            return await GetUserWordsInSet(setId, userId)
                .OrderBy(w => w.Name)
                .Skip(skip)
                .Take(take)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> CountWordsInSetAsync(Guid setId, Guid userId)
        {
            return await GetUserWordsInSet(setId, userId)
                .CountAsync();
        }

        public async Task AddAsync(Word word)
        {
            await _context.Words.AddAsync(word);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Word word)
        {
            await _context.Words
                .Where(w => w.Id == word.Id && w.Set.UserId == word.Set.UserId)
                .ExecuteUpdateAsync(sp => sp
                    .SetProperty(w => w.Name, word.Name)
                    .SetProperty(w => w.Definition, word.Definition)
                    .SetProperty(w => w.IsFavorite, word.IsFavorite)
                    .SetProperty(w => w.IsLastWord, word.IsLastWord)
                    .SetProperty(w => w.ImageURL, word.ImageURL)
                );
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            await _context.Words
                .Where(w => w.Id == id && w.Set.UserId == userId)
                .ExecuteDeleteAsync();
        }

        public async Task<IEnumerable<Word>> GetFavoriteWordsAsync(Guid setId, Guid userId)
        {
            return await GetUserWordsInSet(setId, userId)
                .Where(w => w.IsFavorite)
                .OrderByDescending(w => w.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Word>> SearchWordsAsync(Guid setId, Guid userId, string query)
        {
            return await GetUserWordsInSet(setId, userId)
                .Where(w =>
                    (w.Name.Contains(query) || w.Definition.Contains(query))
                )
                .OrderBy(w => w.Name)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
