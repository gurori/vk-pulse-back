namespace Core.Entities
{
    public sealed class UserEntity : BaseEntity
    {
        public string Role { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
    }
}
