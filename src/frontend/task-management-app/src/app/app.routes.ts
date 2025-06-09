import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { CategoriesComponent } from './category/categories/categories.component';
import { TasksComponent } from './task/tasks/tasks.component';
import { ProjectsComponent } from './project/projects/projects.component';

export const routes: Routes = [
    {
        path: 'categories',
        component: CategoriesComponent
    },
    {
        path: 'tasks',
        component: TasksComponent
    },
    {
        path: 'projects',
        component: ProjectsComponent
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
