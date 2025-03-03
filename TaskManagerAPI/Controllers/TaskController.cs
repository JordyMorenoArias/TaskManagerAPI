using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Security;
using TaskManagerAPI.Models.Task;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;

        public TaskController(ITaskService taskService, IUserService userService)
        {
            _taskService = taskService;
            _userService = userService;
        }

        /// <summary>
        /// Creates a new task for the authenticated user.
        /// </summary>
        /// <param name="taskCreateDTO">Task data to create.</param>
        /// <returns>The created task or an error in case of failure.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] TaskCreateDTO taskCreateDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = _userService.GetAuthenticatedUserId(HttpContext);

                taskCreateDTO.UserId = userId!.Value;

                var result = await _taskService.Create(taskCreateDTO);

                if (result == null)
                    return BadRequest(new { message = "Task creation failed." });

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="taskUpdateDTO">Task data to update.</param>
        /// <returns>The updated task or an error in case of failure.</returns>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] TaskUpdateDTO taskUpdateDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = _userService.GetAuthenticatedUserId(HttpContext);

                taskUpdateDTO.UserId = userId!.Value;

                var result = await _taskService.Update(taskUpdateDTO);

                if (result == null)
                    return BadRequest(new { message = "Task update failed." });

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a specific task.
        /// </summary>
        /// <param name="taskId">ID of the task to delete.</param>
        /// <returns>Deletion confirmation or an error.</returns>
        [HttpDelete("delete/{taskId}")]
        public async Task<IActionResult> Delete(int taskId)
        {
            try
            {
                var userId = _userService.GetAuthenticatedUserId(HttpContext);

                var result = await _taskService.Delete(taskId, userId!.Value);

                if (result == null)
                    return BadRequest(new { message = "Task deletion failed." });

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves all tasks for the authenticated user.
        /// </summary>
        /// <returns>List of tasks or an error if not found.</returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllTask()
        {
            try
            {
                var userId = _userService.GetAuthenticatedUserId(HttpContext);

                var result = await _taskService.GetAllTasks(userId!.Value);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Marks a task as completed.
        /// </summary>
        /// <param name="taskId">ID of the task to mark as completed.</param>
        /// <returns>Confirmation of the action or an error.</returns>
        [HttpPut("completed/{taskId}")]
        public async Task<IActionResult> Completed(int taskId)
        {
            try
            {
                var userId = _userService.GetAuthenticatedUserId(HttpContext);

                var result = await _taskService.Completed(taskId, userId!.Value);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
