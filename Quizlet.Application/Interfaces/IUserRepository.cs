using Quizlet.Domain.Entities;

namespace Quizlet.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> ExistsAsync(string username);
        Task AddAsync(User user);
        Task<User?> GetByCredentialsAsync(string username, string password);
    }
}
