## Context

The Task Manager is a learning project with a .NET 10 Web API (`TaskItem`,
`ITaskService`/`InMemoryTaskService`, `TasksController`) and an Angular frontend.
There is no backend test project. The frontend is configured for **vitest** via
`ng test` (not Karma/Jasmine), and the only spec (`app.spec.ts`) is stale — it
asserts a "Hello, frontend" title that no longer exists. This change adds
automated tests at three levels (unit, integration, acceptance) mirroring the
`task-management` spec scenarios.

## Goals / Non-Goals

**Goals:**
- Unit-test `InMemoryTaskService` business logic in isolation.
- Integration-test the API over real HTTP, including the 500-char validation.
- Make the spec executable: one acceptance test per `task-management` scenario.
- Repair and extend frontend tests for the description UI.

**Non-Goals:**
- CI pipeline configuration (just runnable via `dotnet test` / `npm test`).
- E2E browser automation (Playwright/Cypress) — out of scope for this project.
- Testing unrelated/pre-existing behavior beyond smoke coverage.

## Decisions

- **Test framework: xUnit.** Default and idiomatic for .NET; simple `[Fact]`/`[Theory]`.
  - *Alternative:* NUnit/MSTest — no advantage here.

- **Integration tests via `WebApplicationFactory<Program>`** from
  `Microsoft.AspNetCore.Mvc.Testing`. This boots the real app in-memory and gives
  an `HttpClient` — no live port, fast, exercises the full pipeline (routing,
  JSON, `[ApiController]` validation).
  - *Requirement:* top-level-statement `Program` is internal, so add
    `public partial class Program {}` at the end of `Program.cs` to reference it.
  - *Alternative:* spin up a real Kestrel server + curl — slower, flaky, manual.
  - *Note on state:* `InMemoryTaskService` is a singleton seeded with 2 tasks.
    Integration tests must not assume an empty store; assert on created ids /
    round-trips rather than absolute counts. Each test gets its own factory
    instance (fresh singleton) to avoid cross-test bleed.

- **Acceptance tests = thin layer mapping scenarios to assertions.** Put them in a
  dedicated `AcceptanceTests` class with test names echoing the spec scenario
  text, so traceability spec↔test is obvious. They reuse the unit/integration
  machinery rather than duplicating it.

- **Frontend: keep vitest (`ng test`).** Repair `app.spec.ts`, use
  `provideHttpClientTesting` + `HttpTestingController` to stub the API so the
  component tests don't need a backend. Cover: counter equals `500 - length`,
  description sent in create payload, description renders only when present.
  - *Alternative:* switch to Karma/Jasmine — rejected; vitest is already wired.

- **Project layout:** `backend.Tests/` sibling dir with `TaskManager.Api.Tests.csproj`
  referencing the API project; register it in `TaskManager.slnx`. Test artifacts
  (`bin`/`obj`) are already covered by the root `.gitignore`.

## Risks / Trade-offs

- **Shared singleton store bleeds across integration tests** → Use a fresh
  `WebApplicationFactory` per test class/test and assert on round-trip data, not
  global counts.
- **`Program` must be referenceable** → Add `public partial class Program {}`;
  purely additive, no behavior change.
- **vitest vs Jasmine API differences** → Use Angular `TestBed` (framework-agnostic)
  and the `ng test` runner as configured; avoid runner-specific globals.
- **net10.0 test package versions** → Match test packages to the .NET 10 SDK
  (`Microsoft.AspNetCore.Mvc.Testing` 10.x, current xUnit).

## Migration Plan

Additive only. New test project + new/updated spec files. No production behavior
changes beyond the `public partial class Program {}` marker. Rollback = delete the
test project and revert the marker.
