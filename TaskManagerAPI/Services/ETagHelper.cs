using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace TaskManagerAPI.Services
{
    public class ETagHelper : IETagHelper
    {
        public string GenerateETag(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);

            using var sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));

            return $"\"{Convert.ToBase64String(hash)}\"";
        }
    }
}
