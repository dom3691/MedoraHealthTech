import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { AppContextService, AuthSession, LoginRequest } from '../../../shared/services/app-context.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(private readonly appContext: AppContextService) {}

  login(request: LoginRequest): Observable<AuthSession> {
    return this.appContext.login(request);
  }

  logout(): void {
    this.appContext.signOut();
  }

  currentSession(): AuthSession | null {
    return this.appContext.session();
  }
}
