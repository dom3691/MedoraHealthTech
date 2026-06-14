import { Injectable, inject, signal } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Observable, shareReplay, tap } from 'rxjs';

import { ApiClientService } from './api-client.service';
import { AuthStorageService } from './auth-storage.service';

export type TenantContext = {
  id: string;
  code: string;
  name: string;
  location?: string | null;
  licensedBeds: number;
  plan: string;
};

export type UserContext = {
  id: string;
  fullName: string;
  email: string;
  roleCodes: string[];
  roleNames: string[];
};

export type AuthSession = {
  tenant: TenantContext;
  user: UserContext;
  token: string;
};

export type LoginRequest = {
  tenantCode: string;
  email: string;
  password: string;
};

export type ApplicationConfig = {
  productName: string;
  productSuite: string;
  documentTitle: string;
  loginHeroTitle: string;
  loginHeroSubtitle: string;
  loginComplianceText: string;
  globalSearchPlaceholder: string;
};

export type TenantOnboardingRequest = {
  name: string;
  code: string;
  location?: string | null;
  licensedBeds: number;
  plan: string;
  adminFullName: string;
  adminEmail: string;
  includeDemoOperationalData: boolean;
};

export type TenantOnboardingResult = {
  tenantId: string;
  tenantCode: string;
  tenantName: string;
  adminUserId: string;
  adminEmail: string;
  temporaryPassword: string;
  includedDemoOperationalData: boolean;
};

export type AssignableRole = {
  code: string;
  name: string;
  description: string;
};

export type UserOnboardingRequest = {
  fullName: string;
  email: string;
  roleCodes: string[];
  employeeNumber?: string | null;
  department?: string | null;
  jobTitle?: string | null;
  employmentType?: string | null;
  startDate?: string | null;
};

export type UserOnboardingResult = {
  userId: string;
  staffMemberId: string;
  employeeNumber: string;
  email: string;
  temporaryPassword: string;
  roles: AssignableRole[];
};

@Injectable({ providedIn: 'root' })
export class AppContextService {
  private readonly authStorage = inject(AuthStorageService);
  private applicationRequest$?: Observable<ApplicationConfig>;

  readonly application = signal<ApplicationConfig | null>(null);
  readonly tenants = signal<TenantContext[]>([]);
  readonly session = signal<AuthSession | null>(this.authStorage.loadSession());

  constructor(
    private readonly api: ApiClientService,
    private readonly title: Title
  ) {}

  loadApplication(): Observable<ApplicationConfig> {
    if (!this.applicationRequest$) {
      this.applicationRequest$ = this.api.get<ApplicationConfig>('/api/app/config').pipe(
        tap((config) => {
          this.application.set(config);
          this.title.setTitle(config.documentTitle);
        }),
        shareReplay(1)
      );
    }

    return this.applicationRequest$;
  }

  loadTenants() {
    return this.api.get<TenantContext[]>('/api/operations/tenants').pipe(
      tap((tenants) => this.tenants.set(tenants))
    );
  }

  login(request: LoginRequest) {
    return this.api.post<AuthSession>('/api/auth/login', request).pipe(
      tap((session) => {
        this.session.set(session);
        this.authStorage.saveSession(session);
      })
    );
  }

  onboardTenant(request: TenantOnboardingRequest): Observable<TenantOnboardingResult> {
    return this.api.post<TenantOnboardingResult>('/api/admin/tenants', request).pipe(
      tap(() => {
        this.loadTenants().subscribe();
      })
    );
  }

  loadAssignableRoles(): Observable<AssignableRole[]> {
    return this.api.get<AssignableRole[]>('/api/admin/users/roles');
  }

  onboardUser(request: UserOnboardingRequest): Observable<UserOnboardingResult> {
    return this.api.post<UserOnboardingResult>('/api/admin/users', request);
  }

  signOut(): void {
    this.session.set(null);
    this.authStorage.clearSession();
  }

  activeTenantCode(): string | null {
    return this.session()?.tenant.code ?? null;
  }

  hasRole(...roleCodes: string[]): boolean {
    const activeRoles = this.session()?.user.roleCodes ?? [];
    return roleCodes.some((roleCode) => activeRoles.includes(roleCode));
  }

}
