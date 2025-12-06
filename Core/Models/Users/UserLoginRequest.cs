// Core/Models/Users/UserLoginRequest.cs
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Users
{
    public sealed class UserLoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
