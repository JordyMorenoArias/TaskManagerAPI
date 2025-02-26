namespace TaskManagerAPI.Services
{
    public interface ITaskService
    {
        Task<Models.Task?> Create(Models.Task task);
        Task<Models.Task?> Delete(int taskId, int userId);
        Task<IEnumerable<Models.Task>> GetAllTasks(int userId);
        Task<Models.Task> GetTaskById(int taskId, int userId);
        Task<Models.Task?> Update(Models.Task task);
    }
}