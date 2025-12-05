using System;

namespace Core.Models.Teams
{
    public class TeamResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? AdminId { get; set; } = null;
    }
}
