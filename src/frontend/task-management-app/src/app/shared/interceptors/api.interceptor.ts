import { HttpErrorResponse, HttpInterceptorFn, HttpResponse } from '@angular/common/http';
   import { inject } from '@angular/core';
   import { catchError, switchMap, tap } from 'rxjs/operators';
   import { throwError, from } from 'rxjs';
   import { ToastService } from '../toast.service';

   export const apiInterceptor: HttpInterceptorFn = (req, next) => {
     const toast = inject(ToastService);

     return next(req).pipe(
       tap((event) => {
       }),
       catchError((error: any) => {
         if (error instanceof HttpErrorResponse && error.status === 400) {
           if (error.error instanceof Blob) {
             return from(error.error.text()).pipe(
               switchMap((text: string) => {
                 const errorObj = JSON.parse(text);
                 const message = errorObj.errors?.join(', ');
                 toast.showError(message);
                 return throwError(() => error);
               })
             );
           }
           const message = error.error?.errors?.join(', ');
           toast.showError(message);
         } else {
           toast.showError('Unexpected error occurred');
         }
         return throwError(() => error);
       })
     );
   };