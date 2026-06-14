export const apiConfig = {
  baseUrl: (globalThis as { MEDORA_API_BASE_URL?: string }).MEDORA_API_BASE_URL ?? ''
};
