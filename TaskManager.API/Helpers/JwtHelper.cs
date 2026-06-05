using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.API.Models;

namespace TaskManager.API.Helpers
{
    public class JwtHelper
    {
        private readonly IConfiguration _configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            // Read settings from appsettings.json
            var secret = _configuration["JwtSettings:Secret"]!;
            var issuer = _configuration["JwtSettings:Issuer"]!;
            var audience = _configuration["JwtSettings:Audience"]!;
            var expiryMinutes = int.Parse(
                _configuration["JwtSettings:ExpiryMinutes"]!);

            // The signing key — built from your secret string
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            // Claims = data embedded inside the token
            // The frontend can read these without calling the server
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // Build the token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            // Serialize to string — this is what gets sent to the client
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}