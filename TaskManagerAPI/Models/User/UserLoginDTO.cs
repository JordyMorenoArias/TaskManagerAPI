using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TaskManagerAPI.Models.User
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required.")]
        [PasswordPropertyText]
        public string Password { get; set; } = string.Empty;
    }
}
