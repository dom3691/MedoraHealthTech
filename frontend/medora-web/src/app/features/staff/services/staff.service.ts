import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiClientService } from '../../../shared/services/api-client.service';
import { PagedResult, QueryParameters } from '../../../shared/types/pagination';
import { CreateStaffMemberRequest, StaffMemberDto } from '../types/staff.types';

@Injectable({ providedIn: 'root' })
export class StaffService {
  constructor(private readonly api: ApiClientService) {}

  getStaff(query: QueryParameters = {}): Observable<PagedResult<StaffMemberDto>> {
    return this.api.get<PagedResult<StaffMemberDto>>('/api/staff', query);
  }

  createStaffMember(request: CreateStaffMemberRequest): Observable<StaffMemberDto> {
    return this.api.post<StaffMemberDto>('/api/staff', request);
  }
}
