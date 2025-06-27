namespace Quizlet.Api.DTOs.Auth
{
    public record AuthResponse(string Token, Guid UserId, string Username);
}
