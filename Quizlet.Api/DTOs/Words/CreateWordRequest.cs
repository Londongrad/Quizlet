namespace Quizlet.Api.DTOs.Words
{
    public record CreateWordRequest(
        Guid SetId,
        string Name,
        string Definition,
        bool IsFavorite = false,
        bool IsLastWord = false
    );
}
