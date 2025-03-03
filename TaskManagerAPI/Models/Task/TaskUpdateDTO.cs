using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskManagerMaui.Constants;

namespace TaskManagerAPI.Models.Task
{
    public class TaskUpdateDTO
    {
        [Required(ErrorMessage = "Task ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Task ID must be greater than zero")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required(ErrorMessage = "Priority is required")]
        public Priority Priority { get; set; }
        [Required(ErrorMessage = "DueDate is required")]
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
