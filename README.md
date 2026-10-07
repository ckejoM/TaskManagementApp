# TaskManagementApp

> **Status: work in progress.** This is one of my earlier full-stack projects, and parts of it are still being built.

A task and project manager: register, log in, then organize projects, tasks, and categories, with a dashboard of task statistics.

## What's in it so far
- **Authentication:** ASP.NET Core Identity with JWT bearer tokens
- **Projects, tasks, and categories:** CRUD, with priorities (Low/Medium/High) and completion status
- **Dashboard:** task statistics charted with Chart.js
- **Backend structure:** Clean Architecture (Domain, Application, Infrastructure, API), a `BaseEntity` with audit fields, FluentValidation, Serilog, and NSwag to generate TypeScript clients from the API

## Stack
| Area | Tech |
| --- | --- |
| Frontend | Angular 19, TypeScript, PrimeNG (Sakai template), Chart.js, Tailwind CSS |
| Backend | ASP.NET Core 8, C#, Clean Architecture |
| Data | SQL Server, EF Core (code-first migrations) |
| Auth | ASP.NET Core Identity, JWT |
| Tooling | NSwag, Swagger, Serilog, FluentValidation |

## Layout
```text
src/
  backend/TaskManagementApp/   # Domain, Application, Infrastructure, API
  frontend/task-management-app # Angular app
  frontend/sakai-ng            # PrimeNG Sakai template used for the UI
```

## Run it locally
Prerequisites: .NET 8 SDK, Node.js + Angular CLI, SQL Server.
1. Set the connection string in `src/backend/TaskManagementApp/API/appsettings.Development.json`.
2. Apply migrations and run the API:
   `cd src/backend/TaskManagementApp && dotnet ef database update --project Infrastructure --startup-project API && dotnet run --project API`
3. Run the UI: `cd src/frontend/task-management-app && npm install && ng serve`

## What's next
- Backend tests (xUnit) and frontend tests
- Role-based access (Admin/User) across all endpoints
- A deployment pipeline

---

Built by Jovan Madzic, Software Engineer in Belgrade · [LinkedIn](https://www.linkedin.com/in/jovan-madzic-12093b202/) · [GitHub](https://github.com/ckejoM)
