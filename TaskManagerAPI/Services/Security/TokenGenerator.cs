using System.Security.Cryptography;

namespace TaskManagerAPI.Services.Security
{
    public class TokenGenerator : ITokenGenerator
    {
        public string GenerateToken()
        {
            byte[] tokenBytes = RandomNumberGenerator.GetBytes(32);
            string token = Convert.ToBase64String(tokenBytes)
                .Replace("+", "-")  // Evita problemas en SQL y URLs
                .Replace("/", "_")  // Evita conflictos en rutas
                .TrimEnd('=');

            return token;
        }
    }
}
