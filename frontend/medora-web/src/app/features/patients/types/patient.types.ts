export type PatientStatus = 'Admitted' | 'Outpatient' | 'Critical' | 'Discharged';

export type PatientDto = {
  id: string;
  mrn: string;
  fullName: string;
  age: number;
  sex: string;
  ward?: string | null;
  attendingProvider?: string | null;
  admittedAt?: string | null;
  status: PatientStatus | string;
};

export type CreatePatientRequest = {
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  sex: string;
  phoneNumber?: string | null;
  email?: string | null;
  address?: string | null;
};

export type UpdatePatientStatusRequest = {
  status: PatientStatus | string;
};
