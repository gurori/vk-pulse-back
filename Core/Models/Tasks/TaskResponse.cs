// Core/Models/Tasks/TaskResponse.cs
using Core.Models.Users;
using System;

namespace Core.Models.Tasks
{
    public sealed class TaskResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public int Score { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string? ReceiverId { get; set; }
        public UserDto? Receiver { get; set; } // Для отображения информации о получателе
    }
}
