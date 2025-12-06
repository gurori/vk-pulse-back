// Application/Interfaces/Auth/IJwtProvider.cs
using Core.Structs;
using System.Threading.Tasks;

namespace Application.Interfaces.Auth
{
    public interface IJwtProvider
    {
        public Task<string> GenerateTokenAsync(string userId, string role);
        public Task<TokenValidationResult> ValidateTokenAsync(string token);
        public string GetUserIdFromToken(string token); // Для удобства, если не нужна полная валидация
    }
}
