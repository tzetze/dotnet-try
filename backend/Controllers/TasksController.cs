using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Models;
using TaskManager.Api.Services;

namespace TaskManager.Api.Controllers;

// [ApiController] turns on helpful API behaviors (automatic model validation,
// JSON binding, etc). [Route] sets the URL prefix — here "/api/tasks".
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _service;

    // The framework injects ITaskService here (constructor injection).
    // We registered which concrete class to use in Program.cs.
    public TasksController(ITaskService service)
    {
        _service = service;
    }

    // GET /api/tasks
    [HttpGet]
    public ActionResult<IEnumerable<TaskItem>> GetAll()
    {
        return Ok(_service.GetAll());
    }

    // GET /api/tasks/5
    [HttpGet("{id:int}")]
    public ActionResult<TaskItem> GetById(int id)
    {
        var task = _service.GetById(id);
        return task is null ? NotFound() : Ok(task);
    }

    // POST /api/tasks  (body = JSON task)
    [HttpPost]
    public ActionResult<TaskItem> Create(TaskItem task)
    {
        var created = _service.Add(task);
        // 201 Created + a Location header pointing at the new resource.
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT /api/tasks/5  (replace an existing task)
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, TaskItem task)
    {
        return _service.Update(id, task) ? NoContent() : NotFound();
    }

    // DELETE /api/tasks/5
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        return _service.Delete(id) ? NoContent() : NotFound();
    }
}
