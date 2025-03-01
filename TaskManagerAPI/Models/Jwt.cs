using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Models
{
    public class Jwt
    {
        public required string Key { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required string Subject { get; set; }

    }
}
