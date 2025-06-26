using Microsoft.AspNetCore.Mvc;
using Quizlet.Api.DTOs.Auth;
using Quizlet.Application.Interfaces;
using Quizlet.Domain.Entities;

namespace Quizlet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenGenerator _tokenGenerator;
        private readonly IUserRepository _userRepository;

        public AuthController(IJwtTokenGenerator tokenGenerator, IUserRepository userRepository)
        {
            _tokenGenerator = tokenGenerator;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized("Invalid username or password.");

            var token = _tokenGenerator.GenerateToken(user.Id, user.UserName);

            return Ok(new AuthResponse(token, user.Id, user.UserName));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (await _userRepository.ExistsAsync(request.Username))
                return BadRequest("Username already taken.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new User(Guid.NewGuid(), request.Username, request.Email, passwordHash);

            await _userRepository.AddAsync(user);

            var token = _tokenGenerator.GenerateToken(user.Id, user.UserName);

            return Ok(new AuthResponse(token, user.Id, user.UserName));
        }
    }
}
