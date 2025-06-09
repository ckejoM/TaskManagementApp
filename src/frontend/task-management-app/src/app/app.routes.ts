import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { CategoriesComponent } from './category/categories/categories.component';
import { TasksComponent } from './task/tasks/tasks.component';
import { ProjectsComponent } from './project/projects/projects.component';
import { authGuard } from './shared/guards/auth.guard';

export const routes: Routes = [
    {
        path: 'categories',
        component: CategoriesComponent,
        canActivate: [authGuard]
    },
    {
        path: 'tasks',
        component: TasksComponent,
        canActivate: [authGuard]
    },
    {
        path: 'projects',
        component: ProjectsComponent,
        canActivate: [authGuard]
    },
    {
        path: 'login',
        component: LoginComponent
    },
     {
        path: 'register',
        component: RegisterComponent
    },
     {
        path: '',
        component: LoginComponent
    },
];
