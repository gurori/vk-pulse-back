namespace Core.Models.Users
{
    public sealed record UserRegisterRequest(
        string Name,
        string Password,
        string Email,
        string Role
    );
}
