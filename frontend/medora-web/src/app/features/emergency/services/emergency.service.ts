import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiClientService } from '../../../shared/services/api-client.service';
import { PagedResult, QueryParameters } from '../../../shared/types/pagination';
import { EmergencyCaseDto, RegisterEmergencyArrivalRequest, UpdateEmergencyStatusRequest } from '../types/emergency.types';

@Injectable({ providedIn: 'root' })
export class EmergencyService {
  constructor(private readonly api: ApiClientService) {}

  getCases(query: QueryParameters = {}): Observable<PagedResult<EmergencyCaseDto>> {
    return this.api.get<PagedResult<EmergencyCaseDto>>('/api/emergency/cases', query);
  }

  registerArrival(request: RegisterEmergencyArrivalRequest): Observable<EmergencyCaseDto> {
    return this.api.post<EmergencyCaseDto>('/api/emergency/arrivals', request);
  }

  updateStatus(id: string, request: UpdateEmergencyStatusRequest): Observable<EmergencyCaseDto> {
    return this.api.patch<EmergencyCaseDto>(`/api/emergency/cases/${id}/status`, request);
  }
}
