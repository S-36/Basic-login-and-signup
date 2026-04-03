using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Login_and_Signup.JWT
{
    // JwtSettings 
    public class JwtSettings
    {
        public string SecretKey {get; set;} = string.Empty;
        public string Issuer {get; set;} = string.Empty;
        public string Audience {get; set;} = string.Empty;
        
        public int ExpirationHours {get; set;} = 1;

    }
    // Interface de contrato para Crear el JWT
    public interface IJwtService
    {
        string GenerateToken(string userID, string userEmail, List<string> roles);
    }

    // Constructor del servicio 
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _settings;

        public JwtService(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateToken(string userId, string userEmail, List<string> roles)
        {
            // Se cambian los claims a List Para agregar los roles por separado
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, userEmail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            // Agregar los roles como claims separados 
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Ahora los using están arriba y el código queda limpio
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_settings.SecretKey)
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_settings.ExpirationHours),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}