import { Injectable } from '@angular/core';
import { StorageService } from './storageService';

@Injectable({
  providedIn: 'root'
})
export class AuthClientService {

  constructor(
    private storageService: StorageService
  ) { }

    isLoggedIn(): boolean {
    return !!this.storageService.get('token');
  }

  getUserEmail(): string | null {
    const token = this.storageService.get<string>('token');
      if (token) {
        try {
          const payload = JSON.parse(atob(token.split('.')[1]));
          return payload.email || null;
        } catch {
          return null;
        }
      }
      return null;    
  }

    getUserFullName(): string | null {
    const token = this.storageService.get<string>('token');
      if (token) {
        try {
          const payload = JSON.parse(atob(token.split('.')[1]));
          return payload.fullname || null;
        } catch {
          return null;
        }
      }
      return null;    
  }

  logout(): void {
    this.storageService.remove('token');
  }
}
