namespace Core.Entities
{
    public sealed class TaskEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public int Score { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public string CreatorId { get; set; } = null!;
        public UserEntity Creator { get; set; } = null!;
        public string? ReceiverId { get; set; }
        public UserEntity? Receiver { get; set; }
    }
}
