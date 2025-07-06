using Quizlet.Domain.Entities;

namespace Quizlet.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task AddAsync(User user);
        Task<User?> GetByCredentialsAsync(string username, string password);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByIdAsync(Guid id);
    }
}
