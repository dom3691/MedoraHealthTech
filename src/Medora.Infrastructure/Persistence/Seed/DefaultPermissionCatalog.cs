namespace Medora.Infrastructure.Persistence.Seed;

public sealed record PermissionSeed(string Module, string Action, string Description)
{
    public string Code => $"{Normalize(Module)}.{Normalize(Action)}";

    private static string Normalize(string value) => value
        .Replace("&", "and", StringComparison.OrdinalIgnoreCase)
        .Replace("/", "-", StringComparison.OrdinalIgnoreCase)
        .Replace(" ", "-", StringComparison.OrdinalIgnoreCase)
        .ToLowerInvariant();
}

public sealed record RoleSeed(string Code, string Name, string Description, string[] PermissionPatterns);

public static class DefaultPermissionCatalog
{
    public static readonly string[] CoreActions = ["view", "create", "edit", "delete", "approve", "export", "administer"];

    public static readonly string[] Modules =
    [
        "Dashboard",
        "Patient Flow",
        "Appointments",
        "Outpatient",
        "Patients EHR",
        "Inpatient",
        "Emergency",
        "Wards Beds",
        "Theatre Surgery",
        "Nursing",
        "Lab Diagnostics",
        "Radiology Imaging",
        "Pharmacy Inventory",
        "Doctor Portal",
        "Patient Portal",
        "Telemedicine",
        "Procurement Vendors",
        "Notifications",
        "Staff Master",
        "Departments Positions",
        "Recruitment",
        "Onboarding",
        "Shift Scheduling",
        "Attendance",
        "Leave Management",
        "Payroll Inputs",
        "Credentialing",
        "Training",
        "Performance",
        "Disciplinary Grievance",
        "HR Documents",
        "Employee Self Service",
        "Billing Invoicing",
        "HMO Insurance",
        "Reports Analytics",
        "Admin Settings",
        "Audit Compliance"
    ];

    public static IReadOnlyList<PermissionSeed> Permissions { get; } = Modules
        .SelectMany(module => CoreActions.Select(action => new PermissionSeed(module, action, $"{action} {module}")))
        .Concat([
            new PermissionSeed("Patients EHR", "self-view", "view own patient health record"),
            new PermissionSeed("Appointments", "self-view", "view own appointments"),
            new PermissionSeed("Patient Portal", "self-view", "view own patient portal"),
            new PermissionSeed("Employee Self Service", "self-view", "view own employee self-service data"),
            new PermissionSeed("Leave Management", "self-request", "request own leave"),
            new PermissionSeed("Training", "self-view", "view own training records")
        ])
        .ToArray();

