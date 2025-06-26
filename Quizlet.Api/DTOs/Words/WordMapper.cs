using Quizlet.Domain.Entities;

namespace Quizlet.Api.DTOs.Words
{
    public static class WordMapper
    {
        public static WordResponse ToResponse(this Word word)
        {
            return new WordResponse(
                word.Id,
                word.SetId,
                word.Name,
                word.Definition,
                word.IsFavorite,
                word.IsLastWord,
                word.CreatedAt
            );
        }
    }
}
