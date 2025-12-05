namespace Core.Models.Teams
{
    public sealed class TeamDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        // Если успеем
        public string? AdminId = null;
    }
}
