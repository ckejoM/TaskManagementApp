import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { RippleModule } from 'primeng/ripple';
import { AppFloatingConfigurator } from '../../layout/component/app.floatingconfigurator';
import { AuthService, LoginRequest } from '../../shared/apiClient';
import { StorageService } from '../../shared/services/storageService';
import { ToastService } from '../../shared/services/toast.service';

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [ButtonModule, CheckboxModule, InputTextModule, PasswordModule, FormsModule, RouterModule, RippleModule, AppFloatingConfigurator, ReactiveFormsModule, FormsModule],
    template: `
        <app-floating-configurator />
        <div class="bg-surface-50 dark:bg-surface-950 flex items-center justify-center min-h-screen min-w-[100vw] overflow-hidden">
            <div class="flex flex-col items-center justify-center">
                <div style="border-radius: 56px; padding: 0.3rem; background: linear-gradient(180deg, var(--primary-color) 10%, rgba(33, 150, 243, 0) 30%)">
                    <div class="w-full bg-surface-0 dark:bg-surface-900 py-20 px-8 sm:px-20" style="border-radius: 53px">
                        <div class="text-center mb-8">
                             <img class="mb-8 w-24 h-24 rounded-full object-cover mx-auto" src="/assets/images/taskify-logo.png"/>
                            <div class="text-surface-900 dark:text-surface-0 text-3xl font-medium mb-4">Welcome to Taskify!</div>
                            <span class="text-muted-color font-medium">Sign in to continue</span>
                        </div>

                        <div>
                            <form [formGroup]="loginForm" (ngSubmit)="onSubmit()">
                                <label for="email1" class="block text-surface-900 dark:text-surface-0 text-xl font-medium mb-2">Email</label>
                                <input pInputText formControlName="email" id="email" type="text" placeholder="Email address" class="w-full md:w-[30rem] mb-8" />
                                
                                <label for="password1" class="block text-surface-900 dark:text-surface-0 font-medium text-xl mb-2">Password</label>
                                <p-password id="password" formControlName="password"  placeholder="Password" [toggleMask]="true" styleClass="mb-4" [fluid]="true" [feedback]="false"></p-password>
                                
                                <div class="flex items-center justify-between mt-2 mb-8 gap-8">
                                    <span class="font-medium no-underline ml-2 text-right cursor-pointer text-primary">Forgot password?</span>
                                    <span class="font-medium no-underline ml-2 text-right cursor-pointer text-primary"> 
                                        Don't have an account?
                                        <a routerLink="/auth/register">Register here </a>
                                     </span>
                                </div>
                                <p-button label="Sign In" type="submit" styleClass="w-full"></p-button>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `
})
export class Login {
    constructor(
        private formBuilder: FormBuilder,
        private authService: AuthService,
        private localStorageService: StorageService,
        private router: Router,
        private toastService: ToastService
    ) {               
        this.loginForm = this.formBuilder.group({
            email: ['', [Validators.required, Validators.email]],
            password: ['', Validators.required]
        });
    }

    loginForm: FormGroup;

    onSubmit(){
        if (this.loginForm.valid) {
            const request: LoginRequest = this.loginForm.value;
            this.authService.login(request).subscribe({
            next: (response) => {
                this.localStorageService.set('token', response.token);
                this.toastService.showSuccess('Sucessfully logged in');
                this.router.navigate(['/']);
            },
            error: () => {}
            });
        }
    }
}
