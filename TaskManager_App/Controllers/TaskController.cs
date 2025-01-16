using System.Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager_App.Models;
using TaskManagerApi.Data;

namespace TaskManager_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskDbContext _dbContext;

        public TaskController(TaskDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            if (_dbContext.Tasks == null)
            {
                return NotFound("No tasks found.");
            }

            try
            {
                var tasks = await Task.Run(() => _dbContext.Tasks.ToList());
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetSpecificTask(int id)
        {
            if(_dbContext.Tasks == null)
            {
                return NotFound();
            }
            var task = await _dbContext.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return task;
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> AddTask(TaskItem task)
        {
            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSpecificTask), new { id = task.Id }, task);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask(int id, TaskItem task)
        {
            if (id != task.Id)
            {
                return BadRequest("The task ID does not match the route ID.");
            }

            var existingTask = await _dbContext.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                return NotFound($"Task with ID {id} not found.");
            }

            // Update the properties of task
            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.DueDate = task.DueDate;
            existingTask.Status = task.Status;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskFound(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(existingTask);
        }

        private bool TaskFound(int id)
        {
            return _dbContext.Tasks.Any(t => t.Id == id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _dbContext.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound($"Task with ID {id} not found.");
            }

            _dbContext.Tasks.Remove(task);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return Ok($"Task with ID {id} deleted successfully.");
        }

    }
}
