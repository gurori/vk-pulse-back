// Core/Models/Users/UserRegisterRequest.cs
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Users
{
    public sealed class UserRegisterRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(6)] // Пример минимальной длины
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = "User"; // Роль по умолчанию
    }
}
