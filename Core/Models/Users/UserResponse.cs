namespace Core.Models.Users
{
    public sealed record UserResponse(string Id, string Name, string Email, int Score);
}
