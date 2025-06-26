using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Quizlet.Domain.Entities;
using Quizlet.Infrastructure.Persistence;
using Quizlet.Infrastructure.Persistence.Repositories;

namespace Tests.Infrastructure
{
    public class SetRepositoryTests
    {
        private AppDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddSet()
        {
            var context = GetDbContext(nameof(AddAsync_ShouldAddSet));
            var repo = new SetRepository(context);
            var set = new Set(Guid.NewGuid(), Guid.NewGuid(), "Test Set", "Test Description");

            await repo.AddAsync(set);
            var retrieved = await repo.GetByIdAsync(set.Id, set.UserId);

            retrieved.Should().NotBeNull();
            retrieved!.Title.Should().Be("Test Set");
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveSet()
        {
            var dbName = nameof(DeleteAsync_ShouldRemoveSet);
            var userId = Guid.NewGuid();
            var setId = Guid.NewGuid();

            // 1. Добавляем Set
            using (var context = GetDbContext(dbName))
            {
                var set = new Set(setId, userId, "To Delete", "Description");
                await context.Sets.AddAsync(set);
                await context.SaveChangesAsync();
            }

            // 2. Удаляем через новый контекст
            using (var context = GetDbContext(dbName))
            {
                var repo = new SetRepository(context);
                await repo.DeleteAsync(setId, userId);
            }

            // 3. Проверяем, что удалено
            using (var context = GetDbContext(dbName))
            {
                var repo = new SetRepository(context);
                var result = await repo.GetByIdAsync(setId, userId);
                result.Should().BeNull();
            }
        }

        [Fact]
        public async Task GetAllByUserAsync_ShouldReturnOnlyUserSets()
        {
            var context = GetDbContext(nameof(GetAllByUserAsync_ShouldReturnOnlyUserSets));
            var repo = new SetRepository(context);
            var userId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();

            var sets = new[]
            {
                new Set(Guid.NewGuid(), userId, "User Set 1", "Def 1"),
                new Set(Guid.NewGuid(), userId, "User Set 2", "Def 2"),
                new Set(Guid.NewGuid(), otherUserId, "Other User Set", "Not returned")
            };

            await context.Sets.AddRangeAsync(sets);
            await context.SaveChangesAsync();

            var result = await repo.GetAllByUserAsync(userId);

            result.Should().HaveCount(2)
                .And.OnlyContain(s => s.UserId == userId);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnTrueIfSetExists()
        {
            var context = GetDbContext(nameof(ExistsAsync_ShouldReturnTrueIfSetExists));
            var repo = new SetRepository(context);
            var userId = Guid.NewGuid();
            var set = new Set(Guid.NewGuid(), userId, "Existing Set", "Defined");

            await context.Sets.AddAsync(set);
            await context.SaveChangesAsync();

            var exists = await repo.ExistsAsync(set.Id, userId);
            exists.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnFalseIfSetDoesNotExist()
        {
            var context = GetDbContext(nameof(ExistsAsync_ShouldReturnFalseIfSetDoesNotExist));
            var repo = new SetRepository(context);
            var userId = Guid.NewGuid();

            var exists = await repo.ExistsAsync(Guid.NewGuid(), userId);
            exists.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateSetTitleAndDefinition()
        {
            var dbName = nameof(UpdateAsync_ShouldUpdateSetTitleAndDefinition);
            var userId = Guid.NewGuid();
            var setId = Guid.NewGuid();

            // 1. Добавляем Set
            using (var context = GetDbContext(dbName))
            {
                var set = new Set(setId, userId, "Original Title", "Original Definition");
                await context.Sets.AddAsync(set);
                await context.SaveChangesAsync();
            }

            // 2. Обновляем через новый контекст
            using (var context = GetDbContext(dbName))
            {
                var repo = new SetRepository(context);
                var updated = new Set(setId, userId, "Updated Title", "Updated Definition");
                await repo.UpdateAsync(updated);
            }

            // 3. Проверяем
            using (var context = GetDbContext(dbName))
            {
                var repo = new SetRepository(context);
                var fetched = await repo.GetByIdAsync(setId, userId);

                fetched.Should().NotBeNull();
                fetched!.Title.Should().Be("Updated Title");
                fetched.Description.Should().Be("Updated Definition");
            }
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveOnlyUsersOwnSet()
        {
            var dbName = nameof(DeleteAsync_ShouldRemoveOnlyUsersOwnSet);
            var userId = Guid.NewGuid();
            var wrongUserId = Guid.NewGuid();
            var setId = Guid.NewGuid();

            // 1. Добавляем Set
            using (var context = GetDbContext(dbName))
            {
                var set = new Set(setId, userId, "To Be Deleted", "Owned");
                await context.Sets.AddAsync(set);
                await context.SaveChangesAsync();
            }

            // 2. Попытка удалить чужим пользователем (не должна сработать)
            using (var context = GetDbContext(dbName))
            {
                var repo = new SetRepository(context);
                await repo.DeleteAsync(setId, wrongUserId);
            }

            // 3. Убедимся, что Set всё ещё существует
            using (var context = GetDbContext(dbName))
            {
                var repo = new SetRepository(context);
                var stillThere = await repo.GetByIdAsync(setId, userId);
                stillThere.Should().NotBeNull();
            }

            // 4. Удаляем своим пользователем
            using (var context = GetDbContext(dbName))
            {
                var repo = new SetRepository(context);
                await repo.DeleteAsync(setId, userId);
            }

            // 5. Проверяем, что Set удалён
            using (var context = GetDbContext(dbName))
            {
                var repo = new SetRepository(context);
                var deleted = await repo.GetByIdAsync(setId, userId);
                deleted.Should().BeNull();
            }
        }
    }
}
