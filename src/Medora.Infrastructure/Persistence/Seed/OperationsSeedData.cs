using Medora.Domain.Audit;
using Medora.Domain.HospitalOperations;
using Medora.Domain.HrOperations;

namespace Medora.Infrastructure.Persistence.Seed;

internal static class OperationsSeedData
{
    private static readonly DateTimeOffset Today = new(2026, 6, 11, 8, 0, 0, TimeSpan.Zero);

    public static IReadOnlyList<OperationalModule> Modules(Guid tenantId, DateTimeOffset now) =>
    [
        Module(tenantId, now, 10, "dashboard", "Dashboard", "/dashboard", "Overview", "grid", "Operations Dashboard", "Hospital-wide clinical, operational, finance, and workforce signals."),
        Module(tenantId, now, 20, "patient-flow", "Patient Flow", "/patient-flow", "Front Desk", "workflow", "Patient Flow", "Walk-in journey from registration to consultation outcome."),
        Module(tenantId, now, 30, "appointments", "Appointments", "/appointments", "Front Desk", "calendar", "Appointments", "Manage schedules, providers, rooms, and clinical sessions."),
        Module(tenantId, now, 40, "outpatient", "Outpatient (OPD)", "/outpatient", "Front Desk", "clipboard", "Outpatient Department", "Clinic visits, consultations, and follow-up scheduling."),
        Module(tenantId, now, 50, "patients", "Patients", "/patients", "Clinical", "users", "Patients", "Manage admitted patients, outpatient records, and EHR context."),
        Module(tenantId, now, 60, "inpatient", "Inpatient (IPD)", "/inpatient", "Clinical", "hospital", "Inpatient Department", "Admissions, bed occupancy, discharge planning, and clinical status."),
        Module(tenantId, now, 70, "emergency", "Emergency (ED)", "/emergency", "Clinical", "ambulance", "Emergency Department", "Triage board, acuity tracking, and treatment status."),
        Module(tenantId, now, 80, "wards-beds", "Wards & Beds", "/wards-beds", "Clinical", "bed", "Bed Management", "Real-time ward occupancy and bed allocation."),
        Module(tenantId, now, 90, "theatre-surgery", "Theatre & Surgery", "/theatre-surgery", "Clinical", "scissors", "Theatre & Surgery", "Operating room schedule, case status, and theatre utilization."),
        Module(tenantId, now, 100, "nursing", "Nursing", "/nursing", "Clinical", "syringe", "Nursing Station", "Care tasks, medication rounds, vitals, and ward staffing."),
        Module(tenantId, now, 110, "lab-diagnostics", "Lab & Diagnostics", "/lab-diagnostics", "Diagnostics", "flask", "Lab Worklist", "Laboratory orders, sample processing, and results."),
        Module(tenantId, now, 120, "radiology-imaging", "Radiology & Imaging", "/radiology-imaging", "Diagnostics", "scan", "Radiology & Imaging", "Imaging worklist, reporting status, and critical findings."),
        Module(tenantId, now, 130, "pharmacy-inventory", "Pharmacy & Inventory", "/pharmacy-inventory", "Diagnostics", "pill", "Pharmacy Inventory", "Medication stock, consumables, dispensing, and reordering."),
        Module(tenantId, now, 140, "doctor-portal", "Doctor Portal", "/doctor-portal", "Portals", "stethoscope", "Doctor Portal", "Personal worklist, patient panels, results, and unsigned notes."),
        Module(tenantId, now, 150, "patient-portal", "Patient Portal", "/patient-portal", "Portals", "user-circle", "Patient Portal", "Self-service view for appointments, results, medications, billing, and claims."),
        Module(tenantId, now, 160, "telemedicine", "Telemedicine", "/telemedicine", "Portals", "video", "Telemedicine", "Virtual consultations across video, phone, and chat."),
        Module(tenantId, now, 170, "procurement-vendors", "Procurement & Vendors", "/procurement-vendors", "Operations", "truck", "Procurement & Vendors", "Purchase orders, vendor performance, approvals, and supply spend."),
        Module(tenantId, now, 180, "notifications", "Notifications", "/notifications", "Operations", "bell", "Notifications & Communication", "Alerts, clinical signals, messages, and hospital-wide broadcasts."),
        Module(tenantId, now, 190, "staff", "Staff", "/staff", "HR Operations", "staff", "Staff Directory", "Employees, consultants, engagement details, and HR status."),
        Module(tenantId, now, 200, "departments", "Departments", "/departments", "HR Operations", "building", "Departments", "Hospital divisions, staffing, departmental heads, and capacity."),
        Module(tenantId, now, 210, "hr-workflows", "HR Workflows", "/hr-workflows", "HR Operations", "briefcase", "HR Operations", "Recruitment, shifts, attendance, leave, credentials, training, and performance."),
        Module(tenantId, now, 220, "billing-invoicing", "Billing & Invoicing", "/billing-invoicing", "Finance", "dollar", "Billing & Invoicing", "Patient invoices, payments, outstanding balances, and receivables."),
        Module(tenantId, now, 230, "hmo-insurance", "HMO & Insurance", "/hmo-insurance", "Finance", "shield", "HMO & Insurance", "Claims processing, pre-authorizations, plan coverage, and approvals."),
        Module(tenantId, now, 240, "reports-analytics", "Reports", "/reports-analytics", "Finance", "chart", "Reports & Analytics", "Operational, clinical, financial, and HR performance reporting."),
        Module(tenantId, now, 250, "admin-settings", "Admin & Settings", "/admin-settings", "Administration", "settings", "Admin & Settings", "Tenant configuration, modules, roles, security, and integrations."),
        Module(tenantId, now, 260, "audit-compliance", "Audit & Compliance", "/audit-compliance", "Administration", "audit", "Audit & Compliance", "Access logs, security events, and compliance posture.")
    ];

