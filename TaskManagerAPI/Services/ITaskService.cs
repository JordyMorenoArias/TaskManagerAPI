using TaskManagerAPI.Models.Task;

namespace TaskManagerAPI.Services
{
    public interface ITaskService
    {
        Task<Models.Task.Task?> Completed(int taskId, int userId);
        Task<Models.Task.Task?> Create(TaskCreateDTO taskCreateDTO);
        Task<Models.Task.Task?> Delete(int taskId, int userId);
        Task<IEnumerable<Models.Task.Task>> GetAllTasks(int userId);
        Task<Models.Task.Task> GetTaskById(int taskId, int userId);
        Task<Models.Task.Task?> Update(TaskUpdateDTO taskUpdateDTO);
    }
}