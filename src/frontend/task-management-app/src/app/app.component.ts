import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService, LoginRequest } from './shared/apiClient';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { ToastService } from './shared/services/toast.service';
import { LoginComponent } from "./auth/login/login.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, LoginComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  providers:[AuthService]
})
export class AppComponent {   

}
