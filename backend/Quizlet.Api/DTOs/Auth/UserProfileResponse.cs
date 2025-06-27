namespace Quizlet.Api.DTOs.Auth
{
    public record UserProfileResponse(
        Guid Id,
        string UserName,
        string Email,
        string? ImageUrl
    );
}
