using TaskManagerAPI.Models.Task;
using TaskManagerAPI.Repositories;
using Task = System.Threading.Tasks.Task;

namespace TaskManagerAPI.Services
{
    /// <summary>
    /// Service for managing tasks.
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IServiceProvider _serviceProvider;

        public TaskService(ITaskRepository taskRepository, IServiceProvider serviceProvider)
        {
            _taskRepository = taskRepository;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="task">Task object to create.</param>
        /// <returns>The created task or null if creation failed.</returns>
        /// <exception cref="ArgumentException">Thrown if the task data is invalid.</exception>
        public async Task<Models.Task.Task?> Create(TaskCreateDTO taskCreateDTO)
        {
            if (taskCreateDTO == null)
                throw new ArgumentException("Invalid task data.");

            var userService = _serviceProvider.GetRequiredService<IUserService>();
            var existingUser = await userService.GetUserById(taskCreateDTO.UserId);

            if (existingUser == null)
                throw new KeyNotFoundException("User not found.");

            var task = new Models.Task.Task
            {
                Title = taskCreateDTO.Title,
                Description = taskCreateDTO.Description,
                Priority = taskCreateDTO.Priority,
                DueDate = taskCreateDTO.DueDate,
                UserId = taskCreateDTO.UserId
            };

            var taskCreate = await _taskRepository.Create(task);

            if (taskCreate == null)
                throw new ArgumentException("Task creation failed.");

            return taskCreate;
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="task">Task object with updated data.</param>
        /// <returns>The updated task or null if update failed.</returns>
        /// <exception cref="ArgumentException">Thrown if the task data is invalid.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the task is not found.</exception>
        public async Task<Models.Task.Task?> Update(TaskUpdateDTO taskUpdateDTO)
        {
            if (taskUpdateDTO.Id <= 0)
                throw new ArgumentException("Invalid task data.");

            var userService = _serviceProvider.GetRequiredService<IUserService>();
            var existingUser = await userService.GetUserById(taskUpdateDTO.UserId);

            if (existingUser == null)
                throw new KeyNotFoundException("User not found.");

            var existingTask = await _taskRepository.GetTaskById(taskUpdateDTO.Id, taskUpdateDTO.UserId);

            if (existingTask == null)
                throw new KeyNotFoundException("Task not found.");

            existingTask.Title = taskUpdateDTO.Title;
            existingTask.Description = taskUpdateDTO.Description;
            existingTask.Priority = taskUpdateDTO.Priority;
            existingTask.DueDate = taskUpdateDTO.DueDate;
            existingTask.IsCompleted = taskUpdateDTO.IsCompleted;

            return await _taskRepository.Update(existingTask);
        }

        /// <summary>
        /// Deletes a specific task.
        /// </summary>
        /// <param name="taskId">ID of the task to delete.</param>
        /// <param name="userId">ID of the task owner.</param>
        /// <returns>The deleted task or null if deletion failed.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the task is not found.</exception>
        public async Task<Models.Task.Task?> Delete(int taskId, int userId)
        {
            var userService = _serviceProvider.GetRequiredService<IUserService>();
            var existingUser = await userService.GetUserById(userId);

            if (existingUser == null)
                throw new KeyNotFoundException("User not found.");

            var task = await _taskRepository.GetTaskById(taskId, userId);

            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            return await _taskRepository.Delete(taskId, userId);
        }

        /// <summary>
        /// Deletes all tasks by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public async Task DeleteAllTasksByUserId(int userId)
        {
            await _taskRepository.DeleteAllTasksByUserId(userId);
        }

        /// <summary>
        /// Retrieves a task by its ID.
        /// </summary>
        /// <param name="taskId">Task ID.</param>
        /// <param name="userId">ID of the task owner.</param>
        /// <returns>The found task.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the task is not found.</exception>
        public async Task<Models.Task.Task> GetTaskById(int taskId, int userId)
        {
            var task = await _taskRepository.GetTaskById(taskId, userId);

            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            return task;
        }

        /// <summary>
        /// Retrieves all tasks for a user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>A list of tasks for the user.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the user is not found.</exception>
        public async Task<IEnumerable<Models.Task.Task>> GetAllTasks(int userId)
        {
            var userService = _serviceProvider.GetRequiredService<IUserService>();
            var existingUser = await userService.GetUserById(userId);

            if (existingUser == null)
                throw new KeyNotFoundException("User not found.");

            return await _taskRepository.GetAllTasks(userId);
        }

        /// <summary>
        /// retrieves all tasks by status.
        /// <param name="userId">User ID.</param>
        /// <returns>A list of tasks for the user.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the user is not found.</exception>
        public async Task<IEnumerable<Models.Task.Task>> GetAllTasksByStatus(int userId, bool isCompleted)
        {
            var userService = _serviceProvider.GetRequiredService<IUserService>();
            var existingUser = await userService.GetUserById(userId);

            if (existingUser == null)
                throw new KeyNotFoundException("User not found.");

            return await _taskRepository.GetAllTasksByStatus(userId, isCompleted);
        }

        /// <summary>
        /// Completeds the specified task identifier.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Task not found.</exception>
        public async Task<Models.Task.Task?> Completed(int taskId, int userId)
        {
            var task = await _taskRepository.GetTaskById(taskId, userId);
            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            task.IsCompleted = true; // Only change the status to completed

            return await _taskRepository.Update(task);
        }
    }
}
