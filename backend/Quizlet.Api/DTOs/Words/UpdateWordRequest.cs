namespace Quizlet.Api.DTOs.Words
{
    public record UpdateWordRequest(
        string Name,
        string Definition,
        bool IsFavorite,
        bool IsLastWord
    );
}
