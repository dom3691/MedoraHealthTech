using Medora.Application.Security;
using Medora.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace Medora.Infrastructure.Persistence.Seed;

public sealed class DatabaseSeeder
{
    private readonly MedoraDbContext _dbContext;

    public DatabaseSeeder(MedoraDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await SeedRolesAndPermissionsAsync(cancellationToken);
        await SeedTenantsAsync(cancellationToken);
        await SeedUsersAsync(cancellationToken);
        await SeedOperationsAsync(cancellationToken);
    }

    public async Task SeedRolesAndPermissionsAsync(CancellationToken cancellationToken)
    {
        var existingPermissions = await _dbContext.Permissions.ToDictionaryAsync(x => x.Code, cancellationToken);

        foreach (var permissionSeed in DefaultPermissionCatalog.Permissions)
        {
            if (!existingPermissions.ContainsKey(permissionSeed.Code))
            {
                var permission = new Permission
                {
                    Module = permissionSeed.Module,
                    Action = permissionSeed.Action,
                    Code = permissionSeed.Code,
                    Description = permissionSeed.Description
                };

                _dbContext.Permissions.Add(permission);
                existingPermissions[permission.Code] = permission;
            }
        }

        var existingRoles = await _dbContext.Roles
            .Include(x => x.RolePermissions)
            .ToDictionaryAsync(x => x.Code, cancellationToken);

        foreach (var roleSeed in DefaultPermissionCatalog.Roles)
        {
            if (!existingRoles.TryGetValue(roleSeed.Code, out var role))
            {
                role = new Role
                {
                    Code = roleSeed.Code,
                    Name = roleSeed.Name,
                    Description = roleSeed.Description,
                    IsSystemRole = true
                };
                _dbContext.Roles.Add(role);
                existingRoles[role.Code] = role;
            }

            var desiredPermissions = ResolvePermissions(roleSeed.PermissionPatterns, existingPermissions.Values);
            foreach (var permission in desiredPermissions)
            {
                if (!role.RolePermissions.Any(x => x.PermissionId == permission.Id))
                {
                    role.RolePermissions.Add(new RolePermission { Role = role, Permission = permission });
                }
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static IReadOnlyCollection<Permission> ResolvePermissions(IEnumerable<string> patterns, IEnumerable<Permission> permissions)
    {
        var resolved = new Dictionary<string, Permission>(StringComparer.OrdinalIgnoreCase);
        foreach (var pattern in patterns)
        {
            var matches = MatchPattern(pattern, permissions);
            foreach (var permission in matches)
            {
                resolved[permission.Code] = permission;
            }
        }

        return resolved.Values.ToArray();
    }

    private static IEnumerable<Permission> MatchPattern(string pattern, IEnumerable<Permission> permissions)
    {
        if (pattern == "*.*")
        {
            return permissions;
        }

        var parts = pattern.Split('.', 2);
        if (parts.Length != 2)
        {
            return [];
        }

        var module = Normalize(parts[0]);
        var action = Normalize(parts[1]);

        return permissions.Where(permission =>
            (module == "*" || Normalize(permission.Module) == module) &&
            (action == "*" || Normalize(permission.Action) == action));
    }

    private static string Normalize(string value) => value
        .Replace("&", "and", StringComparison.OrdinalIgnoreCase)
        .Replace("/", "-", StringComparison.OrdinalIgnoreCase)
        .Replace(" ", "-", StringComparison.OrdinalIgnoreCase)
        .ToLowerInvariant();

    private async Task SeedTenantsAsync(CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        await AddTenantIfMissingAsync("MEDORA", "Medora Platform", "Platform Operations", 0, "Internal", now, cancellationToken);
        await AddTenantIfMissingAsync("SMG-01", "St. Mary General Hospital", "Boston, MA", 540, "Enterprise", now, cancellationToken);
        await AddTenantIfMissingAsync("RVS-02", "Riverside Medical Center", "Austin, TX", 312, "Professional", now, cancellationToken);
        await AddTenantIfMissingAsync("NGC-03", "Northgate Children's Hospital", "Seattle, WA", 188, "Professional", now, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task AddTenantIfMissingAsync(
        string code,
        string name,
        string location,
        int licensedBeds,
        string plan,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        if (await _dbContext.Tenants.AnyAsync(x => x.Code == code, cancellationToken))
        {
            return;
        }

        _dbContext.Tenants.Add(new Tenant
        {
            Name = name,
            Code = code,
            Location = location,
            LicensedBeds = licensedBeds,
            Plan = plan,
            CreatedAt = now
        });
    }

    private async Task SeedUsersAsync(CancellationToken cancellationToken)
    {
        var adminRole = await _dbContext.Roles.SingleAsync(x => x.Code == "SA", cancellationToken);
        var medoraSuperAdminRole = await _dbContext.Roles.SingleAsync(x => x.Code == "MSA", cancellationToken);
        var tenants = await _dbContext.Tenants.ToListAsync(cancellationToken);
        var now = DateTimeOffset.UtcNow;

        foreach (var tenant in tenants)
        {
            var email = AdminEmailForTenant(tenant.Code);
            var isMedoraTenant = tenant.Code == "MEDORA";

            if (await _dbContext.Users.AnyAsync(x => x.TenantId == tenant.Id && x.Email == email, cancellationToken))
            {
                continue;
            }

            var user = new ApplicationUser
            {
                TenantId = tenant.Id,
                CreatedAt = now,
                FullName = isMedoraTenant ? "Medora Super Admin" : $"{tenant.Name} Administrator",
                Email = email,
                PasswordHash = PasswordHasher.Hash(tenant.Code, email, "Medora@2026"),
                IsActive = true
            };

            user.UserRoles.Add(new UserRole { User = user, Role = isMedoraTenant ? medoraSuperAdminRole : adminRole });
            _dbContext.Users.Add(user);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedOperationsAsync(CancellationToken cancellationToken)
    {
        var tenants = await _dbContext.Tenants.ToListAsync(cancellationToken);
        var now = DateTimeOffset.UtcNow;

        foreach (var tenant in tenants)
        {
            if (tenant.Code == "MEDORA")
            {
                if (!await _dbContext.OperationalModules.AnyAsync(x => x.TenantId == tenant.Id, cancellationToken))
                {
                    _dbContext.OperationalModules.AddRange(OperationsSeedData.Modules(tenant.Id, now));
                }

                continue;
            }

            if (await _dbContext.OperationalModules.AnyAsync(x => x.TenantId == tenant.Id, cancellationToken))
            {
                continue;
            }

            _dbContext.OperationalModules.AddRange(OperationsSeedData.Modules(tenant.Id, now));
            _dbContext.Departments.AddRange(OperationsSeedData.Departments(tenant.Id, now));
            _dbContext.Patients.AddRange(OperationsSeedData.Patients(tenant.Id, now));
            _dbContext.StaffMembers.AddRange(OperationsSeedData.Staff(tenant.Id, now));
            _dbContext.Wards.AddRange(OperationsSeedData.Wards(tenant.Id, now));
            _dbContext.Beds.AddRange(OperationsSeedData.Beds(tenant.Id, now));
            _dbContext.Appointments.AddRange(OperationsSeedData.Appointments(tenant.Id, now));
            _dbContext.PatientEncounters.AddRange(OperationsSeedData.PatientEncounters(tenant.Id, now));
            _dbContext.Admissions.AddRange(OperationsSeedData.Admissions(tenant.Id, now));
            _dbContext.EmergencyCases.AddRange(OperationsSeedData.EmergencyCases(tenant.Id, now));
            _dbContext.SurgeryCases.AddRange(OperationsSeedData.SurgeryCases(tenant.Id, now));
            _dbContext.NursingTasks.AddRange(OperationsSeedData.NursingTasks(tenant.Id, now));
            _dbContext.DiagnosticOrders.AddRange(OperationsSeedData.DiagnosticOrders(tenant.Id, now));
            _dbContext.InventoryItems.AddRange(OperationsSeedData.Inventory(tenant.Id, now));
            _dbContext.Vendors.AddRange(OperationsSeedData.Vendors(tenant.Id, now));
            _dbContext.PurchaseOrders.AddRange(OperationsSeedData.PurchaseOrders(tenant.Id, now));
            _dbContext.Notifications.AddRange(OperationsSeedData.Notifications(tenant.Id, now));
            _dbContext.Invoices.AddRange(OperationsSeedData.Invoices(tenant.Id, now));
            _dbContext.InsuranceClaims.AddRange(OperationsSeedData.InsuranceClaims(tenant.Id, now));
            _dbContext.PreAuthorizations.AddRange(OperationsSeedData.PreAuthorizations(tenant.Id, now));
            _dbContext.TelemedicineSessions.AddRange(OperationsSeedData.TelemedicineSessions(tenant.Id, now));
            _dbContext.ComplianceControls.AddRange(OperationsSeedData.ComplianceControls(tenant.Id, now));
            _dbContext.AuditLogs.AddRange(OperationsSeedData.AuditLogs(tenant.Id, now));
            _dbContext.ReportMetrics.AddRange(OperationsSeedData.ReportMetrics(tenant.Id, now));
            _dbContext.ShiftAssignments.AddRange(OperationsSeedData.Shifts(tenant.Id, now));
            _dbContext.AttendanceRecords.AddRange(OperationsSeedData.Attendance(tenant.Id, now));
            _dbContext.LeaveRequests.AddRange(OperationsSeedData.LeaveRequests(tenant.Id, now));
            _dbContext.RecruitmentCandidates.AddRange(OperationsSeedData.Candidates(tenant.Id, now));
            _dbContext.TrainingRecords.AddRange(OperationsSeedData.Training(tenant.Id, now));
            _dbContext.CredentialRecords.AddRange(OperationsSeedData.Credentials(tenant.Id, now));
            _dbContext.PerformanceReviews.AddRange(OperationsSeedData.Reviews(tenant.Id, now));
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public static string AdminEmailForTenant(string tenantCode) => tenantCode switch
    {
        "MEDORA" => "superadmin@medora.local",
        "SMG-01" => "admin@stmary.medora.local",
        "RVS-02" => "admin@riverside.medora.local",
        "NGC-03" => "admin@northgate.medora.local",
        _ => $"admin@{tenantCode.ToLowerInvariant().Replace("-", "")}.medora.local"
    };
}
