using Medora.Domain.Entities;
using Medora.Domain.Common;
using Medora.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Medora.Infrastructure.Persistence;

public sealed class MedoraDbContext : DbContext
{
    private readonly Guid? _currentTenantId;

    private bool HasCurrentTenant => _currentTenantId.HasValue;

    private Guid CurrentTenantId => _currentTenantId ?? Guid.Empty;

    public MedoraDbContext(DbContextOptions<MedoraDbContext> options) : base(options)
    {
    }

    public MedoraDbContext(DbContextOptions<MedoraDbContext> options, ICurrentTenantService currentTenantService) : base(options)
    {
        _currentTenantId = currentTenantService.TenantId;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<StaffMember> StaffMembers => Set<StaffMember>();
    public DbSet<OperationalModule> OperationalModules => Set<OperationalModule>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Ward> Wards => Set<Ward>();
    public DbSet<Bed> Beds => Set<Bed>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<PatientEncounter> PatientEncounters => Set<PatientEncounter>();
    public DbSet<Admission> Admissions => Set<Admission>();
    public DbSet<EmergencyCase> EmergencyCases => Set<EmergencyCase>();
    public DbSet<SurgeryCase> SurgeryCases => Set<SurgeryCase>();
    public DbSet<NursingTask> NursingTasks => Set<NursingTask>();
    public DbSet<DiagnosticOrder> DiagnosticOrders => Set<DiagnosticOrder>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<NotificationMessage> Notifications => Set<NotificationMessage>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InsuranceClaim> InsuranceClaims => Set<InsuranceClaim>();
    public DbSet<PreAuthorization> PreAuthorizations => Set<PreAuthorization>();
    public DbSet<TelemedicineSession> TelemedicineSessions => Set<TelemedicineSession>();
    public DbSet<ComplianceControl> ComplianceControls => Set<ComplianceControl>();
    public DbSet<ReportMetric> ReportMetrics => Set<ReportMetric>();
    public DbSet<ShiftAssignment> ShiftAssignments => Set<ShiftAssignment>();
    public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();
    public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();
    public DbSet<RecruitmentCandidate> RecruitmentCandidates => Set<RecruitmentCandidate>();
    public DbSet<TrainingRecord> TrainingRecords => Set<TrainingRecord>();
    public DbSet<CredentialRecord> CredentialRecords => Set<CredentialRecord>();
    public DbSet<PerformanceReview> PerformanceReviews => Set<PerformanceReview>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.ToTable("Tenants");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Code).HasMaxLength(40).IsRequired();
            entity.HasIndex(x => x.Code).IsUnique();
        });

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.FullName).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(256).IsRequired();
            entity.HasIndex(x => new { x.TenantId, x.Email }).IsUnique();
            entity.HasOne<Tenant>().WithMany().HasForeignKey(x => x.TenantId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Code).HasMaxLength(40).IsRequired();
            entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(500).IsRequired();
            entity.HasIndex(x => x.Code).IsUnique();
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("Permissions");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Module).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Action).HasMaxLength(40).IsRequired();
            entity.Property(x => x.Code).HasMaxLength(160).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(300).IsRequired();
            entity.HasIndex(x => x.Code).IsUnique();
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRoles");
            entity.HasKey(x => new { x.UserId, x.RoleId });
            entity.HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId);
            entity.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId);
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.ToTable("RolePermissions");
            entity.HasKey(x => new { x.RoleId, x.PermissionId });
            entity.HasOne(x => x.Role).WithMany(x => x.RolePermissions).HasForeignKey(x => x.RoleId);
            entity.HasOne(x => x.Permission).WithMany().HasForeignKey(x => x.PermissionId);
        });

        ConfigureTenantScopedEntity<AuditLog>(modelBuilder, "AuditLogs");
        ConfigureTenantScopedEntity<Patient>(modelBuilder, "Patients");
        ConfigureTenantScopedEntity<StaffMember>(modelBuilder, "StaffMembers");
        ConfigureTenantScopedEntity<OperationalModule>(modelBuilder, "OperationalModules");
        ConfigureTenantScopedEntity<Department>(modelBuilder, "Departments");
        ConfigureTenantScopedEntity<Ward>(modelBuilder, "Wards");
        ConfigureTenantScopedEntity<Bed>(modelBuilder, "Beds");
        ConfigureTenantScopedEntity<Appointment>(modelBuilder, "Appointments");
        ConfigureTenantScopedEntity<PatientEncounter>(modelBuilder, "PatientEncounters");
        ConfigureTenantScopedEntity<Admission>(modelBuilder, "Admissions");
        ConfigureTenantScopedEntity<EmergencyCase>(modelBuilder, "EmergencyCases");
        ConfigureTenantScopedEntity<SurgeryCase>(modelBuilder, "SurgeryCases");
        ConfigureTenantScopedEntity<NursingTask>(modelBuilder, "NursingTasks");
        ConfigureTenantScopedEntity<DiagnosticOrder>(modelBuilder, "DiagnosticOrders");
        ConfigureTenantScopedEntity<InventoryItem>(modelBuilder, "InventoryItems");
        ConfigureTenantScopedEntity<Vendor>(modelBuilder, "Vendors");
        ConfigureTenantScopedEntity<PurchaseOrder>(modelBuilder, "PurchaseOrders");
        ConfigureTenantScopedEntity<NotificationMessage>(modelBuilder, "Notifications");
        ConfigureTenantScopedEntity<Invoice>(modelBuilder, "Invoices");
        ConfigureTenantScopedEntity<InsuranceClaim>(modelBuilder, "InsuranceClaims");
        ConfigureTenantScopedEntity<PreAuthorization>(modelBuilder, "PreAuthorizations");
        ConfigureTenantScopedEntity<TelemedicineSession>(modelBuilder, "TelemedicineSessions");
        ConfigureTenantScopedEntity<ComplianceControl>(modelBuilder, "ComplianceControls");
        ConfigureTenantScopedEntity<ReportMetric>(modelBuilder, "ReportMetrics");
        ConfigureTenantScopedEntity<ShiftAssignment>(modelBuilder, "ShiftAssignments");
        ConfigureTenantScopedEntity<AttendanceRecord>(modelBuilder, "AttendanceRecords");
        ConfigureTenantScopedEntity<LeaveRequest>(modelBuilder, "LeaveRequests");
        ConfigureTenantScopedEntity<RecruitmentCandidate>(modelBuilder, "RecruitmentCandidates");
        ConfigureTenantScopedEntity<TrainingRecord>(modelBuilder, "TrainingRecords");
        ConfigureTenantScopedEntity<CredentialRecord>(modelBuilder, "CredentialRecords");
        ConfigureTenantScopedEntity<PerformanceReview>(modelBuilder, "PerformanceReviews");

        ConfigureStringAndDecimalDefaults(modelBuilder);
        ConfigureUniqueIndexes(modelBuilder);
    }

    private void ConfigureTenantScopedEntity<TEntity>(ModelBuilder modelBuilder, string tableName)
        where TEntity : TenantScopedEntity
    {
        modelBuilder.Entity<TEntity>(entity =>
        {
            entity.ToTable(tableName);
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.TenantId);
            entity.HasOne<Tenant>().WithMany().HasForeignKey(x => x.TenantId).OnDelete(DeleteBehavior.Restrict);
            entity.HasQueryFilter(x => !x.IsDeleted && (!HasCurrentTenant || x.TenantId == CurrentTenantId));
        });
    }

    private static void ConfigureStringAndDecimalDefaults(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(string) && property.GetMaxLength() is null)
                {
                    property.SetMaxLength(property.Name.Contains("Description", StringComparison.OrdinalIgnoreCase) || property.Name.Contains("Body", StringComparison.OrdinalIgnoreCase) ? 1000 : 200);
                }

                if (property.ClrType == typeof(decimal) || property.ClrType == typeof(decimal?))
                {
                    property.SetPrecision(18);
                    property.SetScale(2);
                }
            }
        }
    }

    private static void ConfigureUniqueIndexes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>().HasIndex(x => new { x.TenantId, x.Mrn }).IsUnique();
        modelBuilder.Entity<StaffMember>().HasIndex(x => new { x.TenantId, x.EmployeeNumber }).IsUnique();
        modelBuilder.Entity<OperationalModule>().HasIndex(x => new { x.TenantId, x.Key }).IsUnique();
        modelBuilder.Entity<Department>().HasIndex(x => new { x.TenantId, x.Code }).IsUnique();
        modelBuilder.Entity<Ward>().HasIndex(x => new { x.TenantId, x.Code }).IsUnique();
        modelBuilder.Entity<Bed>().HasIndex(x => new { x.TenantId, x.BedNumber }).IsUnique();
        modelBuilder.Entity<Appointment>().HasIndex(x => new { x.TenantId, x.AppointmentNumber }).IsUnique();
        modelBuilder.Entity<PatientEncounter>().HasIndex(x => new { x.TenantId, x.EncounterNumber }).IsUnique();
        modelBuilder.Entity<Admission>().HasIndex(x => new { x.TenantId, x.AdmissionNumber }).IsUnique();
        modelBuilder.Entity<EmergencyCase>().HasIndex(x => new { x.TenantId, x.CaseNumber }).IsUnique();
        modelBuilder.Entity<SurgeryCase>().HasIndex(x => new { x.TenantId, x.CaseNumber }).IsUnique();
        modelBuilder.Entity<NursingTask>().HasIndex(x => new { x.TenantId, x.TaskNumber }).IsUnique();
        modelBuilder.Entity<DiagnosticOrder>().HasIndex(x => new { x.TenantId, x.OrderNumber }).IsUnique();
        modelBuilder.Entity<InventoryItem>().HasIndex(x => new { x.TenantId, x.Sku }).IsUnique();
        modelBuilder.Entity<Vendor>().HasIndex(x => new { x.TenantId, x.VendorCode }).IsUnique();
        modelBuilder.Entity<PurchaseOrder>().HasIndex(x => new { x.TenantId, x.PurchaseOrderNumber }).IsUnique();
        modelBuilder.Entity<Invoice>().HasIndex(x => new { x.TenantId, x.InvoiceNumber }).IsUnique();
        modelBuilder.Entity<InsuranceClaim>().HasIndex(x => new { x.TenantId, x.ClaimNumber }).IsUnique();
        modelBuilder.Entity<TelemedicineSession>().HasIndex(x => new { x.TenantId, x.SessionNumber }).IsUnique();
        modelBuilder.Entity<ShiftAssignment>().HasIndex(x => new { x.TenantId, x.ShiftNumber }).IsUnique();
        modelBuilder.Entity<LeaveRequest>().HasIndex(x => new { x.TenantId, x.RequestNumber }).IsUnique();
        modelBuilder.Entity<RecruitmentCandidate>().HasIndex(x => new { x.TenantId, x.CandidateNumber }).IsUnique();
    }
}

