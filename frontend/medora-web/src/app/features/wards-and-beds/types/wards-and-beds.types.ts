export type WardDto = {
  id: string;
  name: string;
  totalBeds: number;
  occupiedBeds: number;
  occupancyPercentage: number;
};

export type BedDto = {
  id: string;
  wardId: string;
  code: string;
  status: 'Occupied' | 'Available' | 'Reserved' | 'Cleaning' | string;
  patientId?: string | null;
};

export type AllocateBedRequest = {
  patientId: string;
  admissionId?: string | null;
  wardId: string;
  bedId: string;
};

export type UpdateBedStatusRequest = {
  status: string;
};
