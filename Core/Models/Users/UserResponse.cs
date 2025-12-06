using Core.Models.Tasks;
using Core.Models.Teams;

namespace Core.Models.Users
{
    public sealed record UserResponse(
        string Id,
        string Name,
        string Email,
        int Score,
        string Role,
        string TeamId,
        TeamDto Team,
        ICollection<TaskDto> CreatedTasks,
        ICollection<TaskDto> ReceivedTasks
    );
}
