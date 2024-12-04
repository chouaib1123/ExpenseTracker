import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { BehaviorSubject, catchError, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient, private router: Router) {}

  login(username: string, password: string) {
    return this.http
      .post(
        'http://localhost:5251/api/Login',
        { username, password },
        { withCredentials: true, observe: 'response' }
      )
      .subscribe({
        next: (res) => {
          console.log('Login response:', res);
          this.router.navigate(['/']);
        },
        error: (err) => {
          console.log('Login failed', err);
        },
      });
  }

  logout() {
    this.http
      .post('http://localhost:5251/api/Login/logout', null, {
        withCredentials: true,
      })
      .subscribe({
        next: (res) => {
          console.log('Logout response:', res);
          this.router.navigate(['/auth/login']);
        },
        error: (err) => {
          console.log('Logout failed', err);
        },
      });
  }

  checkAuthStatus() {
    return this.http
      .get<{ authenticated: boolean }>(
        'http://localhost:5251/api/Login/check-auth',
        { withCredentials: true }
      )
      .pipe(
        catchError(() => {
          return of({ authenticated: false });
        })
      );
  }
}
