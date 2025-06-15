import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthClientService } from '../services/auth-client-service.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthClientService);
  const router = inject(Router);

  if(authService.isLoggedIn()){
    return true;
  }

  router.navigate(['/login'])
  return false;
};
