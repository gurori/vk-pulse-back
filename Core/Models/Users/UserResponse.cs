using System;

namespace Core.Models.Users
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Score { get; set; } = 0;
        public string? TeamId { get; set; } = null;
        public string? PositionId { get; set; } = null;
    }
}
