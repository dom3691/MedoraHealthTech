export type RadiologyStudyDto = {
  id: string;
  studyNumber: string;
  patientId: string;
  patientName: string;
  modality: string;
  bodyPart: string;
  status: string;
  radiologistName?: string | null;
  finding?: string | null;
};

export type CreateRadiologyStudyRequest = {
  patientId: string;
  modality: string;
  bodyPart: string;
  priority?: string | null;
  clinicalNotes?: string | null;
};

export type UpdateRadiologyStudyStatusRequest = {
  status: string;
};
