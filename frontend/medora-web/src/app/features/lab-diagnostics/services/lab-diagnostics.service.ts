import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiClientService } from '../../../shared/services/api-client.service';
import { PagedResult, QueryParameters } from '../../../shared/types/pagination';
import { CreateLabOrderRequest, LabOrderDto, UpdateLabOrderStatusRequest } from '../types/lab-diagnostics.types';

@Injectable({ providedIn: 'root' })
export class LabDiagnosticsService {
  constructor(private readonly api: ApiClientService) {}

  getOrders(query: QueryParameters = {}): Observable<PagedResult<LabOrderDto>> {
    return this.api.get<PagedResult<LabOrderDto>>('/api/lab-diagnostics/orders', query);
  }

  createOrder(request: CreateLabOrderRequest): Observable<LabOrderDto> {
    return this.api.post<LabOrderDto>('/api/lab-diagnostics/orders', request);
  }

  updateStatus(id: string, request: UpdateLabOrderStatusRequest): Observable<LabOrderDto> {
    return this.api.patch<LabOrderDto>(`/api/lab-diagnostics/orders/${id}/status`, request);
  }
}
