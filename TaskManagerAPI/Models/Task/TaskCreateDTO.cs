using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskManagerMaui.Constants;

namespace TaskManagerAPI.Models.Task
{
    public class TaskCreateDTO
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required(ErrorMessage = "Priority is required")]
        public Priority Priority { get; set; }
        [Required(ErrorMessage = "DueDate is required")]
        public DateTime DueDate { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
