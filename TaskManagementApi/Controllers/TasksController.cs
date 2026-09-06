using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Models;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    // database connection through EF Core
    private readonly TaskDbContext _context;

    // statuses that are allowed for a task
    private static readonly string[] ValidStatuses =
    {
        "Not Started",
        "In Progress",
        "Completed"
    };

    // priorities that are allowed for a task
    private static readonly string[] ValidPriorities =
    {
        "Low",
        "Medium",
        "High"
    };

    public TasksController(TaskDbContext context)
    {
        _context = context;
    }

    // gets all tasks and can also search or filter them
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskEntry>>> GetTasks(
        string? search,
        string? status,
        string? priority)
    {
        var query = _context.Tasks.AsQueryable();

        // search the title or description
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(task =>
                task.Title.Contains(search) ||
                (task.Description != null && task.Description.Contains(search)));
        }

        // filter by status
        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(task => task.Status == status);
        }

        // filter by priority
        if (!string.IsNullOrWhiteSpace(priority))
        {
            query = query.Where(task => task.Priority == priority);
        }

        return await query.ToListAsync();
    }

    // gets one task using its id
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskEntry>> GetTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task == null)
        {
            return NotFound();
        }

        return task;
    }

    // creates a new task
    [HttpPost]
    public async Task<ActionResult<TaskEntry>> CreateTask(TaskEntry task)
    {
        // make sure the title was entered
        if (string.IsNullOrWhiteSpace(task.Title))
        {
            return BadRequest("Title is required.");
        }

        // make sure the status is valid
        if (!ValidStatuses.Contains(task.Status))
        {
            return BadRequest("Status must be Not Started, In Progress, or Completed.");
        }

        // make sure the priority is valid
        if (!ValidPriorities.Contains(task.Priority))
        {
            return BadRequest("Priority must be Low, Medium, or High.");
        }

        // set the created date when the task is added
        task.CreatedDate = DateTime.UtcNow;

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTask),
            new { id = task.Id },
            task
        );
    }

    // updates an existing task
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, TaskEntry task)
    {
        // the id in the url and task should match
        if (id != task.Id)
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(task.Title))
        {
            return BadRequest("Title is required.");
        }

        if (!ValidStatuses.Contains(task.Status))
        {
            return BadRequest("Status must be Not Started, In Progress, or Completed.");
        }

        if (!ValidPriorities.Contains(task.Priority))
        {
            return BadRequest("Priority must be Low, Medium, or High.");
        }

        var existingTask = await _context.Tasks.FindAsync(id);

        if (existingTask == null)
        {
            return NotFound();
        }

        // update the task with the new values
        existingTask.Title = task.Title;
        existingTask.Description = task.Description;
        existingTask.Status = task.Status;
        existingTask.Priority = task.Priority;
        existingTask.AssignedTo = task.AssignedTo;
        existingTask.DueDate = task.DueDate;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // deletes a task using its id
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task == null)
        {
            return NotFound();
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // gets tasks that are past their due date and are not completed
    [HttpGet("overdue")]
    public async Task<ActionResult<IEnumerable<TaskEntry>>> GetOverdueTasks()
    {
        var currentDate = DateTime.UtcNow;

        var overdueTasks = await _context.Tasks
            .Where(task =>
                task.DueDate != null &&
                task.DueDate < currentDate &&
                task.Status != "Completed")
            .ToListAsync();

        return overdueTasks;
    }
}