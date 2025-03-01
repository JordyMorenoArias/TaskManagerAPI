namespace TaskManagerAPI.Repositories
{
    public interface ITaskRepository
    {
        Task<Models.Task.Task?> Create(Models.Task.Task task);
        Task<Models.Task.Task?> Delete(int taskId, int userId);
        Task<IEnumerable<Models.Task.Task>> GetAllTasks(int userId);
        Task<Models.Task.Task?> GetTaskById(int taskId, int userId);
        Task<Models.Task.Task?> Update(Models.Task.Task task);
    }
}