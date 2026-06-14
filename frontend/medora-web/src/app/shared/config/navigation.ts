export type AppRole =
  | 'SuperAdmin'
  | 'OperationsAdmin'
  | 'FrontDesk'
  | 'Doctor'
  | 'Nurse'
  | 'LabTechnician'
  | 'Radiologist'
  | 'Pharmacist'
  | 'ProcurementOfficer'
  | 'Patient';

export type NavigationConfigItem = {
  label: string;
  path: string;
  icon: string;
  section: string;
  allowedRoles: AppRole[];
};

const allStaffRoles: AppRole[] = [
  'SuperAdmin',
  'OperationsAdmin',
  'FrontDesk',
  'Doctor',
  'Nurse',
  'LabTechnician',
  'Radiologist',
  'Pharmacist',
  'ProcurementOfficer'
];

export const navigationConfig: NavigationConfigItem[] = [
  { label: 'Dashboard', path: '/dashboard', icon: 'grid', section: 'Overview', allowedRoles: allStaffRoles },
  { label: 'Patient Flow', path: '/patient-flow', icon: 'workflow', section: 'Front Desk', allowedRoles: ['OperationsAdmin', 'FrontDesk', 'Nurse', 'Doctor'] },
  { label: 'Appointments', path: '/appointments', icon: 'calendar', section: 'Front Desk', allowedRoles: ['OperationsAdmin', 'FrontDesk', 'Doctor', 'Nurse'] },
  { label: 'Outpatient (OPD)', path: '/outpatient', icon: 'clipboard', section: 'Front Desk', allowedRoles: ['OperationsAdmin', 'FrontDesk', 'Doctor', 'Nurse'] },
  { label: 'Patients', path: '/patients', icon: 'users', section: 'Clinical', allowedRoles: ['OperationsAdmin', 'Doctor', 'Nurse', 'FrontDesk'] },
  { label: 'Inpatient (IPD)', path: '/inpatient', icon: 'hospital', section: 'Clinical', allowedRoles: ['OperationsAdmin', 'Doctor', 'Nurse'] },
  { label: 'Emergency (ED)', path: '/emergency', icon: 'ambulance', section: 'Clinical', allowedRoles: ['OperationsAdmin', 'Doctor', 'Nurse', 'FrontDesk'] },
  { label: 'Wards & Beds', path: '/wards-and-beds', icon: 'bed', section: 'Clinical', allowedRoles: ['OperationsAdmin', 'Nurse', 'Doctor'] },
  { label: 'Theatre & Surgery', path: '/theatre-and-surgery', icon: 'scissors', section: 'Clinical', allowedRoles: ['OperationsAdmin', 'Doctor', 'Nurse'] },
  { label: 'Nursing', path: '/nursing', icon: 'syringe', section: 'Clinical', allowedRoles: ['OperationsAdmin', 'Nurse'] },
  { label: 'Lab & Diagnostics', path: '/lab-diagnostics', icon: 'flask', section: 'Diagnostics', allowedRoles: ['OperationsAdmin', 'Doctor', 'LabTechnician'] },
  { label: 'Radiology & Imaging', path: '/radiology-imaging', icon: 'scan', section: 'Diagnostics', allowedRoles: ['OperationsAdmin', 'Doctor', 'Radiologist'] },
  { label: 'Pharmacy & Inventory', path: '/pharmacy-inventory', icon: 'pill', section: 'Diagnostics', allowedRoles: ['OperationsAdmin', 'Pharmacist'] },
  { label: 'Doctor Portal', path: '/doctor-portal', icon: 'stethoscope', section: 'Portals', allowedRoles: ['Doctor'] },
  { label: 'Patient Portal', path: '/patient-portal', icon: 'user-circle', section: 'Portals', allowedRoles: ['Patient'] },
  { label: 'Telemedicine', path: '/telemedicine', icon: 'video', section: 'Portals', allowedRoles: ['OperationsAdmin', 'Doctor', 'FrontDesk', 'Patient'] },
  { label: 'Procurement & Vendors', path: '/procurement-vendors', icon: 'truck', section: 'Operations', allowedRoles: ['OperationsAdmin', 'ProcurementOfficer', 'Pharmacist'] },
  { label: 'Notifications', path: '/notifications', icon: 'bell', section: 'Operations', allowedRoles: allStaffRoles },
  { label: 'Staff', path: '/staff', icon: 'staff', section: 'HR Operations', allowedRoles: ['SuperAdmin', 'OperationsAdmin'] }
];
