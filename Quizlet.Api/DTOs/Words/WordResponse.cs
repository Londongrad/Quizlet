namespace Quizlet.Api.DTOs.Words
{
    public record WordResponse(
        Guid Id,
        Guid SetId,
        string Name,
        string Definition,
        bool IsFavorite,
        bool IsLastWord,
        DateTime CreatedAt
    );
}
