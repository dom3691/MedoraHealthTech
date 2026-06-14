export type NursingTaskDto = {
  id: string;
  patientName: string;
  ward: string;
  taskName: string;
  dueAt: string;
  nurseName: string;
  status: string;
};

export type CreateNursingTaskRequest = {
  patientId: string;
  taskName: string;
  dueAt: string;
  nurseId?: string | null;
  notes?: string | null;
};

export type UpdateNursingTaskStatusRequest = {
  status: string;
};
