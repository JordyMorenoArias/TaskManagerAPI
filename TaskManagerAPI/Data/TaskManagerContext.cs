using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Models.User;

namespace TaskManagerAPI.Data
{
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options)
        {
        }
        public DbSet<Models.Task.Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
