import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AuthService, RegisterRequest } from '../../shared/apiClient';
import { Router } from '@angular/router';
import { ToastService } from '../../shared/services/toast.service';
import { NgIf } from '@angular/common';

@Component({
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    NgIf
  ],
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  registerForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private toastService: ToastService,
    private router: Router
  ) {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      firstName: [''],
      lastName: ['']
    });
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      const request: RegisterRequest = this.registerForm.value;
      this.authService.register(request).subscribe({
        next: (response) => {
          this.toastService.showSuccess('Sucessfully registered!');
          this.router.navigate(['/login']);
        },
        error: () => {}
      });
    }
  }
}