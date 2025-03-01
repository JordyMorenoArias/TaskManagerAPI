using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models.User
{
    public class UserUpdateDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
    }
}
