import { Injectable } from '@angular/core';

import type { AuthSession } from './app-context.service';

const SESSION_STORAGE_KEY = 'medora.session';

@Injectable({ providedIn: 'root' })
export class AuthStorageService {
  loadSession(): AuthSession | null {
    const raw = localStorage.getItem(SESSION_STORAGE_KEY);
    if (!raw) {
      return null;
    }

    try {
      return JSON.parse(raw) as AuthSession;
    } catch {
      this.clearSession();
      return null;
    }
  }

  saveSession(session: AuthSession): void {
    localStorage.setItem(SESSION_STORAGE_KEY, JSON.stringify(session));
  }

  clearSession(): void {
    localStorage.removeItem(SESSION_STORAGE_KEY);
  }
}
