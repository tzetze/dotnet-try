# Task Manager — C# / .NET + Angular learning project

A full-stack CRUD app: an **ASP.NET Core Web API** (C#) backend and an **Angular**
frontend that talks to it over HTTP. No database — tasks live in memory and reset
on restart, so you can focus on the concepts.

```
dotnet-try/
├── TaskManager.sln          # .NET "solution" — groups projects together
├── backend/                 # ASP.NET Core Web API (C#)
│   ├── Program.cs           # App startup: DI registration, CORS, pipeline
│   ├── Models/TaskItem.cs   # The data model (a plain C# class)
│   ├── Services/            # Business logic + storage (interface + impl)
│   └── Controllers/         # HTTP endpoints (the REST API)
└── frontend/                # Angular app (TypeScript)
    └── src/app/
        ├── task.model.ts    # TypeScript interface mirroring TaskItem
        ├── task.service.ts  # Calls the API with HttpClient
        ├── app.ts           # Component logic (state + actions)
        └── app.html         # Template (what the user sees)
```

## How to run it

Open two terminals.

**Backend** (http://localhost:5059):
```bash
cd backend
dotnet run --launch-profile http
```

**Frontend** (http://localhost:4200):
```bash
cd frontend
npm start
```

Then open http://localhost:4200 in your browser.

## How the two talk

```
Browser (Angular @ :4200)  ──HTTP/JSON──>  ASP.NET Core API (@ :5059)
   TaskService.getAll()    ── GET /api/tasks ──>   TasksController.GetAll()
```

CORS in `Program.cs` is what allows the browser at :4200 to call the API at :5059.

## Concepts worth knowing for the interview

**C# / .NET**
- **Controller** — a class whose methods handle HTTP requests (`[HttpGet]`, `[HttpPost]`…).
- **Dependency Injection (DI)** — you register services in `Program.cs`
  (`AddSingleton<ITaskService, InMemoryTaskService>()`); the framework hands them
  to constructors automatically. Depend on the *interface*, not the concrete class.
- **Model / POCO** — a plain class holding data; auto-serialized to JSON.
- **Middleware pipeline** — the ordered `app.UseXxx()` calls in `Program.cs`.
- **`IActionResult` / `ActionResult<T>`** — return types that carry HTTP status
  codes (`Ok`, `NotFound`, `CreatedAtAction`, `NoContent`).

**Angular**
- **Component** — a class (`app.ts`) + template (`app.html`) + styles.
- **Service** — reusable logic (here, HTTP calls), shared via DI (`inject()`).
- **HttpClient** — makes HTTP calls; returns **Observables** (RxJS streams) that
  only run when you `.subscribe()`.
- **Signals** — reactive state (`signal()`, `.set()`, `.update()`); the template
  re-renders when they change.
- **Data binding** — `{{ }}` interpolation, `[(ngModel)]` two-way binding,
  `(click)` event binding, `[class.done]` property binding.
- **Control flow** — `@if`, `@for` in templates; `ngOnInit` lifecycle hook.

## Things to try (great for learning)
- Add a `description` field: update `TaskItem.cs`, `task.model.ts`, the form, and the list.
- Add a "clear completed" button that deletes all done tasks.
- Filter the list by priority.
- Swap the in-memory store for a real database (Entity Framework Core) — the
  controller wouldn't change at all, only the service. That's DI paying off.
