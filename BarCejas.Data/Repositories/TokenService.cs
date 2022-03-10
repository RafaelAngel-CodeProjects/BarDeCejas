using BarCejas.Data.Interfaces;
using BarCejas.Entities;
using BarCejas.Entities.Enumerations;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BarCejas.Data.Repositories
{
    public class TokenService : ITokenService
    {
        private const double EXPIRY_DURATION = 90;
        public string BuildToken(string key,
        string issuer, Usuario user)
        {
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, string.IsNullOrEmpty(user.Nombre)? "" : user.Nombre),
                new Claim(ClaimTypes.Surname, string.IsNullOrEmpty(user.Apellido)? "" : user.Apellido),
                new Claim(ClaimTypes.Role, user.IdTipoUsuario == (int)RoleType.Administrador ? RoleType.Administrador.ToString() : user.IdTipoUsuario == (int)RoleType.Profesional ? RoleType.Profesional.ToString() : RoleType.Cliente.ToString()),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: DateTime.Now.AddMinutes(EXPIRY_DURATION), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public bool IsTokenValid(string key, string issuer, string token)
        {
            var mySecret = Encoding.UTF8.GetBytes(key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
