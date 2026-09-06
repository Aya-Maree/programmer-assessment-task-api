using TaskManagementApi.Models;

namespace TaskManagementApi.Data;

public static class DataSeeder
{
    public static void Seed(TaskDbContext context)
    {
        // only add the sample tasks if the database is empty
        if (context.Tasks.Any())
        {
            return;
        }

        // some starting tasks so there is data to work with when the app first runs
        var sampleTasks = new[]
        {
            new TaskEntry
            {
                Title = "Review project requirements",
                Description = "Review the task management API requirements.",
                Status = "Completed",
                Priority = "High",
                AssignedTo = "Aya",
                CreatedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(-1)
            },
            new TaskEntry
            {
                Title = "Test API endpoints",
                Description = "Test CRUD, search, filtering, and validation.",
                Status = "In Progress",
                Priority = "Medium",
                AssignedTo = "Aya",
                CreatedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(2)
            },
            new TaskEntry
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

        // add the sample tasks to the database
        context.Tasks.AddRange(sampleTasks);
        context.SaveChanges();
    }
}