export type LabOrderDto = {
  id: string;
  orderNumber: string;
  patientId: string;
  patientName: string;
  testName: string;
  priority: 'Routine' | 'Urgent' | 'Stat' | string;
  status: string;
  orderedAt: string;
  technicianName?: string | null;
};

export type CreateLabOrderRequest = {
  patientId: string;
  testCode: string;
  priority: string;
  notes?: string | null;
};

export type UpdateLabOrderStatusRequest = {
  status: string;
};
