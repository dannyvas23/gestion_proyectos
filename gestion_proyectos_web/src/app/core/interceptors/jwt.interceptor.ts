import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

/**
 * Interceptor 
 * - HttpInterceptorFn es el nuevo patrón de Angular 17 (reemplaza a las clases con implements HttpInterceptor).
 * - Si el servidor responde 401 (no autorizado), el interceptor llama logout() automáticamente
 */
export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const token = authService.obtenerToken();

  // Si hay token, clonar la petición y agregar el header
  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        // Token expirado o inválido → cerrar sesión
        authService.logout();
      }
      return throwError(() => error);
    })
  );
};
