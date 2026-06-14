import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiClientService } from '../../../shared/services/api-client.service';
import { PagedResult, QueryParameters } from '../../../shared/types/pagination';
import { CreatePatientRequest, PatientDto, UpdatePatientStatusRequest } from '../types/patient.types';

@Injectable({ providedIn: 'root' })
export class PatientService {
  constructor(private readonly api: ApiClientService) {}

  getPatients(query: QueryParameters = {}): Observable<PagedResult<PatientDto>> {
    return this.api.get<PagedResult<PatientDto>>('/api/patients', query);
  }

  getPatient(id: string): Observable<PatientDto> {
    return this.api.get<PatientDto>(`/api/patients/${id}`);
  }

  createPatient(request: CreatePatientRequest): Observable<PatientDto> {
    return this.api.post<PatientDto>('/api/patients', request);
  }

  updateStatus(id: string, request: UpdatePatientStatusRequest): Observable<PatientDto> {
    return this.api.patch<PatientDto>(`/api/patients/${id}/status`, request);
  }
}
