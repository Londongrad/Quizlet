namespace Quizlet.Domain.Entities
{
    public class User(Guid id, string username, string email) : EntityBase(id)
    {
        // Parameterless constructor for EF Core
        private User() : this(Guid.Empty, string.Empty, string.Empty) { }

        private readonly List<Set> _sets = [];
        public string UserName { get; private set; } = username;
        public string Email { get; private set; } = email;
        public IReadOnlyCollection<Set> Sets => _sets.AsReadOnly();
    }
}
