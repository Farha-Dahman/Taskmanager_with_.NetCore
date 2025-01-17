using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager_With_DotNetCore.Models;

namespace TaskManager_With_DotNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskContext _dbContext;

        public TaskController(TaskContext context)
        {
            _dbContext = context;
        }

        // GET: Get All Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (_dbContext.Tasks == null)
            {
                return NotFound("No tasks found.");
            }

            try
            {
                // To ensure page and pageSize are valid
                page = Math.Max(page, 1);
                pageSize = Math.Max(pageSize, 1);

                var totalTasks = await _dbContext.Tasks.CountAsync();
                var tasks = await _dbContext.Tasks
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new
                {
                    TotalCount = totalTasks,
                    Page = page,
                    PageSize = pageSize,
                    Tasks = tasks
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: Get A spacific Task
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetSpecificTask(int id)
        {
            if (_dbContext.Tasks == null)
            {
                return NotFound($"No Tasks are found!.");
            }
            var task = await _dbContext.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound($"Task with ID {id} not found.");
            }

            return Ok(task);
        }

        // POST: Create A New Task
        [HttpPost]
        public async Task<ActionResult<TaskItem>> AddTask([FromBody] TaskItem task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _dbContext.Tasks.AddAsync(task);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetSpecificTask), new { id = task.Id }, task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: Update A Task
        [HttpPut]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem updatedTask)
        {
            if (id != updatedTask.Id)
            {
                return BadRequest("Task ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _dbContext.Entry(updatedTask).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.Tasks.Any(t => t.Id == id))
                {
                    return NotFound($"Task with ID {id} not found.");
                }
                throw;
            }

            return Ok(updatedTask);
        }

        private bool TaskFound(int id)
        {
            return _dbContext.Tasks.Any(t => t.Id == id);
        }

        // DELETE: Delete A Task
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

        // GET: Search For Tasks
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> SearchTasks([FromQuery] string? query)
        {
            if (_dbContext.Tasks == null || string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty.");
            }

            try
            {
                var tasks = await _dbContext.Tasks
                    .Where(t => t.Title.Contains(query) || t.Description.Contains(query))
                    .ToListAsync();

                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
