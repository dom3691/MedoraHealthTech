export type PatientPortalSummaryDto = {
  upcomingVisits: number;
  newResults: number;
  balanceDue: number;
  activeMedications: number;
};

export type PatientSelfServiceDto = {
  profile: Record<string, unknown>;
  appointments: Record<string, unknown>[];
  medications: Record<string, unknown>[];
  labResults: Record<string, unknown>[];
};
