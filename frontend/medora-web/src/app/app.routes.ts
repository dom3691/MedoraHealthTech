import { Routes } from '@angular/router';

import { ShellComponent } from './layout/shell/shell.component';
import { LoginComponent } from './pages/login/login.component';
import { ModulePageComponent } from './pages/module-page/module-page.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '',
    component: ShellComponent,
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
      { path: ':screenKey', component: ModulePageComponent }
    ]
  },
  { path: '**', redirectTo: 'dashboard' }
];
