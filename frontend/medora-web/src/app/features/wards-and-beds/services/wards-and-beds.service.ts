import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiClientService } from '../../../shared/services/api-client.service';
import { BedDto, AllocateBedRequest, UpdateBedStatusRequest, WardDto } from '../types/wards-and-beds.types';

@Injectable({ providedIn: 'root' })
export class WardsAndBedsService {
  constructor(private readonly api: ApiClientService) {}

  getWards(): Observable<WardDto[]> {
    return this.api.get<WardDto[]>('/api/wards-and-beds/wards');
  }

  getBeds(wardId?: string): Observable<BedDto[]> {
    return this.api.get<BedDto[]>('/api/wards-and-beds/beds', wardId ? { wardId } : undefined);
  }

  allocateBed(request: AllocateBedRequest): Observable<BedDto> {
    return this.api.post<BedDto>('/api/wards-and-beds/allocate-bed', request);
  }

  updateBedStatus(id: string, request: UpdateBedStatusRequest): Observable<BedDto> {
    return this.api.patch<BedDto>(`/api/wards-and-beds/beds/${id}/status`, request);
  }
}
