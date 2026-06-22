namespace TaskManager.Api.Models;

// A plain "model" class (POCO = Plain Old CLR Object). It just holds data.
// ASP.NET Core automatically serializes this to/from JSON when it crosses HTTP.
public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsDone { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
}

// An enum is serialized as a number by default; we configure JSON to use the
// string name in Program.cs so the Angular side sees "Low"/"Medium"/"High".
public enum Priority
{
    Low,
    Medium,
    High
}
