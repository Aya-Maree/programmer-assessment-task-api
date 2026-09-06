# Task Management API

For this project, I built a task management API using C# and ASP.NET Core. I used Entity Framework Core with SQLite for the database.

The API lets you create, view, update, and delete tasks. I also added searching, filtering, validation, and a way to view overdue tasks.

I added a simple frontend as well so the tasks can be managed from the browser, but the API can also be tested directly without using the frontend.

## How to Run

You will need the .NET 10 SDK installed.

First, clone the repository:

```bash
git clone https://github.com/Aya-Maree/programmer-assessment-task-api.git
```

Then go into the project folder:

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

The terminal will show the local URL the application is running on. On my machine, it runs on:

```text
http://localhost:5276
```

You can open that URL in your browser to use the frontend.

The database is set up automatically when the application starts. The migrations are applied and some sample tasks are added if the database is empty.

## Tasks

Each task includes:

- Id
- Title
- Description
- Status
- Priority
- Assigned To
- Created Date
- Due Date

For status, I used:

- Not Started
- In Progress
- Completed

For priority, I used:

- Low
- Medium
- High

## API Endpoints

| Method | Endpoint | What it does |
| --- | --- | --- |
| GET | `/api/tasks` | Gets all tasks |
| GET | `/api/tasks/{id}` | Gets one task |
| POST | `/api/tasks` | Creates a task |
| PUT | `/api/tasks/{id}` | Updates a task |
| DELETE | `/api/tasks/{id}` | Deletes a task |
| GET | `/api/tasks/overdue` | Gets overdue tasks |

## Testing the API

The backend can be tested directly without using the frontend. I used `curl` from the terminal, but the same requests can also be tested using an API client such as Postman.

Make sure the application is running first:

```bash
dotnet run
```

The examples below use `http://localhost:5276`. If the terminal shows a different port when the application starts, use that URL instead.

### Get all tasks

```bash
curl http://localhost:5276/api/tasks
```

### Get one task

Replace `1` with the ID of the task you want to retrieve:

```bash
curl http://localhost:5276/api/tasks/1
```

### Create a task

```bash
curl -X POST http://localhost:5276/api/tasks \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Test API",
    "description": "Testing the create endpoint",
    "status": "Not Started",
    "priority": "High",
    "assignedTo": "Aya",
    "dueDate": "2026-09-10T12:00:00"
  }'
```

The response will include the ID that was created for the new task.

### Update a task

Replace `4` with the ID of the task you want to update. The ID in the URL and the request body should match.

```bash
curl -X PUT http://localhost:5276/api/tasks/4 \
  -H "Content-Type: application/json" \
  -d '{
    "id": 4,
    "title": "Test API",
    "description": "Updated task",
    "status": "In Progress",
    "priority": "High",
    "assignedTo": "Aya",
    "dueDate": "2026-09-10T12:00:00"
  }'
```

### Delete a task

Replace `4` with the ID of the task you want to delete:

```bash
curl -X DELETE http://localhost:5276/api/tasks/4
```

### Search tasks

The search checks both the title and description:

```bash
curl "http://localhost:5276/api/tasks?search=project"
```

### Filter tasks

Filter by status:

```bash
curl "http://localhost:5276/api/tasks?status=Completed"
```

Filter by priority:

```bash
curl "http://localhost:5276/api/tasks?priority=High"
```

The filters can also be combined:

```bash
curl "http://localhost:5276/api/tasks?status=In%20Progress&priority=High"
```

### Get overdue tasks

```bash
curl http://localhost:5276/api/tasks/overdue
```

A task will appear here if its due date has passed and its status is not `Completed`.

## Validation

I added some basic validation when creating and updating tasks.

The title is required, and the status and priority have to match one of the accepted values listed above. Invalid date values are also rejected.

Invalid requests return a `400 Bad Request`, and trying to get a task that does not exist returns a `404 Not Found`.

For example, a request with an empty title can be used to test the validation:

```bash
curl -X POST http://localhost:5276/api/tasks \
  -H "Content-Type: application/json" \
  -d '{
    "title": "",
    "status": "Not Started",
    "priority": "High"
  }'
```

## Overdue Tasks

A task is considered overdue if its due date has passed and its status is not `Completed`.

Overdue tasks can be viewed using:

```text
GET /api/tasks/overdue
```

They can also be viewed from the frontend using the Show Overdue button.

## Frontend

I made a small frontend using HTML, CSS, and JavaScript to make it easier to interact with the API.

From the frontend you can:

- view tasks
- add new tasks
- edit tasks
- delete tasks
- search tasks
- filter by status or priority
- view overdue tasks

The frontend uses the same API endpoints listed above.

## Database

I used SQLite because it keeps the project simple to set up and does not require a separate database server.

Entity Framework Core handles the database access and migrations. When the application starts, it applies the migrations and adds sample data if there are no tasks in the database yet.