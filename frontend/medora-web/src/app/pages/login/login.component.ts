import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';

import { AppContextService, TenantContext } from '../../core/app-context.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  private readonly appContext = inject(AppContextService);
  private readonly router = inject(Router);

  readonly tenants = this.appContext.tenants;
  readonly application = this.appContext.application;
  readonly isLoading = signal(true);
  readonly errorMessage = signal<string | null>(null);
  readonly selectedTenantCode = signal<string | null>(null);

  email = '';
  password = '';

  readonly selectedTenant = computed(() =>
    this.tenants().find((tenant) => tenant.code === this.selectedTenantCode()) ?? null
  );

  ngOnInit(): void {
    forkJoin({
      application: this.appContext.loadApplication(),
      tenants: this.appContext.loadTenants()
    }).subscribe({
      next: ({ tenants }) => {
        this.selectedTenantCode.set(this.appContext.session()?.tenant.code ?? tenants.at(0)?.code ?? null);
        this.isLoading.set(false);
      },
      error: () => {
        this.errorMessage.set('Unable to load hospital tenants from the API.');
        this.isLoading.set(false);
      }
    });
  }

  selectTenant(tenant: TenantContext): void {
    this.selectedTenantCode.set(tenant.code);
  }

  signIn(): void {
    const tenantCode = this.selectedTenantCode();
    if (!tenantCode || !this.email || !this.password) {
      this.errorMessage.set('Select a hospital and enter your credentials.');
      return;
    }

    this.errorMessage.set(null);
    this.appContext.login({ tenantCode, email: this.email, password: this.password }).subscribe({
      next: () => this.router.navigateByUrl('/dashboard'),
      error: () => this.errorMessage.set('The hospital, email, or password is incorrect.')
    });
  }
}

