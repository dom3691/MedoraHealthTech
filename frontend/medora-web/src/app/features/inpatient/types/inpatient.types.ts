export type AdmissionDto = {
  id: string;
  patientId: string;
  patientName: string;
  mrn: string;
  ward: string;
  bed?: string | null;
  attendingProvider: string;
  admittedAt: string;
  status: string;
};

export type AdmitPatientRequest = {
  patientId: string;
  wardId: string;
  bedId?: string | null;
  attendingProviderId: string;
  diagnosis?: string | null;
};

export type DischargePatientRequest = {
  dischargeSummary: string;
  dischargedAt?: string | null;
};
