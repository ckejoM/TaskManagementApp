import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService, LoginRequest } from './shared/apiClient';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  providers:[AuthService]
})
export class AppComponent {
  constructor(private authService: AuthService){

  }

  email = '';
  password = '';

  login(){
    let loginRequest = new LoginRequest();
    loginRequest.email = this.email;
    loginRequest.password = this.password;
    this.authService.login(loginRequest)
    .subscribe(data => {    
        console.log(data);  
      }
    );
  }
}
