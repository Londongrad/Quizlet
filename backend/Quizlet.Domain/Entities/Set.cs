using System.Text.Json.Serialization;

namespace Quizlet.Domain.Entities
{
    public class Set : EntityBase
    {
        private readonly List<Word> _words = [];
        public Guid UserId { get; private set; }
        public string Title { get; private set; } = null!;
        public string? Description { get; private set; }
        public IReadOnlyCollection<Word> Words => _words.AsReadOnly();

        [JsonIgnore]
        public User User { get; private set; } = null!;

        public Set(Guid id, Guid userId, string title, string? description = null) : base(id)
        {
            UserId = userId;
            Title = title;
            Description = description;
        }
        // Parameterless constructor for EF Core
        private Set() { }
    }
}
