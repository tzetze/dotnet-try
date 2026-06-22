// A TypeScript interface describing the shape of a task.
// It mirrors the C# TaskItem class on the backend. TypeScript uses this only
// at compile time for type-checking — it disappears in the compiled JS.
export type Priority = 'Low' | 'Medium' | 'High';

export interface Task {
  id: number;
  title: string;
  isDone: boolean;
  priority: Priority;
  description?: string;
}
