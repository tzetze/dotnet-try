import { Component, inject, signal, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TaskService } from './task.service';
import { Task, Priority } from './task.model';

@Component({
  selector: 'app-root',
  // FormsModule gives us [(ngModel)] two-way binding for the input fields.
  imports: [FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private taskService = inject(TaskService);

  // Signals hold reactive state. When a signal's value changes, the template
  // that reads it re-renders automatically.
  tasks = signal<Task[]>([]);
  newTitle = signal('');
  newPriority = signal<Priority>('Medium');

  // ngOnInit is a lifecycle hook: Angular calls it once after the component
  // is created — a good place to load initial data.
  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    // subscribe() fires the HTTP GET; the callback runs when the response arrives.
    this.taskService.getAll().subscribe(tasks => this.tasks.set(tasks));
  }

  addTask(): void {
    const title = this.newTitle().trim();
    if (!title) return;

    this.taskService
      .create({ title, priority: this.newPriority(), isDone: false })
      .subscribe(created => {
        // Append the created task (with its server-assigned id) to the list.
        this.tasks.update(list => [...list, created]);
        this.newTitle.set('');
      });
  }

  toggleDone(task: Task): void {
    const updated: Task = { ...task, isDone: !task.isDone };
    this.taskService.update(task.id, updated).subscribe(() => {
      this.tasks.update(list => list.map(t => (t.id === task.id ? updated : t)));
    });
  }

  deleteTask(task: Task): void {
    this.taskService.delete(task.id).subscribe(() => {
      this.tasks.update(list => list.filter(t => t.id !== task.id));
    });
  }
}