    public static IReadOnlyList<RoleSeed> Roles { get; } =
    [
        new("MSA", "Medora Super Admin", "Medora platform administration, client tenant onboarding, and cross-tenant oversight.", ["*.*"]),
        new("SA", "Tenant Super Admin", "Tenant configuration, employee onboarding, roles, permissions, and audit oversight within one hospital tenant.", ["Dashboard.view", "Staff Master.*", "Departments Positions.*", "Onboarding.*", "Shift Scheduling.*", "Attendance.*", "Leave Management.*", "Payroll Inputs.*", "Credentialing.*", "Training.*", "Performance.*", "Disciplinary Grievance.*", "HR Documents.*", "Employee Self Service.view", "Admin Settings.view", "Admin Settings.edit", "Audit Compliance.view", "Reports Analytics.view", "Reports Analytics.export"]),
        new("MD", "Medical Director", "Clinical oversight, approvals, and clinical reporting.", ["Dashboard.view", "Patients EHR.view", "Patients EHR.edit", "Inpatient.view", "Emergency.view", "Theatre Surgery.view", "Theatre Surgery.approve", "Lab Diagnostics.view", "Radiology Imaging.view", "Reports Analytics.view", "Reports Analytics.export"]),
        new("PHY", "Physician", "Patient care, orders, notes, doctor portal, and results review.", ["Dashboard.view", "Patient Flow.view", "Patient Flow.edit", "Appointments.view", "Appointments.edit", "Patients EHR.view", "Patients EHR.edit", "Inpatient.view", "Inpatient.edit", "Emergency.view", "Emergency.edit", "Lab Diagnostics.view", "Lab Diagnostics.create", "Radiology Imaging.view", "Radiology Imaging.create", "Doctor Portal.*", "Telemedicine.view", "Telemedicine.create", "Telemedicine.edit"]),
        new("NUR", "Nurse", "Vitals, nursing tasks, wards, beds, and shift-linked care work.", ["Dashboard.view", "Patient Flow.view", "Patient Flow.edit", "Patients EHR.view", "Patients EHR.edit", "Inpatient.view", "Emergency.view", "Wards Beds.view", "Wards Beds.edit", "Nursing.*", "Shift Scheduling.view"]),
        new("FD", "Front Desk", "Registration, appointments, outpatient queue, and intake.", ["Dashboard.view", "Patient Flow.*", "Appointments.*", "Outpatient.*", "Patients EHR.view", "Patients EHR.create", "Emergency.view", "Emergency.create", "Billing Invoicing.view"]),
        new("LAB", "Lab Technician", "Lab worklist, sample processing, and result entry.", ["Dashboard.view", "Lab Diagnostics.view", "Lab Diagnostics.edit", "Lab Diagnostics.create", "Notifications.view"]),
        new("RAD", "Radiology Staff", "Imaging worklist, acquisition, and reporting.", ["Dashboard.view", "Radiology Imaging.view", "Radiology Imaging.edit", "Radiology Imaging.create", "Notifications.view"]),
        new("PHA", "Pharmacist", "Medication dispensing, stock control, and reorder flags.", ["Dashboard.view", "Pharmacy Inventory.*", "Procurement Vendors.view", "Notifications.view"]),
        new("FIN", "Finance Officer", "Billing, invoices, claims, payments, exports, and payroll input review.", ["Dashboard.view", "Billing Invoicing.*", "HMO Insurance.*", "Payroll Inputs.view", "Payroll Inputs.export", "Reports Analytics.view", "Reports Analytics.export"]),
        new("HR", "HR Manager", "Staff lifecycle, rosters, attendance, leave, payroll inputs, credentials, training, and HR records.", ["Dashboard.view", "Staff Master.*", "Departments Positions.*", "Recruitment.*", "Onboarding.*", "Shift Scheduling.*", "Attendance.*", "Leave Management.*", "Payroll Inputs.*", "Credentialing.*", "Training.*", "Performance.*", "Disciplinary Grievance.*", "HR Documents.*", "Employee Self Service.view", "Reports Analytics.view", "Reports Analytics.export"]),
        new("DH", "Department Head", "Department workforce oversight, leave approval, rosters, and performance input.", ["Dashboard.view", "Staff Master.view", "Departments Positions.view", "Shift Scheduling.view", "Shift Scheduling.edit", "Attendance.view", "Leave Management.view", "Leave Management.approve", "Training.view", "Performance.view", "Performance.edit", "Reports Analytics.view"]),
        new("PRO", "Procurement Officer", "Vendors, purchase orders, and inventory replenishment.", ["Dashboard.view", "Procurement Vendors.*", "Pharmacy Inventory.view", "Notifications.view"]),
        new("AUD", "Audit Officer", "Read-only audit and compliance reporting.", ["Dashboard.view", "Audit Compliance.view", "Reports Analytics.view", "Reports Analytics.export"]),
        new("EMP", "Employee", "Employee self-service for own HR data.", ["Employee Self Service.self-view", "Leave Management.self-request", "Training.self-view", "Shift Scheduling.view", "Notifications.view"]),
        new("PT", "Patient", "Patient self-service for own patient data.", ["Patient Portal.self-view", "Patients EHR.self-view", "Appointments.self-view", "Telemedicine.view", "Notifications.view"])
    ];
}
