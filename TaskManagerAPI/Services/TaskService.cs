using TaskManagerAPI.Repositories;

namespace TaskManagerAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        public async Task<Models.Task?> Create(Models.Task task)
        {
            if (task == null || string.IsNullOrWhiteSpace(task.Title))
                throw new ArgumentException("Invalid task data.");

            return await taskRepository.Create(task);
        }

        public async Task<Models.Task?> Delete(int taskId, int userId)
        {
            return await taskRepository.Delete(taskId, userId);
        }

        public async Task<Models.Task?> Update(Models.Task task)
        {
            if (task == null || task.Id <= 0)
                throw new ArgumentException("Invalid task data.");

            return await taskRepository.Update(task);
        }

        public async Task<IEnumerable<Models.Task>> GetAllTasks(int userId)
        {
            return await taskRepository.GetAllTasks(userId);
        }
    }
}
