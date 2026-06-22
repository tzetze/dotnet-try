## ADDED Requirements

### Requirement: Backend service behavior is unit tested

The `InMemoryTaskService` SHALL have unit tests covering create, read, update,
and delete, including the description field being persisted on update and being
clearable.

#### Scenario: Service unit tests pass

- **WHEN** `dotnet test` is run
- **THEN** unit tests for add, get-by-id, update (title/priority/isDone/description),
  clearing the description, and delete all pass

### Requirement: API endpoints are integration tested

The task API SHALL have integration tests that exercise the real HTTP pipeline
via `WebApplicationFactory`, covering create/read/update round-trips and the
description length validation.

#### Scenario: Create and read back a description over HTTP

- **WHEN** an integration test POSTs a task with a description and GETs it back
- **THEN** the response includes the same description

#### Scenario: Oversized description is rejected over HTTP

- **WHEN** an integration test POSTs a task with a description longer than 500 characters
- **THEN** the API responds with HTTP 400 and does not store the task

### Requirement: Spec scenarios have acceptance tests

Each scenario in the `task-management` spec SHALL have a corresponding automated
test, so the spec is executable rather than verified by hand.

#### Scenario: Acceptance tests cover the task-management scenarios

- **WHEN** the test suite runs
- **THEN** there is a passing test for each task-management scenario (create with/without
  description, length validation, persist on update, clear on update, UI display,
  UI counter/limit)

### Requirement: Frontend description behavior is tested

The Angular `App` component SHALL have tests (via `ng test`) covering the
description input, the remaining-character counter, and conditional rendering of
a task's description. Existing stale tests SHALL be repaired.

#### Scenario: Frontend tests pass

- **WHEN** `npm test` (`ng test`) is run
- **THEN** the `App` spec passes, including a test that the counter reflects typed
  length and that a task's description renders only when present
