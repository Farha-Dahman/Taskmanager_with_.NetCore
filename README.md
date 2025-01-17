# Task Manager API

This Task Manager API is a .NET Core Web API that allows users to manage tasks. It provides functionality for creating, reading, updating, deleting tasks (CRUD), as well as searching and paginating through tasks. The application uses Entity Framework Core with an In-Memory Database for simplicity and demonstrates key features of a RESTful API.

# Features

- **Add Task:** Create a new task.
  
- **View All Tasks:** Retrieve a list of all tasks with optional pagination.
  
- **Search Tasks:** Search for tasks by title or description.

- **View Task by `ID`:** Retrieve a specific task by its ID.

- **Update Task:** Update an existing task.

- **Delete Task:** Remove a task by its ID.

- **Error Handling:** Provides appropriate HTTP status codes and error messages.

- **Swagger Documentation:** API documentation and testing via Swagger.

# Prerequisites

Ensure you have the following installed on your system:

- .NET Core.

- Visual Studio 2022 or any other code editor.

# Installation Instructions

Clone the Repository:

``` git clone <repository-url>    
cd TaskManagerAPI
```
Restore Dependencies:

```
dotnet restore
```

Run the Application:

```
dotnet run
```

Access the API Documentation:
Open your browser and navigate to:

```
http://localhost:<port>/swagger
```

Swagger provides a UI to interact with and test the API endpoints.


# API Endpoints

**1. Add Task**

Endpoint: `POST /api/tasks`

Description: Creates a new task.

Request Body (JSON):

```    
{
  "title": "Sample Task",
  "description": "Task description",
  "dueDate": "2025-01-16T13:11:06.817Z",
  "priority": "High",
  "status": "In Progress",
  "category": "Personal"
}
```
Response: Returns the created task with its `ID`.

**2. View All Tasks**

Endpoint: `GET /api/tasks`   

Description: Retrieves all tasks with optional pagination.

Query Parameters:

`page` (optional): Page number (default: `1`)   

`pageSize` (optional): Number of tasks per page (default: `10`)       

Response: Returns a paginated list of tasks.

**3. Search Tasks**

Endpoint: `GET /api/tasks/search`

Description: Searches for tasks by title or description.

Query Parameters:

`query`: The search string.

Response: Returns a list of tasks matching the search criteria.

**4. View Task by `ID`**

Endpoint: `GET /api/tasks/{id}`

Description: Retrieves a specific task by `ID`.

Response: Returns the task if found, otherwise a 404 error.

**5. Update Task**

Endpoint: `PUT /api/tasks/{id}`

Description: Updates an existing task.

Request Body (JSON):

```
{
  "id": 1,
  "title": "Updated Task",
  "description": "Updated description",
  "dueDate": "2025-02-01T10:00:00Z",
  "priority": "Medium",
  "status": "Completed",
  "category": "Work"
}
```

Response: Returns the updated task if successful.

**6. Delete Task**

Endpoint: `DELETE /api/tasks/{id}`

Description: Deletes a specific task by `ID `.

Response: Returns a success message if deleted.

---

