using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;

namespace TaskManagerAPI.Repositories
{
    /// <summary>
    /// Repository for task management in the Database.
    /// </summary>
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagerContext context;

        public TaskRepository(TaskManagerContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Creates a new task in the database.
        /// </summary>
        /// <param name="task">Task object to create.</param>
        /// <returns>The created task.</returns>
        public async Task<Models.Task.Task?> Create(Models.Task.Task task)
        {
            var result = await context.Tasks.AddAsync(task);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="task">Task object with updated information.</param>
        /// <returns>The updated task.</returns>
        public async Task<Models.Task.Task?> Update(Models.Task.Task task)
        {
            context.Entry(task).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return task;
        }

        /// <summary>
        /// Deletes a task from the database.
        /// </summary>
        /// <param name="taskId">ID of the task to delete.</param>
        /// <param name="userId">User ID associated with the task.</param>
        /// <returns>The deleted task or null if not found.</returns>
        public async Task<Models.Task.Task?> Delete(int taskId, int userId)
        {
            var task = await context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

            if (task == null)
            {
                return null;
            }

            var result = context.Tasks.Remove(task);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        /// <summary>
        /// Deletes all tasks by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public async Task DeleteAllTasksByUserId(int userId)
        {
            var tasks = await context.Tasks.Where(t => t.UserId == userId).ToListAsync();

            if (tasks.Count == 0)
            {
                context.Tasks.RemoveRange(tasks);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Retrieves a task by its ID and associated user ID.
        /// </summary>
        /// <param name="taskId">Task ID.</param>
        /// <param name="userId">User ID associated with the task.</param>
        /// <returns>The found task or null if not found.</returns>
        public async Task<Models.Task.Task?> GetTaskById(int taskId, int userId)
        {
            return await context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
        }

        /// <summary>
        /// Retrieves all tasks associated with a specific user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>A list of tasks associated with the user.</returns>
        public async Task<IEnumerable<Models.Task.Task>> GetAllTasks(int userId)
        {
            return await context.Tasks.Where(t => t.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Gets all tasks by status.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="isCompleted">if set to <c>true</c> [is completed].</param>
        /// <returns></returns>
        public async Task<IEnumerable<Models.Task.Task>> GetAllTasksByStatus(int userId, bool isCompleted)
        {
            return await context.Tasks.Where(t => t.UserId == userId && t.IsCompleted == isCompleted).ToListAsync();
        }
    }
}
