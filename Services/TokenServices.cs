using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProjetoIntegrador.Interfaces;
using ProjetoIntegrador.Models;

namespace ProjetoIntegrador.Services
{
    public class TokenServices : ITokenServices
    {
        public string GenerateToken(Usuarios user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddDays(365),
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email.ToString()),
                    new Claim(ClaimTypes.GivenName, user.Nome.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                }),
                Issuer = "http://192.168.0.99:5203",
                Audience = "http://192.168.0.99:5203",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
