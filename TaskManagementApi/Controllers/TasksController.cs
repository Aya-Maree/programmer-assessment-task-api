using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Models;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TaskDbContext _context; //db connection through EF Core

    public TasksController(TaskDbContext context)
    {
        _context = context;
    }

    [HttpGet]
public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks(
    string? search,
    string? status,
    string? priority)
{
    var query = _context.Tasks.AsQueryable();

    if (!string.IsNullOrWhiteSpace(search))
    {
        query = query.Where(task =>
            task.Title.Contains(search) ||
            (task.Description != null && task.Description.Contains(search)));
    }

    if (!string.IsNullOrWhiteSpace(status))
    {
        query = query.Where(task => task.Status == status);
    }

    if (!string.IsNullOrWhiteSpace(priority))
    {
        query = query.Where(task => task.Priority == priority);
    }

    return await query.ToListAsync();
}

    [HttpGet("{id}")] //makes GET /api/tasks/{id} return a specific task by its ID
    public async Task<ActionResult<TaskItem>> GetTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task == null)
        {
            return NotFound();
        }

        return task;
    }

    [HttpPost]
public async Task<ActionResult<TaskItem>> CreateTask(TaskItem task)
{
    task.CreatedDate = DateTime.UtcNow;

    _context.Tasks.Add(task);
    await _context.SaveChangesAsync();

    return CreatedAtAction(
        nameof(GetTask),
        new { id = task.Id },
        task
    );
}

[HttpPut("{id}")]
public async Task<IActionResult> UpdateTask(int id, TaskItem task)
{
    if (id != task.Id)
    {
        return BadRequest();
    }

    var existingTask = await _context.Tasks.FindAsync(id);

    if (existingTask == null)
    {
        return NotFound();
    }

    existingTask.Title = task.Title;
    existingTask.Description = task.Description;
    existingTask.Status = task.Status;
    existingTask.Priority = task.Priority;
    existingTask.AssignedTo = task.AssignedTo;
    existingTask.DueDate = task.DueDate;

    await _context.SaveChangesAsync();

    return NoContent();
}

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

}
