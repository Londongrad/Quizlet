﻿namespace Quizlet.Domain.Entities
{
    public class Word(Guid id, Guid setId, string name, string definition, bool isFavorite = false, bool isLastWord = false, string? imagePath = null) : EntityBase(id)
    {
        // Parameterless constructor for EF Core
        private Word() : this(Guid.Empty, Guid.Empty, string.Empty, string.Empty, false, false) { }
        public Guid SetId { get; } = setId;
        public string Name { get; private set; } = name;
        public string Definition { get; private set; } = definition;
        public bool IsFavorite { get; private set; } = isFavorite;
        public bool IsLastWord { get; private set; } = isLastWord;
        public string? ImageURL { get; private set; } = imagePath;
        public Set Set { get; private set; } = null!;
    }
}
