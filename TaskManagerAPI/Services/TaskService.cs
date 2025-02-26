using TaskManagerAPI.Repositories;

namespace TaskManagerAPI.Services
{
    /// <summary>
    /// Service for managing tasks.
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository taskRepository;
        private readonly IUserRepository userRepository;

        public TaskService(ITaskRepository taskRepository, IUserRepository userRepository)
        {
            this.taskRepository = taskRepository;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="task">Task object to create.</param>
        /// <returns>The created task or null if creation failed.</returns>
        /// <exception cref="ArgumentException">Thrown if the task data is invalid.</exception>
        public async Task<Models.Task?> Create(Models.Task task)
        {
            if (task == null || string.IsNullOrWhiteSpace(task.Title))
                throw new ArgumentException("Invalid task data.");

            return await taskRepository.Create(task);
        }

        /// <summary>
        /// Deletes a specific task.
        /// </summary>
        /// <param name="taskId">ID of the task to delete.</param>
        /// <param name="userId">ID of the task owner.</param>
        /// <returns>The deleted task or null if deletion failed.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the task is not found.</exception>
        public async Task<Models.Task?> Delete(int taskId, int userId)
        {
            var task = await taskRepository.GetTaskById(taskId, userId);
            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            return await taskRepository.Delete(taskId, userId);
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="task">Task object with updated data.</param>
        /// <returns>The updated task or null if update failed.</returns>
        /// <exception cref="ArgumentException">Thrown if the task data is invalid.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the task is not found.</exception>
        public async Task<Models.Task?> Update(Models.Task task)
        {
            if (task == null || task.Id <= 0)
                throw new ArgumentException("Invalid task data.");

            var existingTask = await taskRepository.GetTaskById(task.Id, task.UserId);
            if (existingTask == null)
                throw new KeyNotFoundException("Task not found.");

            return await taskRepository.Update(task);
        }

        /// <summary>
        /// Retrieves all tasks for a user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>A list of tasks for the user.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the user is not found.</exception>
        public async Task<IEnumerable<Models.Task>> GetAllTasks(int userId)
        {
            var existingUser = await userRepository.GetUserById(userId);
            if (existingUser == null)
                throw new KeyNotFoundException("User not found.");

            return await taskRepository.GetAllTasks(userId);
        }

        /// <summary>
        /// Retrieves a task by its ID.
        /// </summary>
        /// <param name="taskId">Task ID.</param>
        /// <param name="userId">ID of the task owner.</param>
        /// <returns>The found task.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the task is not found.</exception>
        public async Task<Models.Task> GetTaskById(int taskId, int userId)
        {
            var task = await taskRepository.GetTaskById(taskId, userId);
            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            return task;
        }
    }
}
