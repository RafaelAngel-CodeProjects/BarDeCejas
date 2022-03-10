using BarCejas.Core.Entities;
using BarCejas.Core.Interfaces;
using BarCejas.Infrastructure.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;

        public TokenController(IConfiguration configuration, IUserService userService, IPasswordHasher passwordHasher)
        {
            _configuration = configuration;
            _userService = userService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost]
        public async Task<IActionResult> Authentication(UserLogin login)
        {
            var validation = await IsValidUser(login);
            if (validation.Item1)
            {
                return Ok(new { token = GenerateToken(validation.Item2) });
            }
            return NotFound();
        }

        private async Task<(bool, User)> IsValidUser(UserLogin login)
        {
            try
            {
                var user = await _userService.GetLoginByCredencials(login);
                if (user != null)
                    return (_passwordHasher.Check(user.Password, login.Password), user);
                else
                    throw new Exception("Usuario no encontrado.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GenerateToken(User security)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authenticarion:SecretKey"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, security.Email),
                new Claim("User", security.FirstName),
                new Claim(ClaimTypes.Role, security.Role.ToString())
            };

            var payload = new JwtPayload(_configuration["Authenticarion:Issuer"], _configuration["Authenticarion:Audience"], claims, DateTime.Now, DateTime.UtcNow.AddMinutes(10));

            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(header, payload));
        }
    }
}
