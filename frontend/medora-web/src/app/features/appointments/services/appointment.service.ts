import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiClientService } from '../../../shared/services/api-client.service';
import { PagedResult, QueryParameters } from '../../../shared/types/pagination';
import { AppointmentDto, CreateAppointmentRequest, UpdateAppointmentStatusRequest } from '../types/appointment.types';

@Injectable({ providedIn: 'root' })
export class AppointmentService {
  constructor(private readonly api: ApiClientService) {}

  getAppointments(query: QueryParameters = {}): Observable<PagedResult<AppointmentDto>> {
    return this.api.get<PagedResult<AppointmentDto>>('/api/appointments', query);
  }

  createAppointment(request: CreateAppointmentRequest): Observable<AppointmentDto> {
    return this.api.post<AppointmentDto>('/api/appointments', request);
  }

  updateStatus(id: string, request: UpdateAppointmentStatusRequest): Observable<AppointmentDto> {
    return this.api.patch<AppointmentDto>(`/api/appointments/${id}/status`, request);
  }
}
