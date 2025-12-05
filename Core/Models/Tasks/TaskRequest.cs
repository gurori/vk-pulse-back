namespace Core.Models.Tasks
{
    public sealed class TaskRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Score { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ReceiverId { get; set; } = string.Empty;
    }
}