    public static IReadOnlyList<Department> Departments(Guid tenantId, DateTimeOffset now) =>
    [
        Department(tenantId, now, "SM-D1", "Cardiology", "Clinical", "Dr. Helen Acharya", 42, 60, 88),
        Department(tenantId, now, "SM-D2", "Neurology", "Clinical", "Dr. Marcus Lin", 28, 40, 72),
        Department(tenantId, now, "SM-D3", "Emergency", "Clinical", "Priya Nair", 55, 30, 95),
        Department(tenantId, now, "SM-D4", "Surgery", "Clinical", "Elena Rossi", 64, 80, 67),
        Department(tenantId, now, "SM-D5", "Pediatrics", "Clinical", "Dr. Aisha Khan", 36, 50, 54),
        Department(tenantId, now, "SM-D6", "Oncology", "Clinical", "Dr. Liam Patel", 31, 45, 81)
    ];

    public static IReadOnlyList<Patient> Patients(Guid tenantId, DateTimeOffset now) =>
    [
        Patient(tenantId, now, "MRN-558120", "Robert Hayes", new DateOnly(1961, 4, 18), "M", "Critical"),
        Patient(tenantId, now, "MRN-558204", "Amelia Brooks", new DateOnly(1988, 1, 8), "F", "Admitted"),
        Patient(tenantId, now, "MRN-557991", "Daniel Cho", new DateOnly(1974, 3, 11), "M", "Admitted"),
        Patient(tenantId, now, "MRN-558330", "Sofia Reyes", new DateOnly(1997, 5, 9), "F", "Outpatient"),
        Patient(tenantId, now, "MRN-557845", "James Whitaker", new DateOnly(1955, 9, 23), "M", "Admitted"),
        Patient(tenantId, now, "MRN-558410", "Nina Petrova", new DateOnly(1980, 12, 3), "F", "Admitted"),
        Patient(tenantId, now, "MRN-558455", "Omar Haddad", new DateOnly(1993, 8, 16), "M", "Critical"),
        Patient(tenantId, now, "MRN-557720", "Grace Miller", new DateOnly(1968, 7, 20), "F", "Discharged")
    ];

    public static IReadOnlyList<StaffMember> Staff(Guid tenantId, DateTimeOffset now) =>
    [
        Staff(tenantId, now, "EMP-1042", "Dr. Helen Acharya", "Cardiology", "Chief of Cardiology", "Employee", "Active"),
        Staff(tenantId, now, "CON-2210", "Dr. Marcus Lin", "Neurology", "Consultant Neurosurgeon", "Consultant", "Active"),
        Staff(tenantId, now, "EMP-2210", "Priya Nair", "Emergency", "Registered Nurse", "Employee", "Active"),
        Staff(tenantId, now, "CON-3304", "Dr. Samuel Boateng", "Radiology", "Consultant Radiologist", "Consultant", "Active"),
        Staff(tenantId, now, "EMP-1888", "Elena Rossi", "Surgery", "Ward Manager", "Employee", "On leave"),
        Staff(tenantId, now, "EMP-3301", "Dr. Aisha Khan", "Pediatrics", "Pediatrician", "Employee", "Active"),
        Staff(tenantId, now, "CON-7750", "Dr. Tomas Becker", "Surgery", "Consultant Anesthetist", "Consultant", "Active"),
        Staff(tenantId, now, "EMP-2750", "Grace Okoro", "Pathology", "Lab Technician", "Employee", "Active"),
        Staff(tenantId, now, "CON-6200", "Dr. Liam Patel", "Oncology", "Consultant Oncologist", "Consultant", "Inactive"),
        Staff(tenantId, now, "EMP-1620", "Maria Santos", "Pharmacy", "Pharmacist", "Employee", "Active")
    ];

    public static IReadOnlyList<Ward> Wards(Guid tenantId, DateTimeOffset now) =>
    [
        Ward(tenantId, now, "WARD-CAR", "Cardiac ICU", "Cardiology", 18, 16),
        Ward(tenantId, now, "WARD-MAT", "Maternity", "Pediatrics", 24, 14),
        Ward(tenantId, now, "WARD-NEU", "Neurology", "Neurology", 20, 15),
        Ward(tenantId, now, "WARD-SUR", "General Surgery", "Surgery", 32, 21),
        Ward(tenantId, now, "WARD-PED", "Pediatrics", "Pediatrics", 26, 14),
        Ward(tenantId, now, "WARD-ONC", "Oncology", "Oncology", 22, 18)
    ];

