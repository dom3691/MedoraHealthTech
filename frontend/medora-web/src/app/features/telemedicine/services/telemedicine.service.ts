import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiClientService } from '../../../shared/services/api-client.service';
import { PagedResult, QueryParameters } from '../../../shared/types/pagination';
import { ScheduleTelemedicineSessionRequest, TelemedicineSessionDto } from '../types/telemedicine.types';

@Injectable({ providedIn: 'root' })
export class TelemedicineService {
  constructor(private readonly api: ApiClientService) {}

  getSessions(query: QueryParameters = {}): Observable<PagedResult<TelemedicineSessionDto>> {
    return this.api.get<PagedResult<TelemedicineSessionDto>>('/api/telemedicine/sessions', query);
  }

  scheduleSession(request: ScheduleTelemedicineSessionRequest): Observable<TelemedicineSessionDto> {
    return this.api.post<TelemedicineSessionDto>('/api/telemedicine/sessions', request);
  }
}
