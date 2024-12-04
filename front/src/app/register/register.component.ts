import { Component, OnInit } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClient } from '@angular/common/http';

import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';

import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    RouterLink,
    RouterLinkActive,
    ReactiveFormsModule,
    CommonModule,
    HttpClientModule,
  ],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) {}

  ngOnInit() {
    this.registerForm = this.fb.group(
      {
        username: ['', Validators.required],
        password: ['', [Validators.required, Validators.minLength(6)]],
        Cpassword: ['', [Validators.required]],
      },
      { validators: this.passWordMatchValidator }
    );
  }

  passWordMatchValidator(
    formGroup: FormGroup
  ): { [key: string]: boolean } | null {
    return formGroup.get('password')?.value ===
      formGroup.get('Cpassword')?.value
      ? null
      : { mismatch: true };
  }

  onSubmit() {
    if (this.registerForm.valid) {
      const formData = {
        username: this.registerForm.get('username')?.value,
        password: this.registerForm.get('password')?.value,
      };

      this.http.post('http://localhost:5251/api/Register', formData).subscribe({
        next: (res) => {
          console.log('Registration successful', res);
          this.router.navigate(['/auth/login']);
        },
        error: (err) => {
          console.log('Registration failed', err);
        },
      });
    }
  }
}
