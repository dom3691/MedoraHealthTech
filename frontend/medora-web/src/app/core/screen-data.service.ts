import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { map, Observable, shareReplay, tap } from 'rxjs';

import { AppContextService } from './app-context.service';
import { ScreenCatalogResponse, ScreenDefinition } from './screen.models';

@Injectable({ providedIn: 'root' })
export class ScreenDataService {
  private readonly endpoint = '/api/operations/screens';
  readonly catalog = signal<ScreenCatalogResponse | null>(null);

  constructor(
    private readonly http: HttpClient,
    private readonly appContext: AppContextService
  ) {}

  getCatalog(): Observable<ScreenCatalogResponse> {
    const tenantCode = this.appContext.activeTenantCode();
    const endpoint = tenantCode ? `${this.endpoint}?tenantCode=${encodeURIComponent(tenantCode)}` : this.endpoint;

    return this.http.get<ScreenCatalogResponse>(endpoint).pipe(
      tap((catalog) => this.catalog.set(catalog)),
      shareReplay({ bufferSize: 1, refCount: true })
    );
  }

  getScreen(key: string): Observable<ScreenDefinition | undefined> {
    return this.getCatalog().pipe(map((catalog) => catalog.screens.find((screen) => screen.key === key)));
  }
}