    public static IReadOnlyList<Bed> Beds(Guid tenantId, DateTimeOffset now) =>
        Enumerable.Range(1, 18)
            .Select(index => new Bed
            {
                TenantId = tenantId,
                CreatedAt = now,
                BedNumber = $"CAR-{index:00}",
                WardName = "Cardiac ICU",
                Status = index <= 16 ? "Occupied" : index == 17 ? "Reserved" : "Cleaning"
            })
            .ToArray();

    public static IReadOnlyList<Appointment> Appointments(Guid tenantId, DateTimeOffset now) =>
    [
        Appointment(tenantId, now, "APT-1001", "MRN-558330", "Sofia Reyes", "Dr. Aisha Khan", "Pediatrics", "Follow-up", "C-204", "Completed", 8, 30),
        Appointment(tenantId, now, "APT-1002", "MRN-558120", "Robert Hayes", "Dr. Helen Acharya", "Cardiology", "Consultation", "A-110", "In progress", 9, 0),
        Appointment(tenantId, now, "APT-1003", "MRN-557991", "Daniel Cho", "Dr. Marcus Lin", "Neurology", "Procedure", "OR-3", "Checked in", 9, 30),
        Appointment(tenantId, now, "APT-1004", "MRN-557720", "Grace Miller", "Dr. Helen Acharya", "Cardiology", "Follow-up", "A-112", "Confirmed", 10, 15),
        Appointment(tenantId, now, "APT-1005", "MRN-557845", "James Whitaker", "Dr. Liam Patel", "Oncology", "Procedure", "ONC-1", "Confirmed", 11, 0),
        Appointment(tenantId, now, "APT-1006", "MRN-558410", "Nina Petrova", "Dr. Samuel Boateng", "Radiology", "Imaging", "RAD-2", "Cancelled", 11, 45),
        Appointment(tenantId, now, "APT-1007", "MRN-558204", "Amelia Brooks", "Dr. Aisha Khan", "Maternity", "Consultation", "M-008", "Confirmed", 13, 0)
    ];

    public static IReadOnlyList<PatientEncounter> PatientEncounters(Guid tenantId, DateTimeOffset now) =>
    [
        Encounter(tenantId, now, "A-001", "Marcus Bell", "Walk-in", "Awaiting vitals", "Persistent cough and fever", "HMO", "Aetna Health", null, 6),
        Encounter(tenantId, now, "A-002", "Lena Hopkins", "Walk-in", "Awaiting vitals", "Severe migraine", "Self-pay", null, null, 11),
        Encounter(tenantId, now, "A-003", "Owen Drake", "Walk-in", "Awaiting doctor", "Chest tightness on exertion", "HMO", "BlueCross Shield", null, 24),
        Encounter(tenantId, now, "A-004", "Priya Shah", "Walk-in", "Awaiting doctor", "Abdominal pain", "HMO", "UnitedCare", null, 31),
        Encounter(tenantId, now, "A-005", "Carmen Diaz", "Walk-in", "Completed", "Acute pharyngitis", "Self-pay", null, "Dr. Helen Acharya", 0)
    ];

    public static IReadOnlyList<Admission> Admissions(Guid tenantId, DateTimeOffset now) =>
    [
        Admission(tenantId, now, "ADM-1001", "MRN-558120", "Robert Hayes", "Cardiac ICU", "CAR-01", "Dr. Helen Acharya", "Critical", "Acute myocardial infarction", -4),
        Admission(tenantId, now, "ADM-1002", "MRN-558204", "Amelia Brooks", "Maternity", "MAT-04", "Dr. Aisha Khan", "Admitted", "High-risk maternity observation", -2),
        Admission(tenantId, now, "ADM-1003", "MRN-557991", "Daniel Cho", "Neurology", "NEU-03", "Dr. Marcus Lin", "Admitted", "Post-procedure observation", -6),
        Admission(tenantId, now, "ADM-1004", "MRN-557845", "James Whitaker", "Oncology", "ONC-02", "Dr. Liam Patel", "Admitted", "Chemotherapy cycle", -9),
        Admission(tenantId, now, "ADM-1005", "MRN-558410", "Nina Petrova", "Surgery", "SUR-12", "Dr. Tomas Becker", "Admitted", "Pre-operative care", -3),
        Admission(tenantId, now, "ADM-1006", "MRN-558455", "Omar Haddad", "Emergency", "ED-02", "Priya Nair", "Critical", "Multiple trauma", 0)
    ];

    public static IReadOnlyList<EmergencyCase> EmergencyCases(Guid tenantId, DateTimeOffset now) =>
    [
        Emergency(tenantId, now, "ED-9001", "Robert Hayes", "Resuscitation", "Acute myocardial infarction", 7, 0, "In treatment"),
        Emergency(tenantId, now, "ED-9002", "Omar Haddad", "Emergent", "Multiple trauma - RTA", 7, 35, "Observation")
    ];

    public static IReadOnlyList<SurgeryCase> SurgeryCases(Guid tenantId, DateTimeOffset now) =>
    [
        Surgery(tenantId, now, "SUR-3001", "Daniel Cho", "Coronary bypass", "Dr. Marcus Lin", "OR-1", "In Theatre", 9, 30),
        Surgery(tenantId, now, "SUR-3002", "James Whitaker", "Cholecystectomy", "Dr. Liam Patel", "OR-2", "Recovery", 11, 0)
    ];

