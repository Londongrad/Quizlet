using Quizlet.Domain.Entities;

namespace Quizlet.Application.Interfaces
{
    public interface IWordRepository
    {
        Task<Word?> GetByIdAsync(Guid id, Guid userId);
        Task<IEnumerable<Word>> GetWordsBySetIdAsync(Guid setId, Guid userId, int skip, int take);
        Task<int> CountWordsInSetAsync(Guid setId, Guid userId);
        Task AddAsync(Word word);
        Task UpdateAsync(Word word);
        Task DeleteAsync(Guid id, Guid userId);
        Task<IEnumerable<Word>> GetFavoriteWordsAsync(Guid setId, Guid userId);
        Task<IEnumerable<Word>> SearchWordsAsync(Guid setId, Guid userId, string query);
    }
}
