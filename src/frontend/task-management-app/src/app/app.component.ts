import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService, LoginRequest } from './shared/apiClient';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { ToastService } from './shared/services/toast.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FormsModule, MatButtonModule, MatCardModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  providers:[AuthService]
})
export class AppComponent {   
  constructor(private authService: AuthService, private toastService: ToastService){

  }

  email = '';
  password = '';

  login(){
    let loginRequest = new LoginRequest();
    loginRequest.email = this.email;
    loginRequest.password = this.password;
    this.authService.login(loginRequest)
    .subscribe({    
      });
  }

  testSuccess(): void {
  this.toastService.showSuccess('Operation completed successfully!');
  }

  testError(): void {
    this.toastService.showError('Something went wrong!');
  }
}