    public static IReadOnlyList<NursingTask> NursingTasks(Guid tenantId, DateTimeOffset now) =>
    [
        Nursing(tenantId, now, "TASK-1", "Robert Hayes", "Cardiac ICU", "IV line change", "Priya Nair", "Done", 9, 0),
        Nursing(tenantId, now, "TASK-2", "Amelia Brooks", "Maternity", "Medication round", "Priya Nair", "Due", 9, 35),
        Nursing(tenantId, now, "TASK-3", "Daniel Cho", "Neurology", "Vitals check", "Priya Nair", "Done", 10, 10),
        Nursing(tenantId, now, "TASK-4", "James Whitaker", "Oncology", "Vitals check", "Priya Nair", "Administered", 10, 45),
        Nursing(tenantId, now, "TASK-5", "Nina Petrova", "Surgery", "Blood glucose", "Priya Nair", "Due", 11, 20),
        Nursing(tenantId, now, "TASK-6", "Omar Haddad", "Emergency", "Catheter care", "Priya Nair", "Done", 11, 55)
    ];

    public static IReadOnlyList<DiagnosticOrder> DiagnosticOrders(Guid tenantId, DateTimeOffset now) =>
    [
        Diagnostic(tenantId, now, "LAB-90412", "Lab", "Robert Hayes", "Troponin I", null, "STAT", "Resulted", "Grace Okoro", 8, 12),
        Diagnostic(tenantId, now, "LAB-90418", "Lab", "Omar Haddad", "Full blood count", null, "Urgent", "Processing", "Grace Okoro", 8, 40),
        Diagnostic(tenantId, now, "LAB-90423", "Lab", "James Whitaker", "Tumor markers", null, "Routine", "Collected", "Grace Okoro", 9, 5),
        Diagnostic(tenantId, now, "LAB-90431", "Lab", "Nina Petrova", "Liver function", null, "Routine", "Pending", null, 9, 30),
        Diagnostic(tenantId, now, "LAB-90440", "Lab", "Daniel Cho", "Coagulation panel", null, "Urgent", "Resulted", "Grace Okoro", 7, 55),
        Diagnostic(tenantId, now, "IMG-10575", "Radiology", "Nina Petrova", "X-Ray", "Pelvis", "Routine", "Acquired", "Dr. Samuel Boateng", 8, 45),
        Diagnostic(tenantId, now, "IMG-16615", "Radiology", "Robert Hayes", "CT", "Abdomen", "STAT", "Requested", null, 9, 0),
        Diagnostic(tenantId, now, "IMG-13472", "Radiology", "Amelia Brooks", "MRI", "Knee", "Routine", "Reported", "Dr. Samuel Boateng", 10, 10),
        Diagnostic(tenantId, now, "IMG-11853", "Radiology", "Daniel Cho", "Ultrasound", "Pelvis", "Routine", "Acquired", "Dr. Samuel Boateng", 10, 40),
        Diagnostic(tenantId, now, "IMG-11758", "Radiology", "Sofia Reyes", "Mammography", "Head", "Routine", "Scheduled", "Dr. Samuel Boateng", 11, 20),
        Diagnostic(tenantId, now, "IMG-18139", "Radiology", "James Whitaker", "X-Ray", "Chest", "Routine", "Requested", null, 12, 10)
    ];

    public static IReadOnlyList<InventoryItem> Inventory(Guid tenantId, DateTimeOffset now) =>
    [
        Inventory(tenantId, now, "MED-AMOX-500", "Amoxicillin 500mg", "Medication", 1240, 400, "caps", "In stock", new DateOnly(2027, 2, 1)),
        Inventory(tenantId, now, "MED-INS-GLA", "Insulin Glargine", "Medication", 84, 120, "vials", "Low", new DateOnly(2026, 11, 1)),
        Inventory(tenantId, now, "CON-GLO-M", "Surgical Gloves (M)", "Consumable", 26, 200, "boxes", "Critical", new DateOnly(2028, 5, 1)),
        Inventory(tenantId, now, "MED-PARA-IV", "Paracetamol IV", "Medication", 612, 250, "vials", "In stock", new DateOnly(2026, 12, 1)),
        Inventory(tenantId, now, "REA-BLO-CUL", "Blood Culture Reagent", "Reagent", 58, 60, "kits", "Low", new DateOnly(2026, 8, 1)),
        Inventory(tenantId, now, "CON-N95", "N95 Respirators", "Consumable", 940, 300, "units", "In stock", new DateOnly(2029, 1, 1))
    ];

    public static IReadOnlyList<Vendor> Vendors(Guid tenantId, DateTimeOffset now) =>
    [
        Vendor(tenantId, now, "VEN-001", "MedSupply Co.", "Devices", 1, 90),
        Vendor(tenantId, now, "VEN-002", "Boston BioReagents", "Consumables", 2, 97),
        Vendor(tenantId, now, "VEN-003", "CardioTech Devices", "Pharmaceuticals", 1, 88),
        Vendor(tenantId, now, "VEN-004", "PharmaDirect", "Consumables", 1, 89),
        Vendor(tenantId, now, "VEN-005", "SteriPack Medical", "Devices", 0, 94)
    ];

