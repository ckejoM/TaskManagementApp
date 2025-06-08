import { Component } from '@angular/core';
   import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
   import { MatButtonModule } from '@angular/material/button';
   import { MatCardModule } from '@angular/material/card';
   import { MatFormFieldModule } from '@angular/material/form-field';
   import { MatInputModule } from '@angular/material/input';
   import { AuthService, LoginRequest, TasksService } from '../../shared/apiClient';
import { StorageService } from '../../shared/services/storageService';
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
     selector: 'app-login',
     templateUrl: './login.component.html',
     styleUrls: ['./login.component.scss']
   })
   export class LoginComponent {
     loginForm: FormGroup;

     constructor(
       private fb: FormBuilder,
       private authService: AuthService,
       private localStorageService: StorageService,
       private taskService: TasksService
     ) {
       this.loginForm = this.fb.group({
         email: ['', [Validators.required, Validators.email]],
         password: ['', [Validators.required]]
       });
     }

     onSubmit(): void {
       if (this.loginForm.valid) {
         const request: LoginRequest = this.loginForm.value;
         this.authService.login(request).subscribe({
           next: (response) => {
             this.localStorageService.set('token', response.token);
           },
           error: () => {}
         });
       }
     }
   }