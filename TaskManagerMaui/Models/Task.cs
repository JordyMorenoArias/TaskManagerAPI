using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerMaui.Constants;

namespace TaskManagerMaui.Models
{
    class Task
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public Priority Priority { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}