    public static IReadOnlyList<PurchaseOrder> PurchaseOrders(Guid tenantId, DateTimeOffset now) =>
    [
        PurchaseOrder(tenantId, now, "PO-4431", "Insulin Glargine", "MedSupply Co.", 240, 10080, "Received"),
        PurchaseOrder(tenantId, now, "PO-4412", "Surgical Gloves (M)", "Boston BioReagents", 600, 10200, "Approved"),
        PurchaseOrder(tenantId, now, "PO-4526", "Blood Culture Reagent", "CardioTech Devices", 180, 7920, "Approved")
    ];

    public static IReadOnlyList<NotificationMessage> Notifications(Guid tenantId, DateTimeOffset now) =>
    [
        Notification(tenantId, now, "Critical lab", "STAT result ready - Troponin I", "Robert Hayes - LAB-90412 resulted and awaiting review.", "Critical", 8, 0),
        Notification(tenantId, now, "Inventory", "Critical stock - Surgical Gloves (M)", "Only 26 boxes left, reorder at 200.", "Warning", 9, 10),
        Notification(tenantId, now, "Clinical", "Critical patient - Robert Hayes", "Cardiac ICU - Acute myocardial infarction.", "Critical", 9, 35),
        Notification(tenantId, now, "Clinical", "Critical patient - Omar Haddad", "Emergency - Multiple trauma - RTA.", "Critical", 10, 10),
        Notification(tenantId, now, "Scheduling", "Appointment cancelled", "Nina Petrova with Dr. Samuel Boateng was cancelled.", "Info", 10, 20),
        Notification(tenantId, now, "Broadcast", "Policy update published", "Updated infection-control protocol is now in effect.", "Info", 7, 5)
    ];

    public static IReadOnlyList<Invoice> Invoices(Guid tenantId, DateTimeOffset now) =>
    [
        Invoice(tenantId, now, "INV-2026-4471", "Robert Hayes", "Insurance", 18450, new DateOnly(2026, 6, 8), new DateOnly(2026, 7, 8), "Pending"),
        Invoice(tenantId, now, "INV-2026-4468", "Grace Miller", "Medicare", 640, new DateOnly(2026, 6, 5), new DateOnly(2026, 6, 20), "Paid"),
        Invoice(tenantId, now, "INV-2026-4455", "James Whitaker", "Insurance", 32100, new DateOnly(2026, 6, 2), new DateOnly(2026, 7, 2), "Pending"),
        Invoice(tenantId, now, "INV-2026-4402", "Sofia Reyes", "Self-pay", 220, new DateOnly(2026, 5, 20), new DateOnly(2026, 6, 4), "Overdue"),
        Invoice(tenantId, now, "INV-2026-4490", "Nina Petrova", "Insurance", 9870, new DateOnly(2026, 6, 9), new DateOnly(2026, 7, 9), "Draft")
    ];

    public static IReadOnlyList<InsuranceClaim> InsuranceClaims(Guid tenantId, DateTimeOffset now) =>
    [
        Claim(tenantId, now, "CLM-73137", "Robert Hayes", "BlueCross BlueShield", 18450, "Approved"),
        Claim(tenantId, now, "CLM-77280", "Grace Miller", "Aetna", 640, "Paid"),
        Claim(tenantId, now, "CLM-78899", "James Whitaker", "UnitedHealthcare", 32100, "Submitted"),
        Claim(tenantId, now, "CLM-70613", "Nina Petrova", "Medicare", 9870, "Submitted")
    ];

    public static IReadOnlyList<PreAuthorization> PreAuthorizations(Guid tenantId, DateTimeOffset now) =>
    [
        PreAuth(tenantId, now, "Robert Hayes", "Chemotherapy cycle", "Aetna", "Approved"),
        PreAuth(tenantId, now, "Amelia Brooks", "Specialist referral", "UnitedHealthcare", "Pending"),
        PreAuth(tenantId, now, "Daniel Cho", "Inpatient admission", "Medicare", "Under review")
    ];

    public static IReadOnlyList<TelemedicineSession> TelemedicineSessions(Guid tenantId, DateTimeOffset now) =>
    [
        Session(tenantId, now, "TEL-1001", "Sofia Reyes", "Dr. Aisha Khan", "Video", "Scheduled", 24, 8, 30),
        Session(tenantId, now, "TEL-1002", "Robert Hayes", "Dr. Helen Acharya", "Phone", "Waiting", 17, 9, 0),
        Session(tenantId, now, "TEL-1003", "Grace Miller", "Dr. Helen Acharya", "Chat", "Scheduled", 29, 10, 15),
        Session(tenantId, now, "TEL-1004", "Amelia Brooks", "Dr. Aisha Khan", "Video", "Completed", 10, 13, 0)
    ];

    public static IReadOnlyList<ComplianceControl> ComplianceControls(Guid tenantId, DateTimeOffset now) =>
    [
        Control(tenantId, now, "Access controls", "Role-based access enforced across modules.", "Compliant"),
        Control(tenantId, now, "Data retention policy", "Records retained per approved retention schedule.", "Compliant"),
        Control(tenantId, now, "Quarterly access review", "Next review is due in 9 days.", "Due"),
        Control(tenantId, now, "Encryption at rest", "AES-256 target control on protected data stores.", "Compliant"),
        Control(tenantId, now, "Incident response drill", "Last drill is overdue.", "Warning")
    ];

