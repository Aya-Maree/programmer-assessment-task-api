# Task Management API

This is a simple task management API I built using C# and ASP.NET Core. It uses Entity Framework Core with SQLite to store the tasks.

The API allows you to create, view, update, and delete tasks. I also added searching and filtering, along with an endpoint for finding overdue tasks.

## Getting Started

You will need the .NET 10 SDK installed.

Clone the repository:

```bash
git clone https://github.com/Aya-Maree/programmer-assessment-task-api.git
```

Go into the project folder:

```bash
cd programmer-assessment-task-api/TaskManagementApi
```

Restore the packages:

```bash
dotnet restore
```

Run the project:

```bash
dotnet run
```

The terminal will show the local URL the API is running on. On my machine it runs on:

```text
http://localhost:5276
```

The SQLite database is set up automatically when the project starts. The migrations will be applied and some sample tasks will be added if the database is empty.

## Task Information

Each task has:

- Id
- Title
- Description
- Status
- Priority
- Assigned To
- Created Date
- Due Date

For status, the accepted values are `Not Started`, `In Progress`, and `Completed`.

For priority, the accepted values are `Low`, `Medium`, and `High`.

## Endpoints

| Method | Endpoint | Description |
| --- | --- | --- |
| GET | `/api/tasks` | Get all tasks |
| GET | `/api/tasks/{id}` | Get a task by id |
| POST | `/api/tasks` | Create a new task |
| PUT | `/api/tasks/{id}` | Update a task |
| DELETE | `/api/tasks/{id}` | Delete a task |
| GET | `/api/tasks/overdue` | Get overdue tasks |

An overdue task is a task where the due date has passed and the status is not `Completed`.

## Creating a Task

Example POST request:

```json
{
  "title": "Finish documentation",
  "description": "Finish the README for the project",
  "status": "In Progress",
  "priority": "High",
  "assignedTo": "Aya",
  "dueDate": "2026-09-07T12:00:00"
}
```

The created date is set by the API when the task is created.

## Search and Filtering

Tasks can be searched using the `search` parameter. This searches both the title and description.

```text
GET /api/tasks?search=documentation
```

Tasks can also be filtered by status or priority:

```text
GET /api/tasks?status=Completed

GET /api/tasks?priority=High
```

The filters can be used together too:

```text
GET /api/tasks?status=In%20Progress&priority=High
```

## Validation

There is some basic validation when creating or updating tasks. A title is required, and the status and priority need to be one of the accepted values listed above.

Invalid requests return a `400 Bad Request`, and trying to get a task that does not exist returns a `404 Not Found`.

## Database

I used SQLite because it keeps the project simple to set up and run without needing a separate database server.

Entity Framework Core is used for the database access and migrations. When the application starts, it applies the migrations and adds sample data if there are no tasks in the database yet.