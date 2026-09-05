using TaskManagementApi.Models;

namespace TaskManagementApi.Data;

public static class DataSeeder
{
    public static void Seed(TaskDbContext context)
    {
        if (context.Tasks.Any())
        {
            return;
        }

        var tasks = new[]
        {
            new TaskItem
            {
                Title = "Review project requirements",
                Description = "Review the task management API requirements.",
                Status = "Completed",
                Priority = "High",
                AssignedTo = "Aya",
                CreatedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(-1)
            },
            new TaskItem
            {
                Title = "Test API endpoints",
                Description = "Test CRUD, search, filtering, and validation.",
                Status = "In Progress",
                Priority = "Medium",
                AssignedTo = "Aya",
                CreatedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(2)
            },
            new TaskItem
            {
                Title = "Complete README",
                Description = "Add setup and run instructions.",
                Status = "Not Started",
                Priority = "Low",
                AssignedTo = "Aya",
                CreatedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(3)
            }
        };

        context.Tasks.AddRange(tasks);
        context.SaveChanges();
    }
}