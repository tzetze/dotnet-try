## Context

The Task Manager is a learning project: an ASP.NET Core Web API
(`TaskItem` model, `ITaskService`/`InMemoryTaskService`, `TasksController`)
talking to an Angular frontend (`Task` model, `TaskService`, `App` component).
Tasks today carry `id`, `title`, `isDone`, and `priority`. The store is in
memory, so there is no database schema to migrate. This change adds an optional
short description that flows end to end through the existing layers.

## Goals / Non-Goals

**Goals:**
- Add an optional `description` to a task across model, service, API, and UI.
- Keep the API contract additive (no breaking change for existing clients).
- Validate the description length so it stays "short".

**Non-Goals:**
- Rich text / markdown rendering of the description.
- A dedicated edit form for existing tasks (current UI only adds and toggles).
- Persisting to a real database.

## Decisions

- **Field type and default.** Use `string Description { get; set; } = string.Empty;`
  on the C# model and `description?: string` on the TS model.
  - *Rationale:* matches the existing `Title` pattern (non-null, empty default),
    so "no description" is just an empty string rather than null handling
    sprinkled across layers.
  - *Alternative considered:* nullable `string?`. Rejected to stay consistent
    with `Title` and avoid null checks in the UI.

- **Validation via data annotation.** Add `[MaxLength(500)]` (and rely on
  `[ApiController]` automatic 400 responses) on `Description`.
  - *Rationale:* `[ApiController]` already performs automatic model validation,
    so this is the lowest-effort enforcement of the 500-char cap from the spec.
  - *Alternative considered:* manual checks in the service. Rejected — duplicates
    what the framework gives for free and splits validation logic.

- **Update path.** Extend `InMemoryTaskService.Update` to copy
  `existing.Description = updated.Description;` alongside the other fields.
  - *Rationale:* the service replaces fields on update; description must follow
    the same rule so it can be changed or cleared.

- **Frontend wiring.** `TaskService.create/update` already send the whole task
  object, so no service change is needed. Add a `newDescription` signal and an
  input to `App`/`app.html`, include it in the `create()` payload, and render
  `task.description` conditionally in the list.
  - *Rationale:* minimal, follows the existing signal-based pattern for
    `newTitle`/`newPriority`.

- **Client-side length hint.** Set `maxlength="500"` on the description input so
  the browser prevents over-long entries, and show a live remaining-character
  count (e.g. `123 left`) in the corner of the input. Define `maxDescriptionLength = 500`
  as a constant on the component and derive the remaining count from the
  `newDescription` signal.
  - *Rationale:* surfaces the limit before the user hits it instead of relying on
    a server 400. The server stays the source of truth (the spec's validation
    requirement is unchanged); this is purely a convenience.
  - *Alternative considered:* no client limit, rely solely on the server error.
    Rejected — worse UX; the user only learns the limit after a failed submit.

## Risks / Trade-offs

- **Empty string vs. null semantics** → Standardize on empty string everywhere;
  the UI only renders the description block when the string is non-empty.
- **Client-side limit drifts from server limit** → Both use 500; keep them in
  sync (a single `maxDescriptionLength` constant on the client, `[MaxLength(500)]`
  on the server). The server remains the source of truth — `maxlength` and the
  counter are convenience only.
- **No edit-description UI for existing tasks** → Accepted as a non-goal; the
  backend already supports it via PUT for when an edit form is added later.

## Migration Plan

No data migration: the store is in memory and resets on restart. The change is
additive, so existing create/update calls that omit `description` continue to
work and default to empty.
