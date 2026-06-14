import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiClientService } from '../../../shared/services/api-client.service';
import { PagedResult, QueryParameters } from '../../../shared/types/pagination';
import { CreateNursingTaskRequest, NursingTaskDto, UpdateNursingTaskStatusRequest } from '../types/nursing.types';

@Injectable({ providedIn: 'root' })
export class NursingService {
  constructor(private readonly api: ApiClientService) {}

  getTasks(query: QueryParameters = {}): Observable<PagedResult<NursingTaskDto>> {
    return this.api.get<PagedResult<NursingTaskDto>>('/api/nursing/tasks', query);
  }

  createTask(request: CreateNursingTaskRequest): Observable<NursingTaskDto> {
    return this.api.post<NursingTaskDto>('/api/nursing/tasks', request);
  }

  updateStatus(id: string, request: UpdateNursingTaskStatusRequest): Observable<NursingTaskDto> {
    return this.api.patch<NursingTaskDto>(`/api/nursing/tasks/${id}/status`, request);
  }
}
