import { HttpInterceptorFn } from '@angular/common/http';
   import { inject } from '@angular/core';
   import { StorageService } from '../services/storageService';
   import { catchError, throwError } from 'rxjs';
   import { Router } from '@angular/router';
   import { ToastService } from '../services/toast.service';

   export const authInterceptor: HttpInterceptorFn = (req, next) => {
     const localStorageService = inject(StorageService);
     const router = inject(Router);
     const toast = inject(ToastService);

     const token = localStorageService.get('token');
     let authReq = req;

     if (token && !req.url.includes('/api/Auth/login')) {
       authReq = req.clone({
         setHeaders: {
           Authorization: `Bearer ${token}`
         }
       });
     }

     return next(authReq).pipe(
       catchError((error) => {
         if (error.status === 401) {
           localStorageService.remove('token');
           toast.showError('Session expired. Please log in again.');
           router.navigate(['/auth/login']);
         }
         return throwError(() => error);
       })
     );
   };