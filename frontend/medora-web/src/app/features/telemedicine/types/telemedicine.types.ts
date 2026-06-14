export type TelemedicineSessionDto = {
  id: string;
  scheduledTime: string;
  patientName: string;
  providerName: string;
  channel: 'Video' | 'Phone' | 'Chat' | string;
  durationMinutes?: number | null;
  status: string;
};

export type ScheduleTelemedicineSessionRequest = {
  patientId: string;
  providerId: string;
  scheduledAt: string;
  channel: string;
  reason?: string | null;
};
