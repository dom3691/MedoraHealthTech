import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiClientService } from '../../../shared/services/api-client.service';
import { PagedResult, QueryParameters } from '../../../shared/types/pagination';
import { PatientFlowEncounterDto, RegisterWalkInRequest, UpdatePatientFlowStageRequest } from '../types/patient-flow.types';

@Injectable({ providedIn: 'root' })
export class PatientFlowService {
  constructor(private readonly api: ApiClientService) {}

  getEncounters(query: QueryParameters = {}): Observable<PagedResult<PatientFlowEncounterDto>> {
    return this.api.get<PagedResult<PatientFlowEncounterDto>>('/api/patient-flow/encounters', query);
  }

  registerWalkIn(request: RegisterWalkInRequest): Observable<PatientFlowEncounterDto> {
    return this.api.post<PatientFlowEncounterDto>('/api/patient-flow/walk-ins', request);
  }

  updateStage(id: string, request: UpdatePatientFlowStageRequest): Observable<PatientFlowEncounterDto> {
    return this.api.patch<PatientFlowEncounterDto>(`/api/patient-flow/encounters/${id}/stage`, request);
  }
}
