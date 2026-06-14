export type SurgeryDto = {
  id: string;
  scheduledTime: string;
  patientName: string;
  procedureName: string;
  surgeonName: string;
  theatre: string;
  status: string;
};

export type CreateSurgeryRequest = {
  patientId: string;
  procedureName: string;
  surgeonId: string;
  theatreId?: string | null;
  scheduledAt: string;
};

export type UpdateSurgeryStatusRequest = {
  status: string;
};
