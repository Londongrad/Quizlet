namespace Quizlet.Domain.Entities
{
    public class EntityBase(Guid id)
    {
        public Guid Id { get; private set; } = id;
        public DateTime CreatedAt { get; private set; } = DateTime.Now; 
    }
}
