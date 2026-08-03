# Transaction Service

## Overview

A RESTful Web API built with ASP.NET Core 8 for managing users and getting transaction summaries.

The project demonstrates a clean layered architecture, repository pattern, dependency injection, object mapping, and testing practices.

# Features

- User CRUD operations
- Transaction Create and Read operations
- Retrieve high-volume transactions above a specified threshold
- Generate transaction summary grouped by user
- Generate transaction summary grouped by transaction type
- Global exception handling
- Unit tests
- Integration tests using a dedicated SQL Server test database

---

# Architecture

The solution follows a layered architecture based on the **Separation of Concerns** principle.

```text
Presentation (API)
       | 
       |
Application
(DTOs / Managers / Mappings)
       |
       |
Infrastructure
(Repositories / EF Core / SQL Server)
       |
       |
Domain
(Entities / Enums / QueryModels)
```

Each layer has a single responsibility and communicates with other layers through abstractions, improving maintainability, testability, and extensibility.

---

# Technology Stack

| Technology | Purpose |
|------------|---------|
| ASP.NET Core 8 | REST API Framework |
| Entity Framework Core | ORM and Data Access |
| SQL Server | Database |
| Docker | Local SQL Server Hosting for Development & Integration Testing |
| AutoMapper | Object Mapping |
| Swagger / OpenAPI | API Documentation and Testing |
| xUnit | Unit & Integration Testing |
| Moq | Mocking Dependencies |
| FluentAssertions | Test Assertions |

---

# Design Decisions

## Layered Architecture

The application is separated into API, Application, Infrastructure, and Domain layers. This approach improves maintainability, allows independent evolution of each layer, and simplifies testing.

## Repository Pattern

Repositories encapsulate all data access logic, separating the business layer from the data access layer. Managers depend only on repository abstractions rather than Entity Framework Core directly, improving maintainability, testability, and flexibility. Repository implementations are responsible only for data access, while business rules remain within the application layer.

## Dependency Injection & Service Registration

Dependency registrations are organized using dedicated extension methods in each `ServiceCollectionExtensions.cs` for Infrastructure and Api layer. This keeps `Program.cs` clean and allows every project to manage its own dependencies independently, improving modularity and maintainability.

## Dependency Inversion

Managers depend on abstractions (`IUserRepository`, `ITransactionRepository`) rather than concrete repository implementations. In the same way, controllers depend on manager interfaces instead of concrete manager classes.

This approach reduces coupling between layers, improves testability by allowing dependencies to be mocked during unit testing, and provides flexibility to replace implementations without affecting consuming components.

## Database Design

The database schema was intentionally designed to remain simple while following relational database best practices. Proper relationships, appropriate data types (such as using varchar(36) for the UserId field, which stores GUID values), and indexing (on the Type, UserId, and Amount columns in the Transactions table) were carefully considered to ensure data integrity and efficient query execution without introducing unnecessary complexity.

## Dockerized SQL Server

SQL Server is hosted using Docker to provide a consistent development and testing environment. This removes machine-specific dependencies and allows both the application and integration tests to run against the same database configuration.

## DTOs

DTOs isolate the API contract from domain entities. This prevents exposing data entites directly and provides flexibility for future API changes.

## Query Models

Dedicated Query Models are used for reporting endpoints to represent aggregated data rather than exposing entities directly. This keeps reporting queries independent from the domain model.

## Domain Entities

Domain entities represent the application's core business data and are mapped directly to the database. They remain focused on persistence and business concepts without being coupled to API request or response models.

## AutoMapper

AutoMapper is used to simplify mappings between domain entities or query models and DTOs, reducing repetitive mapping code and improving readability.

## Testing Strategy

The project contains both **Unit Tests** and **Integration Tests**.

### Unit Tests

- Unit tests focus on the manager layer, where the application's business logic resides.
- Controllers and repository implementations are not unit tested, as controllers contain no business logic and repositories primarily handle data access through Entity Framework Core.
- Repository dependencies are mocked using Moq.
- FluentAssertions is used for readable and expressive assertions.

### Integration Tests

- Integration tests validate the complete application flow by exercising the full ASP.NET Core request pipeline through `WebApplicationFactory`. Each test sends real HTTP requests and verifies the interaction between controllers, managers, repositories, Entity Framework Core, middleware, and a dedicated SQL Server database running in Docker
- Tests execute against a dedicated SQL Server test database.
- The database is recreated before each test to ensure complete isolation and repeatability.

---

# Project Structure

```text
src/

- TransactionService.Api
- TransactionService.Application
- TransactionService.Domain
- TransactionService.Infrastructure

tests/
- TransactionService.UnitTests
- TransactionService.IntegrationTests
```

---

# Running the Application

## Prerequisites

- .NET 8 SDK
- SQL Server
  - SQL Server installed locally **or**
  - SQL Server running via Docker

## Clone the repository

```bash
git clone https://github.com/yaseminga/transaction-service.git
```

## Start SQL Server

## Option 1 - Start SQL Server using Docker

The repository includes a `docker-compose.yml` file that starts a SQL Server instance configured for local development.

```bash
docker compose up -d
```

## Option 2 - Use a Local SQL Server

Update the connection string in `appsettings.json` at `TransactionService.Api` and `CustomWebApplicationFactory.cs` at `TransactionService.IntegrationTests` to point to your local SQL Server instance.


## Restore packages

```bash
dotnet restore
```

## Build

```bash
dotnet build
```

## Apply migrations

```bash
dotnet ef database update --project src/TransactionService.Infrastructure --startup-project src/TransactionService.Api
```

## Build

```bash
dotnet build
```

## Run the application

```bash
dotnet run --project src/TransactionService.Api
```

Swagger is available at:

```text
https://localhost:7064/swagger
```

---

# Running Tests

Run all tests

```bash
dotnet test
```

Run only Unit Tests

```bash
dotnet test tests/TransactionService.UnitTests
```

Run only Integration Tests

```bash
dotnet test tests/TransactionService.IntegrationTests
```

---

# API Documentation

Interactive API documentation is available through Swagger after the application starts.

---

# Assumptions

- User identifiers are assumed to be GUID values represented as strings.
- Every transaction belongs to exactly one user.
- A transaction cannot be created for a non-existing user.
- A user cannot be deleted if they have existing transactions, ensuring that transaction history is preserved.

---

# Future Improvement

If I had one additional day available, I would integrate **FluentValidation** to move request validation into a dedicated validation layer. This would keep validation concerns separate from business logic, improve maintainability, and make it easier to extend validation rules as the API evolves.

---

# Notes

The primary goal of this project was to demonstrate clean architecture, maintainable code organization, testing, and enterprise-level development practices while keeping the solution simple and easy to understand.