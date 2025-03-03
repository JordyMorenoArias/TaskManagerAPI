using TaskManagerAPI.Models.Task;
using TaskManagerAPI.Repositories;

namespace TaskManagerAPI.Services
{
    /// <summary>
    /// Service for managing tasks.
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserService _userService;

        public TaskService(ITaskRepository taskRepository, IUserService userService)
        {
            _taskRepository = taskRepository;
            _userService = userService;
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

            var existingUser = await _userService.GetUserById(taskCreateDTO.UserId);

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
        /// Deletes a specific task.
        /// </summary>
        /// <param name="taskId">ID of the task to delete.</param>
        /// <param name="userId">ID of the task owner.</param>
        /// <returns>The deleted task or null if deletion failed.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the task is not found.</exception>
        public async Task<Models.Task.Task?> Delete(int taskId, int userId)
        {
            var existingUser = await _userService.GetUserById(userId);

            if (existingUser == null)
                throw new KeyNotFoundException("User not found.");

            var task = await _taskRepository.GetTaskById(taskId, userId);

            if (task == null)
                throw new KeyNotFoundException("Task not found.");

            return await _taskRepository.Delete(taskId, userId);
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

            var user = await _userService.GetUserById(taskUpdateDTO.UserId);

            if (user == null)
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
        /// Retrieves all tasks for a user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>A list of tasks for the user.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the user is not found.</exception>
        public async Task<IEnumerable<Models.Task.Task>> GetAllTasks(int userId)
        {
            var existingUser = await _userService.GetUserById(userId);

            if (existingUser == null)
                throw new KeyNotFoundException("User not found.");

            return await _taskRepository.GetAllTasks(userId);
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
