import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiClientService } from '../../../shared/services/api-client.service';
import { PagedResult, QueryParameters } from '../../../shared/types/pagination';
import { AdmissionDto, AdmitPatientRequest, DischargePatientRequest } from '../types/inpatient.types';

@Injectable({ providedIn: 'root' })
export class InpatientService {
  constructor(private readonly api: ApiClientService) {}

  getAdmissions(query: QueryParameters = {}): Observable<PagedResult<AdmissionDto>> {
    return this.api.get<PagedResult<AdmissionDto>>('/api/inpatient/admissions', query);
  }

  admitPatient(request: AdmitPatientRequest): Observable<AdmissionDto> {
    return this.api.post<AdmissionDto>('/api/inpatient/admissions', request);
  }

  dischargePatient(id: string, request: DischargePatientRequest): Observable<AdmissionDto> {
    return this.api.patch<AdmissionDto>(`/api/inpatient/admissions/${id}/discharge`, request);
  }
}
