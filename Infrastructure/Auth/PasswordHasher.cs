using Application.Interfaces.Auth;
using static BCrypt.Net.BCrypt;

namespace Infrastructure.Auth
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Generate(string password)
        {
            return EnhancedHashPassword(password);
        }

        public bool Verify(string password, string hashedPassword)
        {
            return EnhancedVerify(password, hashedPassword);
        }
    }
}
