import { Routes } from '@angular/router';

import { LoginComponent } from './features/auth/pages/login/login.component';
import { AppLayoutComponent } from './shared/components/layout/app-layout/app-layout.component';
import { ModuleScreenComponent } from './shared/components/module-screen/module-screen.component';

const moduleRoutes: Routes = [
  { path: 'dashboard', component: ModuleScreenComponent, data: { screenKey: 'dashboard' } },
  { path: 'patients', component: ModuleScreenComponent, data: { screenKey: 'patients' } },
  { path: 'appointments', component: ModuleScreenComponent, data: { screenKey: 'appointments' } },
  { path: 'patient-flow', component: ModuleScreenComponent, data: { screenKey: 'patient-flow' } },
  { path: 'outpatient', component: ModuleScreenComponent, data: { screenKey: 'outpatient' } },
  { path: 'inpatient', component: ModuleScreenComponent, data: { screenKey: 'inpatient' } },
  { path: 'emergency', component: ModuleScreenComponent, data: { screenKey: 'emergency' } },
  { path: 'wards-and-beds', component: ModuleScreenComponent, data: { screenKey: 'wards-and-beds' } },
  { path: 'theatre-and-surgery', component: ModuleScreenComponent, data: { screenKey: 'theatre-and-surgery' } },
  { path: 'nursing', component: ModuleScreenComponent, data: { screenKey: 'nursing' } },
  { path: 'lab-diagnostics', component: ModuleScreenComponent, data: { screenKey: 'lab-diagnostics' } },
  { path: 'radiology-imaging', component: ModuleScreenComponent, data: { screenKey: 'radiology-imaging' } },
  { path: 'pharmacy-inventory', component: ModuleScreenComponent, data: { screenKey: 'pharmacy-inventory' } },
  { path: 'doctor-portal', component: ModuleScreenComponent, data: { screenKey: 'doctor-portal' } },
  { path: 'patient-portal', component: ModuleScreenComponent, data: { screenKey: 'patient-portal' } },
  { path: 'telemedicine', component: ModuleScreenComponent, data: { screenKey: 'telemedicine' } },
  { path: 'procurement-vendors', component: ModuleScreenComponent, data: { screenKey: 'procurement-vendors' } },
  { path: 'notifications', component: ModuleScreenComponent, data: { screenKey: 'notifications' } },
  { path: 'staff', component: ModuleScreenComponent, data: { screenKey: 'staff' } },
  { path: 'departments', component: ModuleScreenComponent, data: { screenKey: 'departments' } },
  { path: 'billing-invoicing', component: ModuleScreenComponent, data: { screenKey: 'billing-invoicing' } },
  { path: 'hmo-insurance', component: ModuleScreenComponent, data: { screenKey: 'hmo-insurance' } },
  { path: 'reports-analytics', component: ModuleScreenComponent, data: { screenKey: 'reports-analytics' } },
  { path: 'admin-settings', component: ModuleScreenComponent, data: { screenKey: 'admin-settings' } },
  { path: 'audit-compliance', component: ModuleScreenComponent, data: { screenKey: 'audit-compliance' } }
];

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '',
    component: AppLayoutComponent,
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
      ...moduleRoutes
    ]
  },
  { path: '**', redirectTo: 'dashboard' }
];
