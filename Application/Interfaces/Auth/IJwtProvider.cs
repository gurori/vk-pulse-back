using Microsoft.IdentityModel.Tokens;

namespace Application.Interfaces.Auth
{
    public interface IJwtProvider
    {
        public Task<string> GenerateTokenAsync(string id, string role);
        public Task<TokenValidationResult> ValidateTokenAsync(string token);
    }
}
