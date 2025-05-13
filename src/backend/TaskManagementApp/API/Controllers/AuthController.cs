using API.Models.Auth;
using Application.Contracts;
using Application.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var command = new RegisterCommand
                {
                    Email = request.Email,
                    Password = request.Password,
                    FirstName = request.FirstName,
                    LastName = request.LastName
                };
                var result = await _authService.RegisterAsync(command);
                var response = new AuthResponse
                {
                    Token = result.Token,
                    UserId = result.UserId,
                    Email = result.Email
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var command = new LoginCommand
                {
                    Email = request.Email,
                    Password = request.Password
                };
                var result = await _authService.LoginAsync(command);
                var response = new AuthResponse
                {
                    Token = result.Token,
                    UserId = result.UserId,
                    Email = result.Email
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
