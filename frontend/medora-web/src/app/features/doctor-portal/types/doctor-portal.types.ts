export type DoctorPortalSummaryDto = {
  myPatients: number;
  appointments: number;
  resultsToReview: number;
  unsignedNotes: number;
};

export type DoctorWorklistDto = {
  appointments: Record<string, unknown>[];
  resultsToReview: Record<string, unknown>[];
  patients: Record<string, unknown>[];
};
