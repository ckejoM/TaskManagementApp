import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { TaskResponse, TasksService } from '../../shared/apiClient';
import { TaskDialogComponent } from '../task-dialog/task-dialog.component';
import { DatePipe, NgIf } from '@angular/common';

@Component({
  standalone: true,
  imports: [
    MatTableModule,
    MatButtonModule,
    MatDialogModule,
    MatProgressSpinnerModule,
    MatIconModule,
    DatePipe,
    NgIf
  ],
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.scss'],
})
export class TasksComponent implements OnInit {
  displayedColumns: string[] = ['title', 'projectId', 'categoryId', 'createdAt', 'actions'];
  tasks: TaskResponse[] = [];
  loading = true;

  constructor(
    private tasksService: TasksService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    this.loading = true;
    this.tasksService.getAll().subscribe({
      next: (tasks) => {
        this.tasks = tasks;
        this.loading = false;
      },
      error: () => (this.loading = false)
    });
  }

  openTaskDialog(task?: TaskResponse): void {
    const dialogRef = this.dialog.open(TaskDialogComponent, {
      width: '400px',
      data: task ? { ...task } : null
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        if (task && task.id) {
          this.tasksService.update(task.id, result).subscribe({
            next: () => this.loadTasks()
          });
        } else {
          this.tasksService.create(result).subscribe({
            next: () => this.loadTasks()
          });
        }
      }
    });
  }

  deleteTask(id: string): void {
    if (confirm('Are you sure you want to delete this task?')) {
      this.tasksService.delete(id).subscribe({
        next: () => this.loadTasks(),
        error: (err) => console.error('Delete error:', err)
      });
    }
  }
}