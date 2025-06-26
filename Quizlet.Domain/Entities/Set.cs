namespace Quizlet.Domain.Entities
{
    public class Set(Guid id, Guid userId, string title, string description) : EntityBase(id)
    {
        // Parameterless constructor for EF Core
        private Set() : this(Guid.Empty, Guid.Empty, string.Empty, string.Empty) { }

        private readonly List<Word> _words = [];
        public Guid UserId { get; private set; } = userId;
        public string Title { get; private set; } = title;
        public string Description { get; private set; } = description;
        public IReadOnlyCollection<Word> Words => _words.AsReadOnly();
        public User User { get; private set; } = null!;
    }
}
