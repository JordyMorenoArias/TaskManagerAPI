using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;

namespace TaskManagerAPI.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagerContext context;

        public TaskRepository(TaskManagerContext context)
        {
            this.context = context;
        }

        public async Task<Models.Task?> GetTaskById(int taskId)
        {
            return await context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task<IEnumerable<Models.Task>> GetAllTasks(int userId)
        {
            return await context.Tasks.Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task<Models.Task?> Create(Models.Task task)
        {
            var result = await context.Tasks.AddAsync(task);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Models.Task?> Update(Models.Task task)
        {
            var result = context.Tasks.Update(task);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Models.Task?> Delete(int taskId, int userId)
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
    }
}
