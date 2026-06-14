import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiClientService } from '../../../shared/services/api-client.service';
import { PagedResult, QueryParameters } from '../../../shared/types/pagination';
import { CreateInventoryItemRequest, InventoryItemDto, ReorderInventoryItemRequest } from '../types/pharmacy-inventory.types';

@Injectable({ providedIn: 'root' })
export class PharmacyInventoryService {
  constructor(private readonly api: ApiClientService) {}

  getItems(query: QueryParameters = {}): Observable<PagedResult<InventoryItemDto>> {
    return this.api.get<PagedResult<InventoryItemDto>>('/api/pharmacy-inventory/items', query);
  }

  createItem(request: CreateInventoryItemRequest): Observable<InventoryItemDto> {
    return this.api.post<InventoryItemDto>('/api/pharmacy-inventory/items', request);
  }

  reorderItem(request: ReorderInventoryItemRequest): Observable<unknown> {
    return this.api.post('/api/pharmacy-inventory/reorders', request);
  }
}
