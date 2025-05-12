# 🗂️ Task Management API

A role-based Task Management module built with **.NET Core Web API**, supporting full **CRUD** operations for Employers and Employees with tailored permissions.

---

## 🚀 Features

### 🧑‍💼 Employer Capabilities
- **Add Task**
  - Fields:
    - Title (text)
    - Description (text)
    - Picture (image upload)
    - Assignee (dropdown of employees)
    - Status (Pending, In Progress, Completed)
    - Reward Price (numeric)
  - ✅ Only employers can add tasks.

- **Edit/Delete Tasks**
  - Can edit/delete only if the task **is not assigned** yet.

- **View All Tasks**
  - Full access to all tasks in the system.

---

### 👨‍🔧 Employee Capabilities
- **View Assigned Tasks Only**
  - See tasks where the employee is the assignee.

- **Update Task Status**
  - Allowed to update the status of assigned tasks (with optional comments).

---

## 🧱 Technology Stack

- **.NET Core Web API**
- **Entity Framework Core**
- **SQL Server / SQLite**
- **Role-based Authorization**
- **Clean Architecture** (Domain, Application, Infrastructure, API layers)
- **SignalR (optional)** – for real-time task status updates *(if integrated)*

---

## 📦 API Endpoints

| Method | Endpoint             | Description                     | Access       |
|--------|----------------------|---------------------------------|--------------|
| POST   | `/api/tasks`         | Create a new task               | Employer     |
| GET    | `/api/tasks`         | Get list of tasks               | All          |
| GET    | `/api/tasks/{id}`    | Get task by ID                  | Assigned user / Employer |
| PUT    | `/api/tasks/{id}`    | Edit task                       | Employer (if not assigned) |
| DELETE | `/api/tasks/{id}`    | Delete task                     | Employer (if not assigned) |
| PATCH  | `/api/tasks/{id}/status` | Update task status + comment | Assigned Employee |

---

## 🔐 Role-Based Access

- **Employer**
  - Can create, view, edit (if unassigned), and delete tasks.
  - Can assign tasks to employees.
- **Employee**
  - Can view tasks assigned to them.
  - Can update status with comments.
  - Cannot create or delete tasks.

---
