# Task Management App 🚀

[![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-18-red)](https://angular.dev/)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-blue)](https://www.mysql.com/)
![GitHub stars](https://img.shields.io/github/stars/ckejoM/TaskManagementApp?style=social)

A **full-stack Task Management Application** built with **Angular**, **ASP.NET Core Web API**, **MySQL**, and **Entity Framework Core**. It features secure **JWT authentication**, **clean architecture**, and best practices like `ErrorOr` for functional error handling and `BaseEntity` for auditability, soft deletes, and concurrency. Perfect for managing projects, tasks, and categories with a sleek dashboard powered by **Chart.js**. 🗂️

> **Live Demo**: [Coming soon after deployment to Azure!](#) 🌐  
> **Status**: In development, stay tuned for updates! 🛠️

## 🎯 Features

- 🔒 **User Authentication**: Register and log in with JWT-based authentication using ASP.NET Core Identity.
- 🗂️ **Project & Task Management**: Create, update, and delete projects, tasks, and categories with role-based access (Admin, User).
- ✅ **Task Tracking**: Categorize tasks, set priorities (Low, Medium, High), and track completion status.
- 📊 **Dashboard**: Visualize task statistics (e.g., completed vs. pending) with interactive Chart.js charts.
- 🛡️ **Production-Grade Practices**:
  - Clean architecture for maintainability and testability.
  - `BaseEntity` with audit fields (`CreatedBy`, `ModifiedOn`) and soft deletes.
  - `ErrorOr` for robust, functional error handling.
  - Serilog for structured logging, FluentValidation for input validation, and global error handling middleware.

## 🛠️ Technologies

| **Category**      | **Tech Stack**                              |
|--------------------|---------------------------------------------|
| **Frontend**      | Angular 18, TypeScript, Angular Material, Chart.js |
| **Backend**       | ASP.NET Core 8, C#, Clean Architecture, `ErrorOr` |
| **Database**      | MySQL 8, Entity Framework Core (Code-First) |
| **Authentication** | ASP.NET Core Identity, JWT                  |
| **Tools**         | NSwag (TypeScript interfaces), Swagger (API docs), Serilog, FluentValidation |
| **Testing**       | xUnit (backend), Jasmine/Karma (frontend)   |
| **Deployment**    | Azure App Service, Azure Static Web Apps, GitHub Actions |

## 📂 Project Structure

```
TaskManagementApp/
├── src/
│   ├── backend/          # ASP.NET Core projects (Domain, Application, Infrastructure, API)
│   ├── frontend/         # Angular project
├── docs/
│   ├── api-specs/       # Swagger API exports
│   ├── diagrams/        # ERD and architecture diagrams
├── .gitignore           # Excludes build artifacts
├── README.md            # You're reading it!
```

## 🚀 Getting Started

*Setup instructions coming soon after development! Expect steps for:*
- Cloning the repo
- Setting up .NET 8, Node.js, and MySQL
- Running migrations for Entity Framework Core
- Launching the Angular frontend

## 🧪 Running Tests

- **Backend**: Run `dotnet test` in the `Tests` project to execute xUnit tests for services and `BaseEntity` logic.
- **Frontend**: Run `ng test` in the `frontend` directory for Jasmine/Karma unit tests.

## 🌟 Why This Project?

This app is a portfolio piece designed to showcase **medior-to-senior-level skills** in full-stack development. It demonstrates:
- **Clean Code**: SOLID principles, repository pattern, and dependency injection.
- **Best Practices**: Structured logging, global error handling, and input validation.
- **Modern Tech**: Integration of `ErrorOr`, `BaseEntity`, and NSwag for type-safe APIs.
- **Real-World Features**: Authentication, CRUD operations, and data visualization.

Built with ❤️ by [Jovan Madzic] ([webstethic@gmail.com](mailto:webstethic@gmail.com)).

## 📬 Feedback

Star this repo ⭐ if you find it useful! Open an issue or PR for suggestions or contributions. Let’s make task management awesome together! 😄