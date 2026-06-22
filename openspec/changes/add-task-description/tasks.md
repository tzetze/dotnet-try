## 1. Backend model & validation

- [ ] 1.1 Add `public string Description { get; set; } = string.Empty;` to `TaskItem` in `backend/Models/TaskItem.cs`
- [ ] 1.2 Annotate `Description` with `[MaxLength(500)]` (add `using System.ComponentModel.DataAnnotations;`)

## 2. Backend persistence

- [ ] 2.1 In `InMemoryTaskService.Update`, copy `existing.Description = updated.Description;` alongside the other fields
- [ ] 2.2 (Optional) Add a description to one of the seeded tasks in the `InMemoryTaskService` constructor

## 3. Backend verification

- [ ] 3.1 Build the backend (`dotnet build`) and confirm it compiles
- [ ] 3.2 Verify POST `/api/tasks` with a `description` returns it; PUT updates it; description >500 chars returns 400

## 4. Frontend model & state

- [ ] 4.1 Add `description?: string;` to the `Task` interface in `frontend/src/app/task.model.ts`
- [ ] 4.2 Add a `newDescription = signal('')` to the `App` component and include `description` in the `create({...})` payload; reset it after add
- [ ] 4.3 Add a `maxDescriptionLength = 500` constant and a computed/derived remaining count (`maxDescriptionLength - newDescription().length`)

## 5. Frontend UI

- [ ] 5.1 Add a description `<input>` (bound with `[(ngModel)]="newDescription"`, `name="description"`, `maxlength="500"`) to the add-task form in `app.html`
- [ ] 5.2 Show the remaining-character count in the corner of the description input (e.g. `{{ maxDescriptionLength - newDescription().length }} left`)
- [ ] 5.3 In the task list, render `task.description` (e.g. `@if (task.description) { ... }`) below the title

## 6. End-to-end verification

- [ ] 6.1 Run frontend (`npm start`) + backend, add a task with a description, confirm it shows in the list
- [ ] 6.2 Add a task without a description and confirm no description text/empty block is shown
