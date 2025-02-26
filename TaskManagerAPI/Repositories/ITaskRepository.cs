using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public interface ITaskRepository
    {
        Task<Models.Task?> Create(Models.Task task);
        Task<Models.Task?> Delete(int taskId, int userId);
        Task<IEnumerable<Models.Task>> GetAllTasks(int userId);
        Task<Models.Task?> GetTaskById(int taskId);
        Task<Models.Task?> Update(Models.Task task);
    }
}