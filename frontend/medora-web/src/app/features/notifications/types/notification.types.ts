export type NotificationDto = {
  id: string;
  title: string;
  message: string;
  category: string;
  severity: string;
  createdAt: string;
  isRead: boolean;
};

export type CreateNotificationRequest = {
  title: string;
  message: string;
  category: string;
  severity: string;
  targetRoleCodes?: string[];
};
