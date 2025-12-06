// Core/Models/Users/UserResponse.cs
using Core.Models.Tasks;
using System.Collections.Generic;

namespace Core.Models.Users
{
    public sealed class UserResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int Score { get; set; }
        public string? TeamName { get; set; }
        public string? PositionName { get; set; }
        public ICollection<TaskDto> InProcessTasks { get; set; } = new List<TaskDto>();
        public ICollection<TaskDto> CompletedTasks { get; set; } = new List<TaskDto>();
    }
}