    public static IReadOnlyList<AuditLog> AuditLogs(Guid tenantId, DateTimeOffset now) =>
    [
        Audit(tenantId, now, "Dr. Helen Acharya", "Changed user permissions", "Report export", "REP-1", "Warning", 6, 0),
        Audit(tenantId, now, "Dr. Marcus Lin", "Granted system access", "Patient record", "MRN-557991", "Denied", 6, 35),
        Audit(tenantId, now, "Priya Nair", "Signed clinical note", "Pharmacy stock", "MED-AMOX-500", "Success", 7, 10),
        Audit(tenantId, now, "Grace Okoro", "Voided draft invoice", "User directory", "EMP-2750", "Success", 10, 5),
        Audit(tenantId, now, "Maria Santos", "Exported report", "Patient record", "MRN-557720", "Success", 11, 15)
    ];

    public static IReadOnlyList<ReportMetric> ReportMetrics(Guid tenantId, DateTimeOffset now) =>
    [
        Report(tenantId, now, "Operations", "Total revenue", "$2.4M", "+5.2%"),
        Report(tenantId, now, "Operations", "Total admissions", "671", "+1.1%"),
        Report(tenantId, now, "Operations", "Average length of stay", "4.2 days", "-0.3 days"),
        Report(tenantId, now, "Operations", "Readmission rate", "8.4%", "-1.2%"),
        Report(tenantId, now, "HR", "Active staff", "9", "1 on leave"),
        Report(tenantId, now, "HR", "Open candidates", "3", "2 interviews")
    ];

    public static IReadOnlyList<ShiftAssignment> Shifts(Guid tenantId, DateTimeOffset now) =>
    [
        Shift(tenantId, now, "SH-1001", "Priya Nair", "Emergency", "Active", 7, 19),
        Shift(tenantId, now, "SH-1002", "Grace Okoro", "Pathology", "Active", 8, 16),
        Shift(tenantId, now, "SH-1003", "Maria Santos", "Pharmacy", "Scheduled", 12, 20)
    ];

    public static IReadOnlyList<AttendanceRecord> Attendance(Guid tenantId, DateTimeOffset now) =>
    [
        Attendance(tenantId, now, "Priya Nair", "Emergency", "Present"),
        Attendance(tenantId, now, "Grace Okoro", "Pathology", "Present"),
        Attendance(tenantId, now, "Elena Rossi", "Surgery", "On leave")
    ];

    public static IReadOnlyList<LeaveRequest> LeaveRequests(Guid tenantId, DateTimeOffset now) =>
    [
        Leave(tenantId, now, "LR-1001", "Elena Rossi", "Annual leave", "Approved", "Dr. Tomas Becker"),
        Leave(tenantId, now, "LR-1002", "Maria Santos", "Sick leave", "Pending", "HR Manager")
    ];

    public static IReadOnlyList<RecruitmentCandidate> Candidates(Guid tenantId, DateTimeOffset now) =>
    [
        Candidate(tenantId, now, "CAN-1001", "Ada Mensah", "Senior Nurse", "Nursing", "Interview"),
        Candidate(tenantId, now, "CAN-1002", "Kemi Johnson", "Payroll Officer", "Finance", "Screening"),
        Candidate(tenantId, now, "CAN-1003", "Victor Cole", "Biomedical Engineer", "Operations", "Offer")
    ];

    public static IReadOnlyList<TrainingRecord> Training(Guid tenantId, DateTimeOffset now) =>
    [
        Training(tenantId, now, "Priya Nair", "Infection control", "Completed"),
        Training(tenantId, now, "Grace Okoro", "Lab safety refresh", "Due"),
        Training(tenantId, now, "Maria Santos", "Medication reconciliation", "Due")
    ];

    public static IReadOnlyList<CredentialRecord> Credentials(Guid tenantId, DateTimeOffset now) =>
    [
        Credential(tenantId, now, "Dr. Helen Acharya", "Medical license", "Active"),
        Credential(tenantId, now, "Priya Nair", "Nursing license", "Expiring soon"),
        Credential(tenantId, now, "Grace Okoro", "Lab scientist certification", "Active")
    ];

    public static IReadOnlyList<PerformanceReview> Reviews(Guid tenantId, DateTimeOffset now) =>
    [
        Review(tenantId, now, "Priya Nair", "2026 H1", 4.6m, "Finalized", "Elena Rossi"),
        Review(tenantId, now, "Grace Okoro", "2026 H1", 4.3m, "Draft", "Dr. Helen Acharya")
    ];

    private static OperationalModule Module(Guid tenantId, DateTimeOffset now, int sort, string key, string label, string route, string group, string icon, string title, string subtitle) => new()
    {
        TenantId = tenantId,
        CreatedAt = now,
        SortOrder = sort,
        Key = key,
        Label = label,
        Route = route,
        Group = group,
        Icon = icon,
        Title = title,
        Subtitle = subtitle
    };

