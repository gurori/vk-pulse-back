namespace Core.Entities
{
    public sealed class PositionEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Salary { get; set; }
    }
}
