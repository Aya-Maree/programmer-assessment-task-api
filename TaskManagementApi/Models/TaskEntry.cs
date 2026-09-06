namespace TaskManagementApi.Models;

public class TaskEntry
{
    public int Id { get; set; } // unique id for each task
    public string Title { get; set; } = string.Empty; // name of the task
    public string? Description { get; set; } // extra details about the task
    public string Status { get; set; } = string.Empty; // current status of the task
    public string Priority { get; set; } = string.Empty; // how important the task is
    public string? AssignedTo { get; set; } // person the task is assigned to
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // date the task was created
    public DateTime? DueDate { get; set; } // date the task should be completed by
}