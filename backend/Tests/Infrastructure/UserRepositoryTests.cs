using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Quizlet.Domain.Entities;
using Quizlet.Infrastructure.Persistence;
using Quizlet.Infrastructure.Persistence.Repositories;

namespace Tests.Infrastructure
{
    public class UserRepositoryTests
    {
        private AppDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task ExistsAsync_ShouldBeTrueIfExists()
        {
            var context = GetDbContext(nameof(ExistsAsync_ShouldBeTrueIfExists));
            var repo = new UserRepository(context);
            var user = new User(Guid.NewGuid(), "Username", "Email", "PasswordHash");

            await repo.AddAsync(user);
            var result = await repo.ExistsAsync(user.UserName);

            result.Should().BeTrue();
        }
        
        [Fact]
        public async Task GetByCredentialsAsync_ShouldReturnUserIfCredentialsMatch()
        {
            var context = GetDbContext(nameof(GetByCredentialsAsync_ShouldReturnUserIfCredentialsMatch));
            var repo = new UserRepository(context);
            var user = new User(Guid.NewGuid(), "Username", "Email", BCrypt.Net.BCrypt.HashPassword("Password"));
            await repo.AddAsync(user);
            var result = await repo.GetByCredentialsAsync(user.UserName, "Password");
            result.Should().NotBeNull();
            result!.UserName.Should().Be(user.UserName);
        }

        [Fact]
        public async Task GetByUsernameAsync_ShouldReturnUserByUsername()
        {
            var context = GetDbContext(nameof(GetByUsernameAsync_ShouldReturnUserByUsername));
            var repo = new UserRepository(context);
            var user = new User(Guid.NewGuid(), "Username", "Email", "PasswordHash");
            await repo.AddAsync(user);
            var result = await repo.GetByUsernameAsync(user.UserName);
            result.Should().NotBeNull();
            result!.UserName.Should().Be(user.UserName);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUserById()
        {
            var context = GetDbContext(nameof(GetByIdAsync_ShouldReturnUserById));
            var repo = new UserRepository(context);
            var user = new User(Guid.NewGuid(), "Username", "Email", "PasswordHash");
            await repo.AddAsync(user);
            var result = await repo.GetByIdAsync(user.Id);
            result.Should().NotBeNull();
            result!.Id.Should().Be(user.Id);
        }
    }
}
