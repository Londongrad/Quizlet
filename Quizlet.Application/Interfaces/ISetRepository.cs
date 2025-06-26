using Quizlet.Domain.Entities;

namespace Quizlet.Application.Interfaces
{
    public interface ISetRepository
    {
        Task<Set?> GetByIdAsync(Guid id, Guid userId);
        Task<IEnumerable<Set>> GetAllByUserAsync(Guid userId);
        Task AddAsync(Set set);
        Task UpdateAsync(Set set);
        Task DeleteAsync(Guid id, Guid userId);
        Task<bool> ExistsAsync(Guid id, Guid userId);
    }
}
