import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Task } from './task.model';

// @Injectable + 'providedIn: root' = this service is a singleton available
// anywhere in the app via dependency injection (Angular's version of the same
// DI idea you saw in the C# backend).
@Injectable({ providedIn: 'root' })
export class TaskService {
  // The base URL of our ASP.NET Core API.
  private readonly baseUrl = 'http://localhost:5059/api/tasks';

  // inject() is the modern way to get a dependency (instead of a constructor arg).
  private http = inject(HttpClient);

  // HttpClient methods return an Observable (an RxJS stream). The HTTP request
  // does NOT fire until something .subscribe()s to it.
  getAll(): Observable<Task[]> {
    return this.http.get<Task[]>(this.baseUrl);
  }

  create(task: Partial<Task>): Observable<Task> {
    return this.http.post<Task>(this.baseUrl, task);
  }

  update(id: number, task: Task): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, task);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
