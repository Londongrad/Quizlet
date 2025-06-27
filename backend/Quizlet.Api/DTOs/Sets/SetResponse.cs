namespace Quizlet.Api.DTOs.Sets
{
    public record SetResponse(
        Guid Id,
        string Title,
        string? Description,
        DateTime CreatedAt,
        int WordCount
    );
}
