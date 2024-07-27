using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        public string CreateToken(AppUser user)
        {
            // key
            var tokenKey = config["TokenKey"] ?? throw new Exception("Access token key is not configured");
            if (tokenKey.Length < 64) throw new Exception("Your token key needs to be longer");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            // Claims
            var claims = new List<Claim> 
            {
                new(ClaimTypes.NameIdentifier, user.UserName)
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature); // Signing credentials
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler(); // Create Token Handler
            var token = tokenHandler.CreateToken(tokenDescriptor); // Create token with tokenDescriptor

            return tokenHandler.WriteToken(token);
        }
    }
}
