using Medora.Application.Abstractions;
using Medora.Application.Screens;
using Medora.Domain.Entities;
using Medora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Medora.Infrastructure.Screens;

public sealed class EfScreenCatalog(MedoraDbContext dbContext, IDateTimeProvider clock) : IScreenCatalog
{
    public async Task<ScreenCatalogResponse> GetCatalogAsync(string? tenantCode = null, CancellationToken cancellationToken = default)
    {
        var tenant = await ResolveTenantAsync(tenantCode, cancellationToken);
        var modules = await dbContext.OperationalModules
            .AsNoTracking()
            .Where(x => x.TenantId == tenant.Id && x.IsActive && !x.IsDeleted)
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);

        var screens = new List<ScreenDefinition>(modules.Count);
        foreach (var module in modules)
        {
            screens.Add(await BuildScreenAsync(module, cancellationToken));
        }

        var navigation = modules
            .GroupBy(x => x.Group)
            .Select(group => new NavigationGroup(
                group.Key,
                group.Select(x => new NavigationItem(x.Key, x.Label, x.Route, x.Icon)).ToArray()))
            .ToArray();

        return new ScreenCatalogResponse(navigation, screens);
    }

    public async Task<ScreenDefinition?> GetScreenAsync(string key, string? tenantCode = null, CancellationToken cancellationToken = default)
    {
        var tenant = await ResolveTenantAsync(tenantCode, cancellationToken);
        var module = await dbContext.OperationalModules
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TenantId == tenant.Id && x.Key == key && x.IsActive && !x.IsDeleted, cancellationToken);

        return module is null ? null : await BuildScreenAsync(module, cancellationToken);
    }

    private async Task<Tenant> ResolveTenantAsync(string? tenantCode, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(tenantCode))
        {
            var normalized = tenantCode.Trim();
            var tenant = await dbContext.Tenants
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Code == normalized && x.IsActive, cancellationToken);

            if (tenant is not null)
            {
                return tenant;
            }
        }

        return await dbContext.Tenants
            .AsNoTracking()
            .OrderBy(x => x.Code)
            .FirstAsync(x => x.IsActive, cancellationToken);
    }

    private async Task<ScreenDefinition> BuildScreenAsync(OperationalModule module, CancellationToken cancellationToken)
    {
        var tenantId = module.TenantId;
        var metrics = await BuildMetricsAsync(module.Key, tenantId, cancellationToken);
        var sections = await BuildSectionsAsync(module.Key, tenantId, cancellationToken);

        return new ScreenDefinition(
            module.Key,
            module.Route,
            module.Title,
            module.Subtitle,
            module.Group,
            metrics,
            BuildActions(module.Key),
            sections);
    }

    private async Task<IReadOnlyList<MetricCard>> BuildMetricsAsync(string key, Guid tenantId, CancellationToken cancellationToken)
    {
        var patients = dbContext.Patients.AsNoTracking().Where(x => x.TenantId == tenantId && !x.IsDeleted);
        var admissions = dbContext.Admissions.AsNoTracking().Where(x => x.TenantId == tenantId && !x.IsDeleted);
        var appointments = dbContext.Appointments.AsNoTracking().Where(x => x.TenantId == tenantId && !x.IsDeleted);
        var beds = dbContext.Beds.AsNoTracking().Where(x => x.TenantId == tenantId && !x.IsDeleted);
        var wards = dbContext.Wards.AsNoTracking().Where(x => x.TenantId == tenantId && !x.IsDeleted);
        var lab = dbContext.DiagnosticOrders.AsNoTracking().Where(x => x.TenantId == tenantId && x.Category == "Lab" && !x.IsDeleted);
        var radiology = dbContext.DiagnosticOrders.AsNoTracking().Where(x => x.TenantId == tenantId && x.Category == "Radiology" && !x.IsDeleted);
        var inventory = dbContext.InventoryItems.AsNoTracking().Where(x => x.TenantId == tenantId && !x.IsDeleted);
        var invoices = dbContext.Invoices.AsNoTracking().Where(x => x.TenantId == tenantId && !x.IsDeleted);
        var staff = dbContext.StaffMembers.AsNoTracking().Where(x => x.TenantId == tenantId && !x.IsDeleted);
        var shifts = dbContext.ShiftAssignments.AsNoTracking().Where(x => x.TenantId == tenantId && !x.IsDeleted);
        var emergency = dbContext.EmergencyCases.AsNoTracking().Where(x => x.TenantId == tenantId && !x.IsDeleted);

        var occupiedBeds = await beds.CountAsync(x => x.Status == "Occupied", cancellationToken);
        var totalBeds = await beds.CountAsync(cancellationToken);
        var occupancy = totalBeds == 0 ? 0 : (int)Math.Round((decimal)occupiedBeds / totalBeds * 100);
        var averageEmergencyWait = await emergency.AnyAsync(cancellationToken)
            ? await emergency.AverageAsync(x => (double)x.WaitMinutes, cancellationToken)
            : 0;

        return key switch
        {
            "dashboard" =>
            [
                Metric("Patients Today", await patients.CountAsync(cancellationToken), "+12% from yesterday", "success", "users"),
                Metric("Admissions", await admissions.CountAsync(cancellationToken), "+5 expected", "info", "trend"),
                Metric("Bed Occupancy", $"{occupancy}%", "Target < 85%", occupancy > 85 ? "warning" : "success", "bed"),
                Metric("ED Wait Time", $"{averageEmergencyWait:0} min", "-4 min vs avg", "success", "clock"),
                Metric("Revenue MTD", Money(await invoices.SumAsync(x => x.Amount, cancellationToken)), "+2.4% vs last month", "success", "dollar"),
                Metric("Staff On Duty", await shifts.CountAsync(x => x.Status == "Active", cancellationToken), "98% fulfillment", "success", "stethoscope")
            ],
            "patients" =>
            [
                Metric("Total Patients", await patients.CountAsync(cancellationToken), null, "neutral", "users"),
                Metric("Admitted", await patients.CountAsync(x => x.Status == "Admitted", cancellationToken), null, "info", "hospital"),
                Metric("Critical", await patients.CountAsync(x => x.Status == "Critical", cancellationToken), null, "danger", "alert"),
                Metric("Outpatient", await patients.CountAsync(x => x.Status == "Outpatient", cancellationToken), null, "neutral", "clipboard")
            ],
            "appointments" or "outpatient" =>
            [
                Metric("Today's Total", await appointments.CountAsync(cancellationToken), null, "neutral", "calendar"),
                Metric("Completed", await appointments.CountAsync(x => x.Status == "Completed", cancellationToken), null, "success", "check"),
                Metric("In Progress", await appointments.CountAsync(x => x.Status == "In progress", cancellationToken), null, "warning", "clock"),
                Metric("Cancelled", await appointments.CountAsync(x => x.Status == "Cancelled", cancellationToken), null, "danger", "x")
            ],
            "patient-flow" =>
            [
                Metric("Awaiting Vitals", await dbContext.PatientEncounters.CountAsync(x => x.TenantId == tenantId && x.Stage == "Awaiting vitals", cancellationToken), "Attendant floor", "info", "pulse"),
                Metric("Awaiting Doctor", await dbContext.PatientEncounters.CountAsync(x => x.TenantId == tenantId && x.Stage == "Awaiting doctor", cancellationToken), "Consultation queue", "warning", "stethoscope"),
                Metric("Completed", await dbContext.PatientEncounters.CountAsync(x => x.TenantId == tenantId && x.Stage == "Completed", cancellationToken), "Dispositioned today", "success", "check"),
                Metric("HMO Covered", "60%", "3 of 5 visits", "neutral", "shield")
            ],
            "inpatient" =>
            [
                Metric("Admitted", await admissions.CountAsync(cancellationToken), null, "neutral", "hospital"),
                Metric("Critical", await admissions.CountAsync(x => x.Status == "Critical", cancellationToken), "Requires close watch", "danger", "pulse"),
                Metric("Bed Occupancy", $"{await WardOccupancyAsync(wards, cancellationToken)}%", "Across all wards", "warning", "bed"),
                Metric("Discharges Due", await admissions.CountAsync(x => x.DischargePlannedAt != null, cancellationToken), "Planned today", "info", "exit")
            ],
            "emergency" =>
            [
                Metric("In Department", await dbContext.EmergencyCases.CountAsync(x => x.TenantId == tenantId, cancellationToken), null, "neutral", "ambulance"),
                Metric("Critical", await dbContext.EmergencyCases.CountAsync(x => x.TenantId == tenantId && (x.Acuity == "Resuscitation" || x.Acuity == "Emergent"), cancellationToken), "High acuity", "danger", "alert"),
                Metric("Avg Wait", $"{averageEmergencyWait:0} min", "To treatment", "warning", "clock"),
                Metric("Awaiting Triage", await dbContext.EmergencyCases.CountAsync(x => x.TenantId == tenantId && x.Status == "Awaiting triage", cancellationToken), "Unassessed", "info", "list")
            ],
            "wards-beds" =>
            [
                Metric("Total Beds", totalBeds, null, "neutral", "bed"),
                Metric("Occupied", occupiedBeds, $"{occupancy}% occupancy", "info", "users"),
                Metric("Available", await beds.CountAsync(x => x.Status == "Available", cancellationToken), "Ready for admission", "success", "available"),
                Metric("Cleaning", await beds.CountAsync(x => x.Status == "Cleaning", cancellationToken), null, "warning", "alert")
            ],
            "lab-diagnostics" =>
            [
                Metric("Total Orders", await lab.CountAsync(cancellationToken), null, "neutral", "flask"),
                Metric("Pending", await lab.CountAsync(x => x.Status == "Pending", cancellationToken), "Needs collection", "warning", "clock"),
                Metric("STAT Priority", await lab.CountAsync(x => x.Priority == "STAT", cancellationToken), "Critical turnaround", "danger", "alert"),
                Metric("Resulted", await lab.CountAsync(x => x.Status == "Resulted", cancellationToken), "Ready for review", "success", "check")
            ],
            "radiology-imaging" =>
            [
                Metric("Studies", await radiology.CountAsync(cancellationToken), null, "neutral", "scan"),
                Metric("Pending", await radiology.CountAsync(x => x.Status == "Requested", cancellationToken), "Requested", "warning", "clock"),
                Metric("Reported", await radiology.CountAsync(x => x.Status == "Reported", cancellationToken), "Ready", "success", "report"),
                Metric("Critical Findings", await radiology.CountAsync(x => x.Finding != null, cancellationToken), "Flagged", "danger", "alert")
            ],
            "pharmacy-inventory" =>
            [
                Metric("Total SKUs", await inventory.CountAsync(cancellationToken), null, "neutral", "box"),
                Metric("Low Stock", await inventory.CountAsync(x => x.Status == "Low", cancellationToken), "Requires attention", "warning", "pill"),
                Metric("Critical Stock", await inventory.CountAsync(x => x.Status == "Critical", cancellationToken), "Reorder immediately", "danger", "alert"),
                Metric("Expiring Soon", await inventory.CountAsync(x => x.ExpiryDate != null && x.ExpiryDate <= Today().AddDays(30), cancellationToken), "Within 30 days", "warning", "alert")
            ],
            "staff" or "hr-workflows" =>
            [
                Metric("Total Staff", await staff.CountAsync(cancellationToken), null, "neutral", "staff"),
                Metric("Employees", await staff.CountAsync(x => x.EmploymentType == "Employee", cancellationToken), null, "success", "users"),
                Metric("Consultants", await staff.CountAsync(x => x.EmploymentType == "Consultant", cancellationToken), null, "info", "briefcase"),
                Metric("On Leave", await staff.CountAsync(x => x.Status == "On leave", cancellationToken), null, "warning", "leave")
            ],
            "billing-invoicing" =>
            [
                Metric("Revenue MTD", Money(await invoices.SumAsync(x => x.Amount, cancellationToken)), null, "success", "dollar"),
                Metric("Paid", Money(await invoices.Where(x => x.Status == "Paid").SumAsync(x => x.Amount, cancellationToken)), null, "success", "check"),
                Metric("Outstanding", Money(await invoices.Where(x => x.Status != "Paid").SumAsync(x => x.Amount, cancellationToken)), null, "warning", "invoice"),
                Metric("Overdue", Money(await invoices.Where(x => x.Status == "Overdue").SumAsync(x => x.Amount, cancellationToken)), null, "danger", "alert")
            ],
            _ => []
        };
    }

    private async Task<IReadOnlyList<ScreenSection>> BuildSectionsAsync(string key, Guid tenantId, CancellationToken cancellationToken)
    {
        return key switch
        {
            "dashboard" => await DashboardSectionsAsync(tenantId, cancellationToken),
            "patients" => [await PatientsTableAsync(tenantId, "Patient Registry", cancellationToken)],
            "appointments" or "outpatient" => [await AppointmentsTableAsync(tenantId, key == "outpatient" ? "Clinic Schedule" : "Schedule", cancellationToken)],
            "patient-flow" => [FlowSteps(), await PatientFlowBoardAsync(tenantId, cancellationToken)],
            "inpatient" => [await AdmissionsTableAsync(tenantId, "Admitted Patients", cancellationToken), await WardBarsAsync(tenantId, "Ward Occupancy", cancellationToken)],
            "emergency" => [await EmergencyTableAsync(tenantId, cancellationToken)],
            "wards-beds" => [await WardBarsAsync(tenantId, "Ward Overview", cancellationToken), await BedCardsAsync(tenantId, cancellationToken)],
            "theatre-surgery" => [await SurgeriesTableAsync(tenantId, cancellationToken)],
            "nursing" => [await NursingTableAsync(tenantId, cancellationToken), await StaffingTableAsync(tenantId, cancellationToken)],
            "lab-diagnostics" => [await DiagnosticsTableAsync(tenantId, "Lab", "Active Orders", cancellationToken)],
            "radiology-imaging" => [await DiagnosticsTableAsync(tenantId, "Radiology", "Imaging Worklist", cancellationToken)],
            "pharmacy-inventory" => [await InventoryTableAsync(tenantId, cancellationToken)],
            "doctor-portal" => await DoctorSectionsAsync(tenantId, cancellationToken),
            "patient-portal" => await PatientPortalSectionsAsync(tenantId, cancellationToken),
            "telemedicine" => [await TelemedicineTableAsync(tenantId, cancellationToken)],
            "procurement-vendors" => [await PurchaseOrdersTableAsync(tenantId, cancellationToken), await VendorsTableAsync(tenantId, cancellationToken)],
            "notifications" => [await NotificationsFeedAsync(tenantId, cancellationToken)],
            "staff" => [await StaffTableAsync(tenantId, cancellationToken)],
            "departments" => [await DepartmentCardsAsync(tenantId, cancellationToken)],
            "hr-workflows" => await HrSectionsAsync(tenantId, cancellationToken),
            "billing-invoicing" => [await InvoicesTableAsync(tenantId, cancellationToken)],
            "hmo-insurance" => [await ClaimsTableAsync(tenantId, cancellationToken), await PreAuthTableAsync(tenantId, cancellationToken)],
            "reports-analytics" => [await ReportCardsAsync(tenantId, cancellationToken), await WardBarsAsync(tenantId, "Bed Occupancy Trend", cancellationToken)],
            "admin-settings" => [await SettingsSectionAsync(tenantId, cancellationToken), await ModuleTogglesAsync(tenantId, cancellationToken)],
            "audit-compliance" => [await AuditTableAsync(tenantId, cancellationToken), await ComplianceCardsAsync(tenantId, cancellationToken)],
            _ => []
        };
    }

    private async Task<IReadOnlyList<ScreenSection>> DashboardSectionsAsync(Guid tenantId, CancellationToken ct) =>
    [
        await AdmissionsTableAsync(tenantId, "Active Admissions", ct),
        await AppointmentsTableAsync(tenantId, "Today's Appointments", ct),
        await WardBarsAsync(tenantId, "Bed Occupancy by Ward", ct),
        await DiagnosticsTableAsync(tenantId, "Lab", "Urgent Lab Orders", ct)
    ];

    private async Task<IReadOnlyList<ScreenSection>> DoctorSectionsAsync(Guid tenantId, CancellationToken ct) =>
    [
        await AppointmentsTableAsync(tenantId, "Today's Appointments", ct),
        await DiagnosticsTableAsync(tenantId, "Lab", "Results to Review", ct),
        await AdmissionsTableAsync(tenantId, "My Patients", ct)
    ];

    private async Task<IReadOnlyList<ScreenSection>> PatientPortalSectionsAsync(Guid tenantId, CancellationToken ct)
    {
        var patient = await dbContext.Patients
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId && !x.IsDeleted)
            .OrderByDescending(x => x.Status == "Outpatient")
            .ThenBy(x => x.FullName)
            .FirstOrDefaultAsync(ct);

        var profileItems = patient is null
            ? [new CardItem("No patient profile selected", "Patient portal data will appear after the first patient record is created.", "NP")]
            : new[] { new CardItem(patient.FullName, $"{patient.Mrn} - {Age(patient.DateOfBirth)} {patient.Sex}", Initials(patient.FullName)) };

        return
        [
            new ScreenSection("Patient", "profile", Items: profileItems),
            await AppointmentsTableAsync(tenantId, "Upcoming Appointments", ct),
            await InvoicesTableAsync(tenantId, ct),
            await DiagnosticsTableAsync(tenantId, "Lab", "Recent Lab Results", ct)
        ];
    }

    private async Task<IReadOnlyList<ScreenSection>> HrSectionsAsync(Guid tenantId, CancellationToken ct)
    {
        var shifts = await dbContext.ShiftAssignments.AsNoTracking().Where(x => x.TenantId == tenantId).OrderBy(x => x.StartsAt).ToListAsync(ct);
        var leave = await dbContext.LeaveRequests.AsNoTracking().Where(x => x.TenantId == tenantId).ToListAsync(ct);
        var candidates = await dbContext.RecruitmentCandidates.AsNoTracking().Where(x => x.TenantId == tenantId).ToListAsync(ct);

        return
        [
            Table("Shift Assignments", Cols(("staff", "Staff"), ("department", "Department"), ("starts", "Starts"), ("ends", "Ends"), ("status", "Status")),
                shifts.Select(x => Row(("staff", x.StaffName), ("department", x.Department), ("starts", Time(x.StartsAt)), ("ends", Time(x.EndsAt)), ("status", x.Status))).ToArray()),
            Table("Leave Requests", Cols(("request", "Request"), ("staff", "Staff"), ("type", "Type"), ("dates", "Dates"), ("status", "Status")),
                leave.Select(x => Row(("request", x.RequestNumber), ("staff", x.StaffName), ("type", x.LeaveType), ("dates", $"{Date(x.StartsOn)} - {Date(x.EndsOn)}"), ("status", x.Status))).ToArray()),
            Table("Recruitment Pipeline", Cols(("candidate", "Candidate"), ("position", "Position"), ("department", "Department"), ("status", "Stage")),
                candidates.Select(x => Row(("candidate", x.FullName), ("position", x.Position), ("department", x.Department), ("status", x.Stage))).ToArray())
        ];
    }

    private async Task<ScreenSection> PatientsTableAsync(Guid tenantId, string title, CancellationToken ct)
    {
        var patients = await dbContext.Patients.AsNoTracking().Where(x => x.TenantId == tenantId).ToListAsync(ct);
        return Table(title, Cols(("patient", "Patient"), ("mrn", "MRN"), ("ageSex", "Age / Sex"), ("status", "Status")),
            patients.Select(x => Row(("patient", x.FullName), ("mrn", x.Mrn), ("ageSex", $"{Age(x.DateOfBirth)} {x.Sex}"), ("status", x.Status ?? ""))).ToArray());
    }

    private async Task<ScreenSection> AppointmentsTableAsync(Guid tenantId, string title, CancellationToken ct)
    {
        var appointments = await dbContext.Appointments.AsNoTracking().Where(x => x.TenantId == tenantId).OrderBy(x => x.ScheduledAt).ToListAsync(ct);
        return Table(title, Cols(("time", "Time"), ("patient", "Patient"), ("provider", "Provider"), ("department", "Department"), ("type", "Type"), ("room", "Room"), ("status", "Status")),
            appointments.Select(x => Row(("time", Time(x.ScheduledAt)), ("patient", x.PatientName), ("provider", x.ProviderName), ("department", x.Department), ("type", x.VisitType), ("room", x.Room ?? ""), ("status", x.Status))).ToArray());
    }

    private async Task<ScreenSection> AdmissionsTableAsync(Guid tenantId, string title, CancellationToken ct)
    {
        var admissions = await dbContext.Admissions.AsNoTracking().Where(x => x.TenantId == tenantId).ToListAsync(ct);
        return Table(title, Cols(("mrn", "MRN"), ("patient", "Patient"), ("ward", "Ward"), ("attending", "Attending"), ("admitted", "Admitted"), ("status", "Status")),
            admissions.Select(x => Row(("mrn", x.PatientMrn), ("patient", x.PatientName), ("ward", x.WardName), ("attending", x.AttendingProvider), ("admitted", Date(x.AdmittedAt)), ("status", x.Status))).ToArray());
    }

    private async Task<ScreenSection> EmergencyTableAsync(Guid tenantId, CancellationToken ct)
    {
        var cases = await dbContext.EmergencyCases.AsNoTracking().Where(x => x.TenantId == tenantId).ToListAsync(ct);
        return Table("Triage Board", Cols(("patient", "Patient"), ("acuity", "Acuity"), ("complaint", "Complaint"), ("arrival", "Arrival"), ("wait", "Wait"), ("status", "Status")),
            cases.Select(x => Row(("patient", x.PatientName), ("acuity", x.Acuity), ("complaint", x.Complaint), ("arrival", Time(x.ArrivedAt)), ("wait", $"{x.WaitMinutes} min"), ("status", x.Status))).ToArray());
    }

    private async Task<ScreenSection> WardBarsAsync(Guid tenantId, string title, CancellationToken ct)
    {
        var wards = await dbContext.Wards.AsNoTracking().Where(x => x.TenantId == tenantId).ToListAsync(ct);
        return new(title, "bars", Items: wards.Select(x => new CardItem(x.Name, x.Department, $"{x.OccupiedBeds}/{x.TotalBeds}", $"{Percent(x.OccupiedBeds, x.TotalBeds)}%", Percent(x.OccupiedBeds, x.TotalBeds) >= 80 ? "warning" : "success", Progress: Percent(x.OccupiedBeds, x.TotalBeds))).ToArray());
    }

    private async Task<ScreenSection> BedCardsAsync(Guid tenantId, CancellationToken ct)
    {
        var ward = await dbContext.Wards
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId && !x.IsDeleted)
            .OrderByDescending(x => x.OccupiedBeds)
            .ThenBy(x => x.Name)
            .FirstOrDefaultAsync(ct);

        var bedQuery = dbContext.Beds.AsNoTracking().Where(x => x.TenantId == tenantId && !x.IsDeleted);
        if (ward is not null)
        {
            bedQuery = bedQuery.Where(x => x.WardName == ward.Name);
        }

        var beds = await bedQuery.OrderBy(x => x.BedNumber).ToListAsync(ct);
        var title = ward is null ? "Bed Map" : $"{ward.Name} - Bed Map";
        return new(title, "cards", Items: beds.Select(x => new CardItem(x.BedNumber, x.WardName, Status: x.Status, Tone: x.Status)).ToArray());
    }

    private async Task<ScreenSection> PatientFlowBoardAsync(Guid tenantId, CancellationToken ct)
    {
        var encounters = await dbContext.PatientEncounters.AsNoTracking().Where(x => x.TenantId == tenantId).OrderBy(x => x.ArrivedAt).ToListAsync(ct);
        return new ScreenSection("Patient Flow Board", "board", ColumnsBoard:
        [
            Board("Attendant's floor", "Waiting for vitals", "info", encounters.Where(x => x.Stage == "Awaiting vitals")),
            Board("Doctor queue", "Vitals done, awaiting doctor", "warning", encounters.Where(x => x.Stage == "Awaiting doctor")),
            Board("Completed", "Consultation outcome", "success", encounters.Where(x => x.Stage == "Completed"))
        ]);
    }

    private static ScreenSection FlowSteps() => new("Workflow", "steps", Steps:
    [
        new("1", "Register", "Self-register and verify HMO cover", "clipboard"),
        new("2", "Attendant's floor", "Wait, then have vitals taken", "pulse"),
        new("3", "Doctor queue", "See the next available doctor", "stethoscope"),
        new("4", "Outcome", "Pharmacy, scan or department", "check")
    ]);

    private async Task<ScreenSection> SurgeriesTableAsync(Guid tenantId, CancellationToken ct)
    {
        var cases = await dbContext.SurgeryCases.AsNoTracking().Where(x => x.TenantId == tenantId).ToListAsync(ct);
        return Table("Surgical Schedule", Cols(("time", "Time"), ("patient", "Patient"), ("procedure", "Procedure"), ("surgeon", "Surgeon"), ("theatre", "Theatre"), ("status", "Status")),
            cases.Select(x => Row(("time", Time(x.ScheduledAt)), ("patient", x.PatientName), ("procedure", x.Procedure), ("surgeon", x.SurgeonName), ("theatre", x.Theatre), ("status", x.Status))).ToArray());
    }

    private async Task<ScreenSection> NursingTableAsync(Guid tenantId, CancellationToken ct)
    {
        var tasks = await dbContext.NursingTasks.AsNoTracking().Where(x => x.TenantId == tenantId).ToListAsync(ct);
        return Table("Care Tasks", Cols(("patient", "Patient"), ("ward", "Ward"), ("task", "Task"), ("due", "Due"), ("nurse", "Nurse"), ("status", "Status")),
            tasks.Select(x => Row(("patient", x.PatientName), ("ward", x.WardName), ("task", x.TaskType), ("due", Time(x.DueAt)), ("nurse", x.NurseName), ("status", x.Status))).ToArray());
    }

    private async Task<ScreenSection> StaffingTableAsync(Guid tenantId, CancellationToken ct)
    {
        var wards = await dbContext.Wards.AsNoTracking().Where(x => x.TenantId == tenantId).ToListAsync(ct);
        var nurses = await dbContext.StaffMembers
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId && !x.IsDeleted && x.JobTitle.Contains("Nurse"))
            .OrderBy(x => x.FullName)
            .ToListAsync(ct);

        var activeStaff = await dbContext.StaffMembers
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId && !x.IsDeleted && x.Status == "Active")
            .OrderBy(x => x.FullName)
            .ToListAsync(ct);

        return Table("Ward Staffing", Cols(("ward", "Ward"), ("patients", "Patients"), ("lead", "Lead Nurse")),
            wards.Select(x => Row(("ward", x.Name), ("patients", x.OccupiedBeds.ToString()), ("lead", LeadStaffForWard(x, nurses, activeStaff)))).ToArray());
    }

    private async Task<ScreenSection> DiagnosticsTableAsync(Guid tenantId, string category, string title, CancellationToken ct)
    {
        var orders = await dbContext.DiagnosticOrders.AsNoTracking().Where(x => x.TenantId == tenantId && x.Category == category).ToListAsync(ct);
        return Table(title, Cols(("order", "Order"), ("patient", "Patient"), ("test", "Test"), ("priority", "Priority"), ("status", "Status"), ("ordered", "Ordered"), ("tech", "Assigned")),
            orders.Select(x => Row(("order", x.OrderNumber), ("patient", x.PatientName), ("test", x.BodyPart == null ? x.TestName : $"{x.TestName} - {x.BodyPart}"), ("priority", x.Priority), ("status", x.Status), ("ordered", Time(x.OrderedAt)), ("tech", x.AssignedTo ?? "-"))).ToArray());
    }

    private async Task<ScreenSection> InventoryTableAsync(Guid tenantId, CancellationToken ct)
    {
        var items = await dbContext.InventoryItems.AsNoTracking().Where(x => x.TenantId == tenantId).ToListAsync(ct);
        return Table("Inventory List", Cols(("item", "Item"), ("category", "Category"), ("stock", "Stock Level"), ("unit", "Unit"), ("status", "Status"), ("expiry", "Expiry")),
            items.Select(x => Row(("item", x.Name), ("category", x.Category), ("stock", $"{x.QuantityOnHand} / {x.ReorderLevel}"), ("unit", x.Unit), ("status", x.Status), ("expiry", x.ExpiryDate == null ? "" : Date(x.ExpiryDate.Value)))).ToArray());
    }

    private async Task<ScreenSection> TelemedicineTableAsync(Guid tenantId, CancellationToken ct)
    {
        var sessions = await dbContext.TelemedicineSessions.AsNoTracking().Where(x => x.TenantId == tenantId).ToListAsync(ct);
        return Table("Virtual Sessions", Cols(("time", "Time"), ("patient", "Patient"), ("provider", "Provider"), ("channel", "Channel"), ("duration", "Duration"), ("status", "Status")),
            sessions.Select(x => Row(("time", Time(x.ScheduledAt)), ("patient", x.PatientName), ("provider", x.ProviderName), ("channel", x.Channel), ("duration", $"{x.DurationMinutes} min"), ("status", x.Status))).ToArray());
    }

    private async Task<ScreenSection> PurchaseOrdersTableAsync(Guid tenantId, CancellationToken ct)
    {
        var orders = await dbContext.PurchaseOrders.AsNoTracking().Where(x => x.TenantId == tenantId).ToListAsync(ct);
        return Table("Purchase Orders", Cols(("po", "PO"), ("item", "Item"), ("vendor", "Vendor"), ("quantity", "Qty"), ("amount", "Total"), ("status", "Status")),
            orders.Select(x => Row(("po", x.PurchaseOrderNumber), ("item", x.ItemName), ("vendor", x.VendorName), ("quantity", x.Quantity.ToString()), ("amount", Money(x.TotalAmount)), ("status", x.Status))).ToArray());
    }

    private async Task<ScreenSection> VendorsTableAsync(Guid tenantId, CancellationToken ct)
    {
        var vendors = await dbContext.Vendors.AsNoTracking().Where(x => x.TenantId == tenantId).ToListAsync(ct);
        return Table("Vendors", Cols(("vendor", "Vendor"), ("category", "Category"), ("open", "Open"), ("ontime", "On-Time")),
            vendors.Select(x => Row(("vendor", x.Name), ("category", x.Category ?? ""), ("open", x.OpenOrders.ToString()), ("ontime", $"{x.OnTimeRate:0}%"))).ToArray());
    }

    private async Task<ScreenSection> NotificationsFeedAsync(Guid tenantId, CancellationToken ct)
    {
        var notifications = await dbContext.Notifications.AsNoTracking().Where(x => x.TenantId == tenantId).OrderBy(x => x.PublishedAt).ToListAsync(ct);
        return new("Activity Feed", "feed", Tabs: ["All", "Critical lab", "Inventory", "Clinical", "Scheduling", "Broadcast"],
            Items: notifications.Select(x => new CardItem(x.Title, x.Body, Time(x.PublishedAt), Status: x.Category, Tone: x.Severity)).ToArray());
    }

    private async Task<ScreenSection> StaffTableAsync(Guid tenantId, CancellationToken ct)
    {
        var staff = await dbContext.StaffMembers.AsNoTracking().Where(x => x.TenantId == tenantId).ToListAsync(ct);
        return Table("Staff Directory", Cols(("name", "Name & Role"), ("department", "Department"), ("type", "Type"), ("status", "Status"), ("details", "Engagement Details")),
            staff.Select(x => Row(("name", $"{x.FullName}|{x.JobTitle}"), ("department", x.Department), ("type", x.EmploymentType), ("status", x.Status), ("details", x.EmployeeNumber))).ToArray(), ["All", "Employee", "Consultant"]);
    }

    private async Task<ScreenSection> DepartmentCardsAsync(Guid tenantId, CancellationToken ct)
    {
        var departments = await dbContext.Departments.AsNoTracking().Where(x => x.TenantId == tenantId).ToListAsync(ct);
        return new("Department Capacity", "cards", Items: departments.Select(x => new CardItem(x.Name, $"ID: {x.Code} - Head: {x.HeadName}", $"{x.StaffCount} staff", $"{x.BedCount} beds", $"{x.OccupancyRate:0}%", x.OccupancyRate >= 85 ? "danger" : x.OccupancyRate >= 80 ? "warning" : "success", (int)x.OccupancyRate)).ToArray());
    }

    private async Task<ScreenSection> InvoicesTableAsync(Guid tenantId, CancellationToken ct)
    {
        var invoices = await dbContext.Invoices.AsNoTracking().Where(x => x.TenantId == tenantId).ToListAsync(ct);
        return Table("Invoices", Cols(("invoice", "Invoice #"), ("patient", "Patient"), ("payer", "Payer"), ("amount", "Amount"), ("status", "Status"), ("issued", "Issued"), ("due", "Due")),
            invoices.Select(x => Row(("invoice", x.InvoiceNumber), ("patient", x.PatientName), ("payer", x.PayerType), ("amount", Money(x.Amount)), ("status", x.Status), ("issued", Date(x.IssuedOn)), ("due", Date(x.DueOn)))).ToArray(), ["All", "Paid", "Pending", "Overdue", "Draft"]);
    }

    private async Task<ScreenSection> ClaimsTableAsync(Guid tenantId, CancellationToken ct)
    {
        var claims = await dbContext.InsuranceClaims.AsNoTracking().Where(x => x.TenantId == tenantId).ToListAsync(ct);
        return Table("Claims", Cols(("claim", "Claim"), ("patient", "Patient"), ("insurer", "Insurer"), ("amount", "Amount"), ("submitted", "Submitted"), ("status", "Status")),
            claims.Select(x => Row(("claim", x.ClaimNumber), ("patient", x.PatientName), ("insurer", x.InsurerName), ("amount", Money(x.Amount)), ("submitted", Date(x.SubmittedOn)), ("status", x.Status))).ToArray());
    }

    private async Task<ScreenSection> PreAuthTableAsync(Guid tenantId, CancellationToken ct)
    {
        var requests = await dbContext.PreAuthorizations.AsNoTracking().Where(x => x.TenantId == tenantId).ToListAsync(ct);
        return Table("Pre-Authorizations", Cols(("patient", "Patient"), ("service", "Service"), ("insurer", "Insurer"), ("status", "Status")),
            requests.Select(x => Row(("patient", x.PatientName), ("service", x.ServiceName), ("insurer", x.InsurerName), ("status", x.Status))).ToArray());
    }

    private async Task<ScreenSection> ReportCardsAsync(Guid tenantId, CancellationToken ct)
    {
        var metrics = await dbContext.ReportMetrics.AsNoTracking().Where(x => x.TenantId == tenantId).ToListAsync(ct);
        return new("Executive Metrics", "cards", Items: metrics.Select(x => new CardItem(x.Name, x.Area, x.Value, x.Trend, Tone: x.Trend != null && x.Trend.StartsWith("-") ? "warning" : "success")).ToArray());
    }

    private async Task<ScreenSection> SettingsSectionAsync(Guid tenantId, CancellationToken ct)
    {
        var tenant = await dbContext.Tenants.AsNoTracking().FirstAsync(x => x.Id == tenantId, ct);
        return new ScreenSection("Hospital Configuration", "settings", Items:
        [
            new("Hospital Name", tenant.Name),
            new("Tenant Code", tenant.Code),
            new("City / Region", tenant.Location),
            new("Licensed Beds", tenant.LicensedBeds.ToString())
        ], Tabs: ["Hospital Config", "Users & Roles", "Permissions", "Branding", "Integrations", "Security"]);
    }

    private async Task<ScreenSection> ModuleTogglesAsync(Guid tenantId, CancellationToken ct)
    {
        var modules = await dbContext.OperationalModules.AsNoTracking().Where(x => x.TenantId == tenantId).OrderBy(x => x.SortOrder).ToListAsync(ct);
        return new("Active Modules", "toggles", Items: modules.Select(x => new CardItem(x.Label, x.Group, Status: x.IsActive ? "On" : "Off")).ToArray());
    }

    private async Task<ScreenSection> AuditTableAsync(Guid tenantId, CancellationToken ct)
    {
        var logs = await dbContext.AuditLogs.AsNoTracking().Where(x => x.TenantId == tenantId).OrderBy(x => x.CreatedAt).ToListAsync(ct);
        return Table("Access Log", Cols(("time", "Time"), ("user", "User"), ("action", "Action"), ("resource", "Resource"), ("result", "Result")),
            logs.Select(x => Row(("time", Time(x.CreatedAt)), ("user", x.Actor), ("action", x.Action), ("resource", x.TargetType), ("result", x.Result))).ToArray());
    }

    private async Task<ScreenSection> ComplianceCardsAsync(Guid tenantId, CancellationToken ct)
    {
        var controls = await dbContext.ComplianceControls.AsNoTracking().Where(x => x.TenantId == tenantId).ToListAsync(ct);
        return new("Compliance Posture", "cards", Items: controls.Select(x => new CardItem(x.Name, x.Description, Status: x.Status, Tone: x.Status)).ToArray());
    }

    private static BoardColumn Board(string title, string subtitle, string tone, IEnumerable<PatientEncounter> encounters) =>
        new(title, subtitle, tone, encounters.Select(x => new CardItem(
            x.PatientName,
            x.Complaint,
            x.EncounterNumber,
            x.Stage == "Completed" ? null : $"{x.WaitMinutes}m waiting",
            x.Stage == "Completed" ? "Pharmacy" : x.PayerType == "HMO" ? $"HMO - {x.Insurer}" : x.PayerType,
            Detail: x.Stage == "Completed" ? "Prescription ready for pharmacy review" : null,
            Action: FlowActionForStage(x.Stage))).ToArray());

    private static ActionButton? FlowActionForStage(string stage) => stage switch
    {
        "Awaiting vitals" => new("Take vitals", "primary", "pulse"),
        "Awaiting doctor" => new("See doctor", "primary", "stethoscope"),
        "Completed" => new("Go to Pharmacy", "link", "pill", "/pharmacy-inventory"),
        _ => null
    };

    private static IReadOnlyList<ActionButton> BuildActions(string key) => key switch
    {
        "patients" or "inpatient" => [new("Admit Patient", "primary", "plus")],
        "appointments" => [new("New Appointment", "primary", "plus")],
        "patient-flow" => [new("Register Walk-In", "primary", "plus")],
        "emergency" => [new("Register Arrival", "primary", "plus")],
        "wards-beds" => [new("Allocate Bed", "primary", "bed")],
        "lab-diagnostics" => [new("New Order", "primary", "flask")],
        "pharmacy-inventory" => [new("Add Item", "primary", "plus")],
        "procurement-vendors" => [new("New Order", "primary", "plus")],
        "staff" => [new("Add Staff", "primary", "plus")],
        "departments" => [new("New Department", "primary", "plus")],
        "billing-invoicing" => [new("Create Invoice", "primary", "plus")],
        _ => []
    };

    private static ScreenSection Table(string title, IReadOnlyList<TableColumn> columns, IReadOnlyList<IReadOnlyDictionary<string, string>> rows, IReadOnlyList<string>? tabs = null) =>
        new(title, "table", columns, rows, Tabs: tabs);

    private static IReadOnlyList<TableColumn> Cols(params (string Key, string Label)[] columns) =>
        columns.Select(x => new TableColumn(x.Key, x.Label)).ToArray();

    private static IReadOnlyDictionary<string, string> Row(params (string Key, string Value)[] values) =>
        values.ToDictionary(x => x.Key, x => x.Value);

    private static MetricCard Metric(string label, int value, string? hint, string tone, string icon) => new(label, value.ToString("N0"), hint, tone, icon);
    private static MetricCard Metric(string label, string value, string? hint, string tone, string icon) => new(label, value, hint, tone, icon);
    private static string Money(decimal amount) => amount >= 1_000_000 ? $"${amount / 1_000_000:0.##}M" : $"${amount:N2}";
    private static string Time(DateTimeOffset value) => value.ToString("HH:mm");
    private static string Date(DateTimeOffset value) => value.ToString("yyyy-MM-dd");
    private static string Date(DateOnly value) => value.ToString("yyyy-MM-dd");
    private DateOnly Today() => DateOnly.FromDateTime(clock.UtcNow.UtcDateTime);
    private int Age(DateOnly? dateOfBirth)
    {
        if (dateOfBirth is null)
        {
            return 0;
        }

        var today = Today();
        var age = today.Year - dateOfBirth.Value.Year;
        if (dateOfBirth.Value > today.AddYears(-age))
        {
            age--;
        }

        return Math.Max(age, 0);
    }
    private static int Percent(int value, int total) => total == 0 ? 0 : (int)Math.Round((decimal)value / total * 100);
    private static string Initials(string value) => string.Concat(value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(2).Select(x => x[0])).ToUpperInvariant();

    private static string LeadStaffForWard(Ward ward, IReadOnlyList<Medora.Domain.Entities.StaffMember> nurses, IReadOnlyList<Medora.Domain.Entities.StaffMember> activeStaff)
    {
        return nurses.FirstOrDefault(x => x.Department == ward.Department)?.FullName
            ?? nurses.FirstOrDefault()?.FullName
            ?? activeStaff.FirstOrDefault(x => x.Department == ward.Department)?.FullName
            ?? activeStaff.FirstOrDefault()?.FullName
            ?? "-";
    }

    private static async Task<int> WardOccupancyAsync(IQueryable<Ward> wards, CancellationToken cancellationToken)
    {
        var totals = await wards
            .GroupBy(_ => 1)
            .Select(x => new { Total = x.Sum(w => w.TotalBeds), Occupied = x.Sum(w => w.OccupiedBeds) })
            .FirstOrDefaultAsync(cancellationToken);

        return totals is null ? 0 : Percent(totals.Occupied, totals.Total);
    }
}

