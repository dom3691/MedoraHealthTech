import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiClientService } from '../../../shared/services/api-client.service';
import { PagedResult, QueryParameters } from '../../../shared/types/pagination';
import { CreateOutpatientVisitRequest, OutpatientVisitDto } from '../types/outpatient.types';

@Injectable({ providedIn: 'root' })
export class OutpatientService {
  constructor(private readonly api: ApiClientService) {}

  getVisits(query: QueryParameters = {}): Observable<PagedResult<OutpatientVisitDto>> {
    return this.api.get<PagedResult<OutpatientVisitDto>>('/api/outpatient/visits', query);
  }

  createVisit(request: CreateOutpatientVisitRequest): Observable<OutpatientVisitDto> {
    return this.api.post<OutpatientVisitDto>('/api/outpatient/visits', request);
  }
}