    private static Department Department(Guid tenantId, DateTimeOffset now, string code, string name, string type, string head, int staff, int beds, decimal occupancy) => new() { TenantId = tenantId, CreatedAt = now, Code = code, Name = name, Type = type, HeadName = head, StaffCount = staff, BedCount = beds, OccupancyRate = occupancy };
    private static Patient Patient(Guid tenantId, DateTimeOffset now, string mrn, string name, DateOnly dob, string sex, string status) => new() { TenantId = tenantId, CreatedAt = now, Mrn = mrn, FullName = name, DateOfBirth = dob, Sex = sex, Status = status };
    private static StaffMember Staff(Guid tenantId, DateTimeOffset now, string number, string name, string department, string title, string type, string status) => new() { TenantId = tenantId, CreatedAt = now, EmployeeNumber = number, FullName = name, Department = department, JobTitle = title, EmploymentType = type, Status = status, WorkEmail = $"{name.ToLowerInvariant().Replace(" ", ".")}@meridian.org", StartDate = new DateOnly(2022, 1, 10) };
    private static Ward Ward(Guid tenantId, DateTimeOffset now, string code, string name, string department, int total, int occupied) => new() { TenantId = tenantId, CreatedAt = now, Code = code, Name = name, Department = department, TotalBeds = total, OccupiedBeds = occupied };
    private static Appointment Appointment(Guid tenantId, DateTimeOffset now, string number, string mrn, string patient, string provider, string department, string type, string room, string status, int hour, int minute) => new() { TenantId = tenantId, CreatedAt = now, AppointmentNumber = number, PatientMrn = mrn, PatientName = patient, ProviderName = provider, Department = department, VisitType = type, Room = room, Status = status, ScheduledAt = Today.AddHours(hour).AddMinutes(minute) };
    private static PatientEncounter Encounter(Guid tenantId, DateTimeOffset now, string number, string patient, string entry, string stage, string complaint, string payer, string? insurer, string? provider, int wait) => new() { TenantId = tenantId, CreatedAt = now, EncounterNumber = number, PatientName = patient, EntryPoint = entry, Stage = stage, Complaint = complaint, PayerType = payer, Insurer = insurer, AssignedProvider = provider, WaitMinutes = wait, ArrivedAt = Today.AddMinutes(-wait) };
    private static Admission Admission(Guid tenantId, DateTimeOffset now, string number, string mrn, string patient, string ward, string bed, string provider, string status, string diagnosis, int days) => new() { TenantId = tenantId, CreatedAt = now, AdmissionNumber = number, PatientMrn = mrn, PatientName = patient, WardName = ward, BedNumber = bed, AttendingProvider = provider, Status = status, Diagnosis = diagnosis, AdmittedAt = Today.AddDays(days) };
    private static EmergencyCase Emergency(Guid tenantId, DateTimeOffset now, string number, string patient, string acuity, string complaint, int hour, int minute, string status) => new() { TenantId = tenantId, CreatedAt = now, CaseNumber = number, PatientName = patient, Acuity = acuity, Complaint = complaint, ArrivedAt = Today.Date.AddHours(hour).AddMinutes(minute), WaitMinutes = (int)(Today - Today.Date.AddHours(hour).AddMinutes(minute)).TotalMinutes, Status = status };
    private static SurgeryCase Surgery(Guid tenantId, DateTimeOffset now, string number, string patient, string procedure, string surgeon, string theatre, string status, int hour, int minute) => new() { TenantId = tenantId, CreatedAt = now, CaseNumber = number, PatientName = patient, Procedure = procedure, SurgeonName = surgeon, Theatre = theatre, Status = status, ScheduledAt = Today.Date.AddHours(hour).AddMinutes(minute) };
    private static NursingTask Nursing(Guid tenantId, DateTimeOffset now, string number, string patient, string ward, string task, string nurse, string status, int hour, int minute) => new() { TenantId = tenantId, CreatedAt = now, TaskNumber = number, PatientName = patient, WardName = ward, TaskType = task, NurseName = nurse, Status = status, DueAt = Today.Date.AddHours(hour).AddMinutes(minute) };
    private static DiagnosticOrder Diagnostic(Guid tenantId, DateTimeOffset now, string number, string category, string patient, string test, string? part, string priority, string status, string? assigned, int hour, int minute) => new() { TenantId = tenantId, CreatedAt = now, OrderNumber = number, Category = category, PatientName = patient, TestName = test, BodyPart = part, Priority = priority, Status = status, AssignedTo = assigned, OrderedAt = Today.Date.AddHours(hour).AddMinutes(minute) };
    private static InventoryItem Inventory(Guid tenantId, DateTimeOffset now, string sku, string name, string category, int quantity, int reorder, string unit, string status, DateOnly expiry) => new() { TenantId = tenantId, CreatedAt = now, Sku = sku, Name = name, Category = category, QuantityOnHand = quantity, ReorderLevel = reorder, Unit = unit, Status = status, ExpiryDate = expiry };
    private static Vendor Vendor(Guid tenantId, DateTimeOffset now, string code, string name, string category, int open, decimal onTime) => new() { TenantId = tenantId, CreatedAt = now, VendorCode = code, Name = name, Category = category, OpenOrders = open, OnTimeRate = onTime };
    private static PurchaseOrder PurchaseOrder(Guid tenantId, DateTimeOffset now, string number, string item, string vendor, int quantity, decimal total, string status) => new() { TenantId = tenantId, CreatedAt = now, PurchaseOrderNumber = number, ItemName = item, VendorName = vendor, Quantity = quantity, TotalAmount = total, Status = status, ExpectedDate = new DateOnly(2026, 6, 18) };
    private static NotificationMessage Notification(Guid tenantId, DateTimeOffset now, string category, string title, string body, string severity, int hour, int minute) => new() { TenantId = tenantId, CreatedAt = now, Category = category, Title = title, Body = body, Severity = severity, Status = "Unread", PublishedAt = Today.Date.AddHours(hour).AddMinutes(minute) };
    private static Invoice Invoice(Guid tenantId, DateTimeOffset now, string number, string patient, string payer, decimal amount, DateOnly issued, DateOnly due, string status) => new() { TenantId = tenantId, CreatedAt = now, InvoiceNumber = number, PatientName = patient, PayerType = payer, Amount = amount, IssuedOn = issued, DueOn = due, Status = status };
    private static InsuranceClaim Claim(Guid tenantId, DateTimeOffset now, string number, string patient, string insurer, decimal amount, string status) => new() { TenantId = tenantId, CreatedAt = now, ClaimNumber = number, PatientName = patient, InsurerName = insurer, Amount = amount, SubmittedOn = new DateOnly(2026, 6, 8), Status = status };
    private static PreAuthorization PreAuth(Guid tenantId, DateTimeOffset now, string patient, string service, string insurer, string status) => new() { TenantId = tenantId, CreatedAt = now, PatientName = patient, ServiceName = service, InsurerName = insurer, Status = status };
    private static TelemedicineSession Session(Guid tenantId, DateTimeOffset now, string number, string patient, string provider, string channel, string status, int duration, int hour, int minute) => new() { TenantId = tenantId, CreatedAt = now, SessionNumber = number, PatientName = patient, ProviderName = provider, Channel = channel, Status = status, DurationMinutes = duration, ScheduledAt = Today.Date.AddHours(hour).AddMinutes(minute) };
    private static ComplianceControl Control(Guid tenantId, DateTimeOffset now, string name, string description, string status) => new() { TenantId = tenantId, CreatedAt = now, Name = name, Description = description, Status = status, DueOn = status == "Compliant" ? null : new DateOnly(2026, 6, 20) };
    private static AuditLog Audit(Guid tenantId, DateTimeOffset now, string actor, string action, string type, string id, string result, int hour, int minute) => new() { TenantId = tenantId, CreatedAt = Today.Date.AddHours(hour).AddMinutes(minute), Actor = actor, Action = action, TargetType = type, TargetId = id, Result = result };
    private static ReportMetric Report(Guid tenantId, DateTimeOffset now, string area, string name, string value, string trend) => new() { TenantId = tenantId, CreatedAt = now, Area = area, Name = name, Value = value, Trend = trend, PeriodStart = new DateOnly(2026, 6, 1), PeriodEnd = new DateOnly(2026, 6, 7) };
    private static ShiftAssignment Shift(Guid tenantId, DateTimeOffset now, string number, string staff, string department, string status, int start, int end) => new() { TenantId = tenantId, CreatedAt = now, ShiftNumber = number, StaffName = staff, Department = department, Status = status, StartsAt = Today.Date.AddHours(start), EndsAt = Today.Date.AddHours(end) };
    private static AttendanceRecord Attendance(Guid tenantId, DateTimeOffset now, string staff, string department, string status) => new() { TenantId = tenantId, CreatedAt = now, StaffName = staff, Department = department, WorkDate = new DateOnly(2026, 6, 11), ClockIn = status == "Present" ? new TimeOnly(7, 55) : null, ClockOut = null, Status = status };
    private static LeaveRequest Leave(Guid tenantId, DateTimeOffset now, string number, string staff, string type, string status, string approver) => new() { TenantId = tenantId, CreatedAt = now, RequestNumber = number, StaffName = staff, LeaveType = type, StartsOn = new DateOnly(2026, 6, 10), EndsOn = new DateOnly(2026, 6, 14), Status = status, ApproverName = approver };
    private static RecruitmentCandidate Candidate(Guid tenantId, DateTimeOffset now, string number, string name, string position, string department, string stage) => new() { TenantId = tenantId, CreatedAt = now, CandidateNumber = number, FullName = name, Position = position, Department = department, Stage = stage };
    private static TrainingRecord Training(Guid tenantId, DateTimeOffset now, string staff, string course, string status) => new() { TenantId = tenantId, CreatedAt = now, StaffName = staff, CourseName = course, DueOn = new DateOnly(2026, 6, 30), CompletedOn = status == "Completed" ? new DateOnly(2026, 6, 7) : null, Status = status };
    private static CredentialRecord Credential(Guid tenantId, DateTimeOffset now, string staff, string credential, string status) => new() { TenantId = tenantId, CreatedAt = now, StaffName = staff, CredentialName = credential, ExpiresOn = new DateOnly(2026, 12, 31), Status = status };
    private static PerformanceReview Review(Guid tenantId, DateTimeOffset now, string staff, string period, decimal score, string status, string reviewer) => new() { TenantId = tenantId, CreatedAt = now, StaffName = staff, ReviewPeriod = period, Score = score, Status = status, ReviewerName = reviewer };
}
