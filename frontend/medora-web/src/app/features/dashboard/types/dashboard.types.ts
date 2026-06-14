export type DashboardSummaryDto = {
  patientsToday: number;
  admissions: number;
  bedOccupancy: number;
  emergencyWaitTimeMinutes: number;
  revenueMonthToDate: number;
  staffOnDuty: number;
  criticalPatients: DashboardPatientAlertDto[];
  appointmentsSnapshot: DashboardAppointmentDto[];
};

export type DashboardPatientAlertDto = {
  patientId: string;
  fullName: string;
  mrn: string;
  location: string;
  diagnosis: string;
};

export type DashboardAppointmentDto = {
  id: string;
  time: string;
  patientName: string;
  providerName: string;
  type: string;
  status: string;
};
