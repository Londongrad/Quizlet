namespace Quizlet.Domain.Entities
{
    public abstract class EntityBase
    {
        public Guid Id { get; private set; }
        public DateTime CreatedAt { get; private set; }

        // Parameterless constructor for EF Core
        protected EntityBase()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        protected EntityBase(Guid id)
        {
            Id = id;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
