export type StaffMemberDto = {
  id: string;
  fullName: string;
  roleName: string;
  department: string;
  engagementType: 'Employee' | 'Consultant' | string;
  status: string;
  employeeNumber?: string | null;
  shift?: string | null;
};

export type CreateStaffMemberRequest = {
  fullName: string;
  email: string;
  roleCodes: string[];
  departmentId?: string | null;
  jobTitle: string;
  engagementType: string;
  startDate?: string | null;
};
