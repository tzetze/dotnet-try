## Why

A task currently only has a title, which often isn't enough to capture what the
task actually involves. Letting users attach a short, optional description gives
them room to add context (notes, acceptance criteria, links) without cluttering
the title.

## What Changes

- Add an optional `description` field to a task (free text, capped at a sensible
  length, e.g. 500 characters).
- Persist the description when a task is created and updated.
- Return the description from the API alongside the other task fields.
- Surface the description in the Angular UI: an optional input when adding a
  task, and display of the description for tasks that have one.
- Existing tasks (and create requests that omit the field) remain valid — the
  description defaults to empty/none. No breaking change to the API contract.

## Capabilities

### New Capabilities
- `task-management`: Creating, reading, updating, and deleting tasks, including
  their title, completion state, priority, and now an optional short description.

### Modified Capabilities
<!-- None — no existing specs in openspec/specs/ yet. -->

## Impact

- **Backend**: `TaskItem` model gains a `Description` property;
  `InMemoryTaskService.Update` copies it; the `TasksController` create/update
  endpoints accept and return it.
- **Frontend**: `Task` model gains `description`; `task.service.ts` already
  passes the whole object so needs no change; `app.ts` add-task flow captures
  the description; `app.html` renders an input and the description text.
- **API contract**: additive only — the new field is optional.
- **Data**: in-memory store; no migrations needed.
