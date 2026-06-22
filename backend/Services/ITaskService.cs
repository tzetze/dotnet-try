using TaskManager.Api.Models;

namespace TaskManager.Api.Services;

// An interface defines a contract. The controller depends on this interface,
// not the concrete class — that's "dependency injection" and it makes code
// easy to test and swap (e.g. replace the in-memory store with a database later).
public interface ITaskService
{
    IEnumerable<TaskItem> GetAll();
    TaskItem? GetById(int id);
    TaskItem Add(TaskItem task);
    bool Update(int id, TaskItem task);
    bool Delete(int id);
}
