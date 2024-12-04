import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { AuthService } from './service';

@Injectable({
  providedIn: 'root',
})
export class LoginGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): Observable<boolean> {
    return this.authService.checkAuthStatus().pipe(
      tap((response) => {
        if (response.authenticated) {
          this.router.navigate(['/']);
        }
      }),
      map((response) => !response.authenticated),
      catchError(() => {
        return of(true);
      })
    );
  }
}
