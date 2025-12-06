// Core/Models/Users/UserDto.cs
namespace Core.Models.Users
{
    public sealed class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int Score { get; set; }
        public string? TeamName { get; set; }
        public string? PositionName { get; set; }
    }
}
