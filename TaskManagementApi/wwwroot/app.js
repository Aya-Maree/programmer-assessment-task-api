const taskList = document.getElementById("taskList");
const searchInput = document.getElementById("search");
const statusFilter = document.getElementById("statusFilter");
const priorityFilter = document.getElementById("priorityFilter");
const overdueButton = document.getElementById("overdueButton");

const titleInput = document.getElementById("title");
const descriptionInput = document.getElementById("description");
const statusInput = document.getElementById("status");
const priorityInput = document.getElementById("priority");
const assignedToInput = document.getElementById("assignedTo");
const dueDateInput = document.getElementById("dueDate");
const addTaskButton = document.getElementById("addTaskButton");
const message = document.getElementById("message");

let editingTaskId = null;

// load tasks from the api
async function loadTasks() {
    let url = "/api/tasks";
    const params = new URLSearchParams();

    if (searchInput.value.trim() !== "") {
        params.append("search", searchInput.value.trim());
    }

    if (statusFilter.value !== "") {
        params.append("status", statusFilter.value);
    }

    if (priorityFilter.value !== "") {
        params.append("priority", priorityFilter.value);
    }

    if (params.toString() !== "") {
        url += "?" + params.toString();
    }

    const response = await fetch(url);
    const tasks = await response.json();

    displayTasks(tasks);
}

function displayTasks(tasks) {
    taskList.innerHTML = "";

    if (tasks.length === 0) {
        taskList.innerHTML = "<p>No tasks found.</p>";
        return;
    }

    tasks.forEach(task => {
        const taskDiv = document.createElement("div");
        taskDiv.className = "task";

        const dueDate = task.dueDate
            ? new Date(task.dueDate).toLocaleDateString()
            : "No due date";

        taskDiv.innerHTML = `
            <h3>${task.title}</h3>
            <p>${task.description || ""}</p>

            <p class="task-details">
                Status: ${task.status} |
                Priority: ${task.priority} |
                Assigned to: ${task.assignedTo || "Unassigned"} |
                Due: ${dueDate}
            </p>

            <div class="task-buttons">
                <button class="edit-button" onclick="editTask(${task.id})">
                    Edit
                </button>

                <button class="delete-button" onclick="deleteTask(${task.id})">
                    Delete
                </button>
            </div>
        `;

        taskList.appendChild(taskDiv);
    });
}

function clearForm() {
    titleInput.value = "";
    descriptionInput.value = "";
    statusInput.value = "Not Started";
    priorityInput.value = "Low";
    assignedToInput.value = "";
    dueDateInput.value = "";
}

// add a new task or save an edited one
async function saveTask() {
    message.textContent = "";

    const task = {
        title: titleInput.value.trim(),
        description: descriptionInput.value.trim(),
        status: statusInput.value,
        priority: priorityInput.value,
        assignedTo: assignedToInput.value.trim() || null,
        dueDate: dueDateInput.value || null
    };

    let response;
    const wasEditing = editingTaskId !== null;

    if (wasEditing) {
        task.id = editingTaskId;

        response = await fetch(`/api/tasks/${editingTaskId}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(task)
        });
    } else {
        response = await fetch("/api/tasks", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(task)
        });
    }

    if (!response.ok) {
        const error = await response.text();
        message.textContent = error;
        return;
    }

    clearForm();

    editingTaskId = null;
    addTaskButton.textContent = "Add Task";

    message.textContent = wasEditing
        ? "Task updated."
        : "Task added.";

    loadTasks();
}

// put the task into the form so it can be edited
async function editTask(id) {
    const response = await fetch(`/api/tasks/${id}`);

    if (!response.ok) {
        alert("Could not load task.");
        return;
    }

    const task = await response.json();

    editingTaskId = task.id;

    titleInput.value = task.title;
    descriptionInput.value = task.description || "";
    statusInput.value = task.status;
    priorityInput.value = task.priority;
    assignedToInput.value = task.assignedTo || "";

    if (task.dueDate) {
        dueDateInput.value = task.dueDate.split("T")[0];
    } else {
        dueDateInput.value = "";
    }

    addTaskButton.textContent = "Save Changes";

    titleInput.focus();
    window.scrollTo(0, 0);
}

// delete a task
async function deleteTask(id) {
    const response = await fetch(`/api/tasks/${id}`, {
        method: "DELETE"
    });

    if (response.ok) {
        loadTasks();
    }
}

let showingOverdue = false;

// switch between overdue tasks and all tasks
async function loadOverdueTasks() {
    if (showingOverdue) {
        showingOverdue = false;
        overdueButton.textContent = "Show Overdue";
        loadTasks();
        return;
    }

    const response = await fetch("/api/tasks/overdue");
    const tasks = await response.json();

    displayTasks(tasks);

    showingOverdue = true;
    overdueButton.textContent = "Show All Tasks";
}

addTaskButton.addEventListener("click", saveTask);

searchInput.addEventListener("input", loadTasks);
statusFilter.addEventListener("change", loadTasks);
priorityFilter.addEventListener("change", loadTasks);
overdueButton.addEventListener("click", loadOverdueTasks);

// load everything when the page opens
loadTasks();