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
                var errorResponse = new ErrorResponse { Errors = [ex.Message] };
                return BadRequest(errorResponse);
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
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
                var errorResponse = new ErrorResponse { Errors = [ex.Message] };
                return BadRequest(errorResponse);
            }
        }
    }
}
