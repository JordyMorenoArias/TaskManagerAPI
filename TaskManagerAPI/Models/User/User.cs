using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models.User
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required.")]
        [PasswordPropertyText]
        public string Password { get; set; } = string.Empty;
    }
}
