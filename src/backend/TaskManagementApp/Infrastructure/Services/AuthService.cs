using Application.Common;
using Application.Contracts;
using Application.Dtos.Auth;
using Infrastructure.Common.Auth;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(
            IOptions<JwtOptions> jwtOptions,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _jwtOptions = jwtOptions.Value;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<Result<AuthResult>> RegisterAsync(RegisterCommand command)
        {
            var identityUser = new ApplicationUser
            {
                Email = command.Email,
                UserName = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName
            };

            var result = await _userManager.CreateAsync(identityUser, command.Password);

            if (!result.Succeeded)
            {
                throw new Exception("Registration failed");
            }

            return await GenerateAuthResult(identityUser);
        }


        public async Task<Result<AuthResult>> LoginAsync(LoginCommand command)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Email == command.Email);
            if (user == null)
            {
                return Result<AuthResult>.Failure("Invalid email or password");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, command.Password, false);

            if (!result.Succeeded)
            {
                Result<AuthResult>.Failure("Invalid email or password");
            }

            return await GenerateAuthResult(user);
        }

        private async Task<Result<AuthResult>> GenerateAuthResult(ApplicationUser identityUser)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, identityUser.Id),
                new Claim(ClaimTypes.Email, identityUser.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddHours(_jwtOptions.Expiry);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: expiry,
                signingCredentials: credentials);

            return Result<AuthResult>.Success(new AuthResult
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserId = identityUser.Id,
                Email = identityUser.Email
            });
        }
    }
}
