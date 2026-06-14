import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import {
  AppContextService,
  TenantContext,
  TenantOnboardingRequest,
  TenantOnboardingResult
} from '../../../shared/services/app-context.service';

@Injectable({ providedIn: 'root' })
export class TenantService {
  constructor(private readonly appContext: AppContextService) {}

  getTenants(): Observable<TenantContext[]> {
    return this.appContext.loadTenants();
  }

  onboardTenant(request: TenantOnboardingRequest): Observable<TenantOnboardingResult> {
    return this.appContext.onboardTenant(request);
  }
}
