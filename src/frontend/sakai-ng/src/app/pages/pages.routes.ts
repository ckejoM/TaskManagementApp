import { Routes } from '@angular/router';
import { Documentation } from './sakaiThemePages/documentation/documentation';
import { Crud } from './sakaiThemePages/crud/crud';
import { Empty } from './sakaiThemePages/empty/empty';

export default [
    { path: 'documentation', component: Documentation },
    { path: 'crud', component: Crud },
    { path: 'empty', component: Empty },
    { path: '**', redirectTo: '/notfound' }
] as Routes;
