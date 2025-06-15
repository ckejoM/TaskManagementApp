import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  constructor(private messageService: MessageService) {}

  showSuccess(message: string): void {
    this.messageService.add({ severity: 'success', summary: 'Success message', detail: message, life: 3000});
  }

  showError(message: string): void {
    this.messageService.add({ severity: 'error', summary: 'Error', detail: message, life: 5000});
  }
}