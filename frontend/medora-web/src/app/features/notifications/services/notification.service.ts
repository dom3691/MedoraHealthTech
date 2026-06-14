import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiClientService } from '../../../shared/services/api-client.service';
import { PagedResult, QueryParameters } from '../../../shared/types/pagination';
import { CreateNotificationRequest, NotificationDto } from '../types/notification.types';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  constructor(private readonly api: ApiClientService) {}

  getNotifications(query: QueryParameters = {}): Observable<PagedResult<NotificationDto>> {
    return this.api.get<PagedResult<NotificationDto>>('/api/notifications', query);
  }

  createNotification(request: CreateNotificationRequest): Observable<NotificationDto> {
    return this.api.post<NotificationDto>('/api/notifications', request);
  }
}
