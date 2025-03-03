using System.ComponentModel.DataAnnotations;
using TaskManagerMaui.Constants;

namespace TaskManagerAPI.Models.Task
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public Priority Priority { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; } = false;
        public int UserId { get; set; }
    }
}
