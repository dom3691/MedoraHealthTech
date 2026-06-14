import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiClientService } from '../../../shared/services/api-client.service';
import { PatientSelfServiceDto } from '../types/patient-portal.types';

@Injectable({ providedIn: 'root' })
export class PatientPortalService {
  constructor(private readonly api: ApiClientService) {}

  getSelfServiceView(): Observable<PatientSelfServiceDto> {
    return this.api.get<PatientSelfServiceDto>('/api/patient-portal');
  }
}
