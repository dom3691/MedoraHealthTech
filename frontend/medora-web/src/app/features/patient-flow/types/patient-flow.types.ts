export type PatientFlowStage = 'AwaitingVitals' | 'AwaitingDoctor' | 'Completed';

export type PatientFlowEncounterDto = {
  id: string;
  queueNumber: string;
  patientName: string;
  age: number;
  sex: string;
  complaint: string;
  payerType: string;
  hmoProvider?: string | null;
  stage: PatientFlowStage | string;
  waitMinutes: number;
};

export type RegisterWalkInRequest = {
  patientId?: string | null;
  firstName: string;
  lastName: string;
  age: number;
  sex: string;
  complaint: string;
  payerType: string;
  hmoProvider?: string | null;
};

export type UpdatePatientFlowStageRequest = {
  stage: PatientFlowStage | string;
  outcome?: string | null;
};
