using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Models;

namespace TaskManagementApi.Data;

public class TaskDbContext : DbContext
{
    // sets up the database context using the options from Program.cs
    public TaskDbContext(DbContextOptions<TaskDbContext> options)
        : base(options)
    {
    }

    // represents the tasks table in the database
    public DbSet<TaskEntry> Tasks { get; set; }
}