import { Component, OnInit } from '@angular/core';
import {
  Router,
  RouterOutlet,
  RouterLink,
  RouterLinkActive,
} from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from './auth/service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, CommonModule],
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'expense-tracker';
  isAuthenticated = false;

  constructor(private router: Router, private authService: AuthService) {}

  ngOnInit(): void {
    // Check auth status on init
    this.authService.checkAuthStatus().subscribe({
      next: (response) => {
        this.isAuthenticated = response.authenticated;
        console.log('Auth status checked:', response);
      },
      error: (err) => {
        console.log('Auth status check failed:', err);
      },
    });
  }

  shouldNotShowNavbar(): boolean {
    return !this.router.url.includes('auth');
  }

  logout(): void {
    this.authService.logout();
  }
}
