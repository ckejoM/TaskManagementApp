using API.Models;
using API.Models.Auth;
using API.Models.Category;
using Application.Common;
using Application.Contracts;
using Application.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var command = new RegisterCommand
            {
                Email = request.Email,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName
            };
            var result = await _authService.RegisterAsync(command);

            if (!result.IsSuccess)
            {
                var errorResponse = new ErrorResponse { Errors = result.Errors };
                return BadRequest(errorResponse);
            }

            var response = new AuthResponse
            {
                Token = result.Value.Token,
                UserId = result.Value.UserId,
                Email = result.Value.Email
            };
            return Ok(response);
            
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var command = new LoginCommand
            {
                Email = request.Email,
                Password = request.Password
            };
            var result = await _authService.LoginAsync(command);

            if (!result.IsSuccess)
            {
                var errorResponse = new ErrorResponse { Errors = result.Errors };
                return BadRequest(errorResponse);
            }

            var response = new AuthResponse
            {
                Token = result.Value.Token,
                UserId = result.Value.UserId,
                Email = result.Value.Email
            };

            return Ok(response);           
            
        }
    }
}
