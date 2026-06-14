import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiClientService } from '../../../shared/services/api-client.service';
import { PagedResult, QueryParameters } from '../../../shared/types/pagination';
import { CreatePurchaseOrderRequest, PurchaseOrderDto, VendorDto } from '../types/procurement-vendors.types';

@Injectable({ providedIn: 'root' })
export class ProcurementVendorsService {
  constructor(private readonly api: ApiClientService) {}

  getVendors(query: QueryParameters = {}): Observable<PagedResult<VendorDto>> {
    return this.api.get<PagedResult<VendorDto>>('/api/procurement-vendors/vendors', query);
  }

  getPurchaseOrders(query: QueryParameters = {}): Observable<PagedResult<PurchaseOrderDto>> {
    return this.api.get<PagedResult<PurchaseOrderDto>>('/api/procurement-vendors/purchase-orders', query);
  }

  createPurchaseOrder(request: CreatePurchaseOrderRequest): Observable<PurchaseOrderDto> {
    return this.api.post<PurchaseOrderDto>('/api/procurement-vendors/purchase-orders', request);
  }
}
