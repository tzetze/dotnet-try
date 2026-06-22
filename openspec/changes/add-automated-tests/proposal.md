## Why

The task-management behaviors — including the new optional description — are
currently verified by hand (curl + clicking through the UI). Manual checks don't
re-run on change and rot quickly (the existing `app.spec.ts` still asserts the
old "Hello, frontend" title and would fail). Automated tests turn the spec
scenarios into a regression safety net that runs on demand.

## What Changes

- Add a backend xUnit test project (`TaskManager.Api.Tests`) with:
  - **Unit tests** for `InMemoryTaskService` (add, get, update incl. description,
    clear description, delete).
  - **Integration tests** for the API via `WebApplicationFactory` (POST/PUT/GET
    round-trips, the 500-char validation → 400).
- Add **acceptance tests** that map directly to the `task-management` spec
  scenarios (one test per scenario) so the spec is executable.
- Fix and extend the frontend tests (vitest via `ng test`): repair the stale
  `App` spec and cover the description input, character counter, and conditional
  rendering of a task's description.
- Register the new test project in `TaskManager.slnx`.
- Minor production touch: expose `Program` as a `public partial class` so the
  integration test host can reference it (no behavior change).

## Capabilities

### New Capabilities
- `automated-test-coverage`: The task-management behaviors SHALL be covered by
  automated unit, integration, and acceptance tests that can be run on demand.

### Modified Capabilities
<!-- None — task-management behavior is unchanged; tests only verify it. -->

## Impact

- **Backend**: new `backend.Tests` project (xUnit, `Microsoft.AspNetCore.Mvc.Testing`);
  `Program.cs` gains a `public partial class Program {}` marker; `TaskManager.slnx`
  references the test project.
- **Frontend**: `app.spec.ts` repaired and expanded; possibly a small testing
  helper for `HttpClient` (provideHttpClientTesting).
- **CI/tooling**: tests run via `dotnet test` and `npm test` (`ng test`).
- **No runtime/API behavior change.**
