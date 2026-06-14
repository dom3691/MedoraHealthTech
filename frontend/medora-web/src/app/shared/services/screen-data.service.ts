import { Injectable, signal } from '@angular/core';
import { map, Observable, shareReplay, tap } from 'rxjs';

import { ApiClientService } from './api-client.service';
import { AppContextService } from './app-context.service';
import { ScreenCatalogResponse, ScreenDefinition } from '../types/screen';

@Injectable({ providedIn: 'root' })
export class ScreenDataService {
  private readonly endpoint = '/api/operations/screens';
  readonly catalog = signal<ScreenCatalogResponse | null>(null);

  constructor(
    private readonly api: ApiClientService,
    private readonly appContext: AppContextService
  ) {}

  getCatalog(): Observable<ScreenCatalogResponse> {
    const tenantCode = this.appContext.activeTenantCode();

    return this.api.get<ScreenCatalogResponse>(this.endpoint, tenantCode ? { tenantCode } : undefined).pipe(
      tap((catalog) => this.catalog.set(catalog)),
      shareReplay({ bufferSize: 1, refCount: true })
    );
  }

  getScreen(key: string): Observable<ScreenDefinition | undefined> {
    return this.getCatalog().pipe(map((catalog) => catalog.screens.find((screen) => screen.key === key)));
  }
}
