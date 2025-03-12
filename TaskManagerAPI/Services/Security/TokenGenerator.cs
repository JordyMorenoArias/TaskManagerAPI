using System.Security.Cryptography;

namespace TaskManagerAPI.Services.Security
{
    public class TokenGenerator : ITokenGenerator
    {
        public string GenerateToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }
    }
}
