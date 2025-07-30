import {CanActivateFn, Router} from '@angular/router';
import {inject} from '@angular/core';
import { UserService } from '../service/user-service';

export const authGuard: CanActivateFn = (route, state) => {
  const token = localStorage.getItem('token');
  let userService = inject(UserService)
  let router = inject(Router)
  if (userService.getTokenData() === undefined) {
    router.navigate(['/']);

    return false;
  }
  return true;
};
