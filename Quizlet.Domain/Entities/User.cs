namespace Quizlet.Domain.Entities
{
    public class User : EntityBase
    {
        // Parameterless constructor for EF Core
        private User() { }
        public User(Guid id, string username, string email, string? imageURL, string passwordHash) : base(id)
        {
            UserName = username;
            Email = email;
            ImageURL = imageURL;
            PasswordHash = passwordHash;
        }

        private readonly List<Set> _sets = [];
        public string UserName { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string? ImageURL { get; private set; }
        public string PasswordHash { get; private set; } = null!;
        public IReadOnlyCollection<Set> Sets => _sets.AsReadOnly();
    }
}
