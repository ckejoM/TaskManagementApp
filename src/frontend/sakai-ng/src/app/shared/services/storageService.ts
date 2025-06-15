import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class StorageService {
  // Get item with type safety
  get<T>(key: string): T | null {
    try {
      const item = localStorage.getItem(key);
      return item ? JSON.parse(item) : null;
    } catch (error) {
      console.error('Error accessing localStorage', error);
      return null;
    }
  }

  // Set item
  set(key: string, value: any): void {
    try {
      localStorage.setItem(key, JSON.stringify(value));
    } catch (error) {
      console.error('Error saving to localStorage', error);
    }
  }

  // Remove item
  remove(key: string): void {
    localStorage.removeItem(key);
  }

  // Clear all
  clear(): void {
    localStorage.clear();
  }
}