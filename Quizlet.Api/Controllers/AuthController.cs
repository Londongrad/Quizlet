using Microsoft.AspNetCore.Mvc;
using Quizlet.Application.Interfaces;

namespace Quizlet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenGenerator _tokenGenerator;

        public AuthController(IJwtTokenGenerator tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Тут будет реальная проверка логина (пока — фейк)
            if (request.Username != "admin" || request.Password != "1234")
                return Unauthorized();

            var userId = Guid.NewGuid(); // в реальности ты получаешь это из базы
            var token = _tokenGenerator.GenerateToken(userId, request.Username);

            return Ok(new { token });
        }
    }
    public record LoginRequest(string Username, string Password);
}
