// Infrastructure/Auth/JwtProvider.cs
using Application.Interfaces.Auth;
using Core.Configuration;
using Core.Structs;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq; // Добавлен для .FirstOrDefault()

namespace Infrastructure.Auth
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _options;
        private readonly SymmetricSecurityKey _signingKey;

        public JwtProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
            _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
        }

        public Task<string> GenerateTokenAsync(string userId, string role)
        {
            var claims = new[]
            {
                new Claim(Core.Structs.CustomClaims.UserId, userId),
                new Claim(Core.Structs.CustomClaims.Role, role),
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_options.ExpirationMinutes)),
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult(tokenHandler.WriteToken(token));
        }

        public async Task<TokenValidationResult> ValidateTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _options.Issuer,
                ValidateAudience = true,
                ValidAudience = _options.Audience,
                ValidateLifetime = true,
                IssuerSigningKey = _signingKey,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return new TokenValidationResult { IsValid = true, Claims = principal.Claims.ToDictionary(c => c.Type, c => (object)c.Value) };
            }
            catch (Exception ex)
            {
                return new TokenValidationResult { IsValid = false, ErrorMessage = ex.Message };
            }
        }

        public string GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var jwtToken = tokenHandler.ReadJwtToken(token);
                return jwtToken.Claims.FirstOrDefault(c => c.Type == CustomClaims.UserId)?.Value ??
                       jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value ?? string.Empty;
            }
            catch (Exception)
            {
                return string.Empty; // Возвращаем пустую строку или выбрасываем исключение, если токен невалиден
            }
        }
    }
}
