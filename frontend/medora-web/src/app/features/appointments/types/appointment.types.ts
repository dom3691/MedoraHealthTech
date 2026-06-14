export type AppointmentStatus = 'Confirmed' | 'CheckedIn' | 'InProgress' | 'Completed' | 'Cancelled';

export type AppointmentDto = {
  id: string;
  appointmentDate: string;
  appointmentTime: string;
  patientId: string;
  patientName: string;
  providerId: string;
  providerName: string;
  department: string;
  type: string;
  room?: string | null;
  status: AppointmentStatus | string;
};

export type CreateAppointmentRequest = {
  patientId: string;
  providerId: string;
  departmentId?: string | null;
  appointmentDate: string;
  appointmentTime: string;
  type: string;
  reason?: string | null;
};

export type UpdateAppointmentStatusRequest = {
  status: AppointmentStatus | string;
};
