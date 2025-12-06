// Core/Models/Tasks/TaskRequest.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Tasks
{
    public sealed class TaskRequest
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        [Range(1, int.MaxValue)]
        public int Score { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string ReceiverId { get; set; } = string.Empty;
    }
}
