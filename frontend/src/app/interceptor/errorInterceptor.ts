import {HttpErrorResponse, HttpInterceptorFn} from '@angular/common/http';
import {catchError, throwError} from 'rxjs';
import {inject} from '@angular/core';
import {SnackbarService} from '../service/snackbar-service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  let snack = inject(SnackbarService)
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      snack.error(error.error)
      return throwError(() => error);
    })
  );
};
