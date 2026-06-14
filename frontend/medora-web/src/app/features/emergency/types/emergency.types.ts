export type EmergencyCaseDto = {
  id: string;
  patientId: string;
  patientName: string;
  acuity: string;
  complaint: string;
  arrivedAt: string;
  waitMinutes: number;
  status: string;
};

export type RegisterEmergencyArrivalRequest = {
  patientId?: string | null;
  patientName: string;
  complaint: string;
  acuity: string;
};

export type UpdateEmergencyStatusRequest = {
  status: string;
};
