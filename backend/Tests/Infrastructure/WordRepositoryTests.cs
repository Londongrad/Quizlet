using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Quizlet.Domain.Entities;
using Quizlet.Infrastructure.Persistence;
using Quizlet.Infrastructure.Persistence.Repositories;
using System.Runtime.CompilerServices;

namespace Tests.Infrastructure
{
    public class WordRepositoryTests
    {
        private static string GetMethodName([CallerMemberName] string methodName = "") => methodName;

        private AppDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddWord()
        {
            var context = GetDbContext(GetMethodName());
            var repo = new WordRepository(context);
            var setId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var set = new Set(setId, userId, "Test Set", "Test Description");
            await context.Sets.AddAsync(set);
            await context.SaveChangesAsync();

            var word = new Word(Guid.NewGuid(), setId, "cat", "test definition", true);

            await repo.AddAsync(word);
            var fetched = await repo.GetByIdAsync(word.Id, userId);

            fetched.Should().NotBeNull();
            fetched!.Name.Should().Be("cat");
            fetched.IsFavorite.Should().BeTrue();
        }

        [Fact]
        public async Task GetWordsBySetIdAsync_ShouldReturnCorrectPortion()
        {
            var context = GetDbContext(GetMethodName());
            var repo = new WordRepository(context);
            var userId = Guid.NewGuid();
            var setId = Guid.NewGuid();

            var set = new Set(setId, userId, "Bulk Test", "Test Description");
            await context.Sets.AddAsync(set);
            await context.SaveChangesAsync();

            for (int i = 0; i < 50; i++)
            {
                var word = new Word(Guid.NewGuid(), setId, $"word{i}", "test definition");
                await context.Words.AddAsync(word);
            }

            await context.SaveChangesAsync();

            var portion = await repo.GetWordsBySetIdAsync(setId, userId, 10, 5);
            portion.Should().HaveCount(5);
        }

        [Fact]
        public async Task SearchWordsAsync_ShouldReturnMatchingWords()
        {
            var context = GetDbContext(GetMethodName());
            var repo = new WordRepository(context);
            var userId = Guid.NewGuid();
            var setId = Guid.NewGuid();

            var set = new Set(setId, userId, "Search Set", "A set for searching");
            await context.Sets.AddAsync(set);

            var words = new[]
            {
                new Word(Guid.NewGuid(), setId, "cat", "animal", true, false),
                new Word(Guid.NewGuid(), setId, "car", "vehicle", false, false),
                new Word(Guid.NewGuid(), setId, "cart", "market vehicle", false, false)
            };

            await context.Words.AddRangeAsync(words);
            await context.SaveChangesAsync();

            var results = await repo.SearchWordsAsync(setId, userId, "car");

            results.Should().HaveCount(2)
                .And.OnlyContain(w => w.Name.Contains("car") || w.Definition.Contains("car"));
        }

        [Fact]
        public async Task GetFavoriteWordsAsync_ShouldReturnOnlyFavorites()
        {
            var context = GetDbContext(GetMethodName());
            var repo = new WordRepository(context);
            var userId = Guid.NewGuid();
            var setId = Guid.NewGuid();

            var set = new Set(setId, userId, "Favorites Set", "Set for favorites");
            await context.Sets.AddAsync(set);

            var words = new[]
            {
                new Word(Guid.NewGuid(), setId, "apple", "fruit", true, false),
                new Word(Guid.NewGuid(), setId, "banana", "fruit", false, false),
                new Word(Guid.NewGuid(), setId, "cherry", "fruit", true, false)
            };

            await context.Words.AddRangeAsync(words);
            await context.SaveChangesAsync();

            var favorites = await repo.GetFavoriteWordsAsync(userId);

            favorites.Should().HaveCount(2)
                .And.AllSatisfy(w => w.IsFavorite.Should().BeTrue());
        }

        [Fact]
        public async Task UpdateAsync_ShouldChangeWordProperties()
        {
            var dbName = GetMethodName();
            var wordId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var setId = Guid.NewGuid();

            // 1. Добавляем word в базу
            using (var context = GetDbContext(dbName))
            {
                var set = new Set(setId, userId, "Update Set", "Initial def");
                var word = new Word(wordId, setId, "book", "original", false, false);
                await context.Sets.AddAsync(set);
                await context.Words.AddAsync(word);
                await context.SaveChangesAsync();
            }

            // 2. Обновляем word через новый контекст
            using (var context = GetDbContext(dbName))
            {
                var repo = new WordRepository(context);
                var updatedWord = new Word(wordId, setId, "book", "updated definition", true, false);
                await repo.UpdateAsync(updatedWord);
            }

            // 3. Проверяем результат
            using (var context = GetDbContext(dbName))
            {
                var repo = new WordRepository(context);
                var fetched = await repo.GetByIdAsync(wordId, userId);

                fetched.Should().NotBeNull();
                fetched!.Definition.Should().Be("updated definition");
                fetched.IsFavorite.Should().BeTrue();
            }
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotDeleteIfWrongUser()
        {
            var context = GetDbContext(GetMethodName());
            var repo = new WordRepository(context);
            var correctUserId = Guid.NewGuid();
            var wrongUserId = Guid.NewGuid();
            var setId = Guid.NewGuid();

            var set = new Set(setId, correctUserId, "Protected Set", "Do not touch");
            await context.Sets.AddAsync(set);

            var word = new Word(Guid.NewGuid(), setId, "secure-word", "should stay", false, false);
            await context.Words.AddAsync(word);
            await context.SaveChangesAsync();

            await repo.DeleteAsync(word.Id, wrongUserId);

            var stillThere = await repo.GetByIdAsync(word.Id, correctUserId);
            stillThere.Should().NotBeNull();
        }
    }
}
