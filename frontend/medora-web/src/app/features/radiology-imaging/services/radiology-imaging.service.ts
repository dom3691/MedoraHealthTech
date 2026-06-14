import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiClientService } from '../../../shared/services/api-client.service';
import { PagedResult, QueryParameters } from '../../../shared/types/pagination';
import { CreateRadiologyStudyRequest, RadiologyStudyDto, UpdateRadiologyStudyStatusRequest } from '../types/radiology-imaging.types';

@Injectable({ providedIn: 'root' })
export class RadiologyImagingService {
  constructor(private readonly api: ApiClientService) {}

  getStudies(query: QueryParameters = {}): Observable<PagedResult<RadiologyStudyDto>> {
    return this.api.get<PagedResult<RadiologyStudyDto>>('/api/radiology-imaging/studies', query);
  }

  createStudy(request: CreateRadiologyStudyRequest): Observable<RadiologyStudyDto> {
    return this.api.post<RadiologyStudyDto>('/api/radiology-imaging/studies', request);
  }

  updateStatus(id: string, request: UpdateRadiologyStudyStatusRequest): Observable<RadiologyStudyDto> {
    return this.api.patch<RadiologyStudyDto>(`/api/radiology-imaging/studies/${id}/status`, request);
  }
}
