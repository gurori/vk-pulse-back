using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces.Auth;
using Application.Interfaces.Repositories;
using Core.Models;
using Core.Models.Users;
using Core.Structs;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infastructure.Auth
{
    public class JwtProvider(IOptions<JwtOptions> options, IRoleRepository roleRepository)
        : IJwtProvider
    {
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly JwtSecurityTokenHandler _tokenHandler = new();
        private readonly JwtOptions _options = options.Value;

        public async Task<string> GenerateTokenAsync(string id, string role)
        {
            List<Claim> claims = [new(CustomClaims.UserId, id)];

            HashSet<int> permissions = await _roleRepository.GetPermissionsIdsAsync(role);

            claims.Add(new(CustomClaims.Permissions, string.Join(" ", permissions)));

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddDays(_options.ExpiresDays)
            );

            return _tokenHandler.WriteToken(token);
        }

        public async Task<TokenValidationResult> ValidateTokenAsync(string token)
        {
            var tokenValidationParameters = JwtParameters.GetTokenValidationParameters(_options);

            return await _tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);
        }
    }
}
