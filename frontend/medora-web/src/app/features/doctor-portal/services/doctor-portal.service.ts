import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiClientService } from '../../../shared/services/api-client.service';
import { DoctorWorklistDto } from '../types/doctor-portal.types';

@Injectable({ providedIn: 'root' })
export class DoctorPortalService {
  constructor(private readonly api: ApiClientService) {}

  getWorklist(): Observable<DoctorWorklistDto> {
    return this.api.get<DoctorWorklistDto>('/api/doctor-portal');
  }
}
