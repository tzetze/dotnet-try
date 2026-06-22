using TaskManager.Api.Models;

namespace TaskManager.Api.Services;

// A concrete implementation that stores tasks in a list in memory.
// Data resets every time the app restarts — perfect for learning, no DB needed.
public class InMemoryTaskService : ITaskService
{
    private readonly List<TaskItem> _tasks = new();
    private int _nextId = 1;
    private readonly object _lock = new();

    public InMemoryTaskService()
    {
        // Seed a couple of tasks so the UI isn't empty on first run.
        Add(new TaskItem { Title = "Learn C# basics", Priority = Priority.High });
        Add(new TaskItem { Title = "Try an Angular component", Priority = Priority.Medium });
    }

    public IEnumerable<TaskItem> GetAll()
    {
        lock (_lock) return _tasks.ToList();
    }

    public TaskItem? GetById(int id)
    {
        lock (_lock) return _tasks.FirstOrDefault(t => t.Id == id);
    }

    public TaskItem Add(TaskItem task)
    {
        lock (_lock)
        {
            task.Id = _nextId++;
            _tasks.Add(task);
            return task;
        }
    }

    public bool Update(int id, TaskItem updated)
    {
        lock (_lock)
        {
            var existing = _tasks.FirstOrDefault(t => t.Id == id);
            if (existing is null) return false;

            existing.Title = updated.Title;
            existing.IsDone = updated.IsDone;
            existing.Priority = updated.Priority;
            return true;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            var existing = _tasks.FirstOrDefault(t => t.Id == id);
            if (existing is null) return false;
            return _tasks.Remove(existing);
        }
    }
}
