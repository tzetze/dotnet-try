## 1. Backend test project setup

- [ ] 1.1 Create `backend.Tests/TaskManager.Api.Tests.csproj` (xUnit, net10.0) referencing `backend/TaskManager.Api.csproj`
- [ ] 1.2 Add packages: `xunit`, `xunit.runner.visualstudio`, `Microsoft.NET.Test.Sdk`, `Microsoft.AspNetCore.Mvc.Testing`
- [ ] 1.3 Add `public partial class Program {}` to the end of `backend/Program.cs`
- [ ] 1.4 Register the test project in `TaskManager.slnx`
- [ ] 1.5 Confirm `dotnet test` discovers the project (0 tests OK at this point)

## 2. Backend unit tests (InMemoryTaskService)

- [ ] 2.1 Add `InMemoryTaskServiceTests`: Add assigns incrementing id and stores task
- [ ] 2.2 GetById returns the task / null when missing
- [ ] 2.3 Update replaces title, isDone, priority, and description
- [ ] 2.4 Update with empty description clears an existing description
- [ ] 2.5 Update returns false for a missing id; Delete removes / returns false when missing

## 3. Backend integration tests (API over HTTP)

- [ ] 3.1 Add `TasksApiTests` using `WebApplicationFactory<Program>` (fresh factory per test)
- [ ] 3.2 GET /api/tasks returns seeded tasks including the description field
- [ ] 3.3 POST with a description returns 201 and echoes the description
- [ ] 3.4 PUT updates the description (204) and a subsequent GET reflects it
- [ ] 3.5 POST with a description >500 chars returns 400 and does not store the task

## 4. Acceptance tests (spec scenarios)

- [ ] 4.1 Add `AcceptanceTests` with one test per `task-management` scenario, names echoing the scenario text
- [ ] 4.2 Cover: create with description, create without description, length validation, persist on update, clear on update
- [ ] 4.3 Run `dotnet test` — all backend tests green

## 5. Frontend tests (App component, ng test / vitest)

- [ ] 5.1 Repair `app.spec.ts`: provide `provideHttpClient` + `provideHttpClientTesting`; fix the stale title assertion
- [ ] 5.2 Test: typing in the description input makes `descriptionRemaining()` equal `500 - length`
- [ ] 5.3 Test: `addTask()` sends `description` in the create payload (assert via `HttpTestingController`)
- [ ] 5.4 Test: a task with a description renders the `.description` element; one without does not
- [ ] 5.5 Run `npm test` (`ng test`) — frontend tests green

## 6. Final verification

- [ ] 6.1 `dotnet test` and `npm test` both pass from a clean state
- [ ] 6.2 Confirm test `bin`/`obj` and frontend artifacts are gitignored (not staged)
