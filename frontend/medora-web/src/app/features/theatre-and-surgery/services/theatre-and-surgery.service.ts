import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiClientService } from '../../../shared/services/api-client.service';
import { PagedResult, QueryParameters } from '../../../shared/types/pagination';
import { CreateSurgeryRequest, SurgeryDto, UpdateSurgeryStatusRequest } from '../types/theatre-and-surgery.types';

@Injectable({ providedIn: 'root' })
export class TheatreAndSurgeryService {
  constructor(private readonly api: ApiClientService) {}

  getCases(query: QueryParameters = {}): Observable<PagedResult<SurgeryDto>> {
    return this.api.get<PagedResult<SurgeryDto>>('/api/theatre-and-surgery/cases', query);
  }

  createCase(request: CreateSurgeryRequest): Observable<SurgeryDto> {
    return this.api.post<SurgeryDto>('/api/theatre-and-surgery/cases', request);
  }

  updateStatus(id: string, request: UpdateSurgeryStatusRequest): Observable<SurgeryDto> {
    return this.api.patch<SurgeryDto>(`/api/theatre-and-surgery/cases/${id}/status`, request);
  }
}
