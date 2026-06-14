export type OutpatientVisitDto = {
  id: string;
  visitTime: string;
  patientName: string;
  providerName: string;
  department: string;
  type: string;
  status: string;
  waitMinutes: number;
};

export type CreateOutpatientVisitRequest = {
  patientId: string;
  providerId: string;
  departmentId?: string | null;
  visitType: string;
  reason?: string | null;
};
