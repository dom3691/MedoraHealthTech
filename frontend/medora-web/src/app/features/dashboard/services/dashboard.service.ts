import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiClientService } from '../../../shared/services/api-client.service';
import { DashboardSummaryDto } from '../types/dashboard.types';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  constructor(private readonly api: ApiClientService) {}

  getSummary(): Observable<DashboardSummaryDto> {
    return this.api.get<DashboardSummaryDto>('/api/dashboard');
  }
}
