using Academy.Infrastructure.Caching;
using Academy.Infrastructure.Multitenancy;
using Academy.Infrastructure.Persistence.Context;
using Academy.Shared.Authorization;
using Academy.Shared.Multitenancy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Academy.Infrastructure.Persistence.Initialization
{
    internal class ApplicationDbSeeder
    {
        private readonly TenantInfo _currentTenant;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CustomSeederRunner _seederRunner;
        private readonly ILogger<ApplicationDbSeeder> _logger;
        private readonly IConfiguration _config;


        public ApplicationDbSeeder(TenantInfo currentTenant, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, CustomSeederRunner seederRunner, ILogger<ApplicationDbSeeder> logger, IConfiguration config)
        {
            _currentTenant = currentTenant;
            _roleManager = roleManager;
            _userManager = userManager;
            _seederRunner = seederRunner;
            _logger = logger;
            _config = config;
        }

        public async Task SeedDatabaseAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
        {
            await SeedRolesAsync(dbContext);
            await SeedAdminUserAsync();
            await _seederRunner.RunSeedersAsync(cancellationToken);
        }

        private async Task SeedRolesAsync(ApplicationDbContext dbContext)
        {
            foreach (string roleName in Roles.DefaultRoles)
            {
                if (await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName)
                    is not ApplicationRole role)
                {
                    // Create the role
                    _logger.LogInformation("Seeding {role} Role for '{tenantId}' Tenant.", roleName, _currentTenant.Id);
                    role = new ApplicationRole(roleName, $"{roleName} Role for {_currentTenant.Id} Tenant");
                    await _roleManager.CreateAsync(role);
                }

                // Assign permissions
                if (roleName == Roles.Basic)
                {
                    await AssignPermissionsToRoleAsync(dbContext, Permissions.Basic, role);
                }
                else if (roleName == Roles.Admin)
                {
                    await AssignPermissionsToRoleAsync(dbContext, Permissions.Admin, role);

                    if (_currentTenant.Id == _config.GetSection(nameof(DefaultTenantSettings)).Get<DefaultTenantSettings>()!.Id)
                    {
                        await AssignPermissionsToRoleAsync(dbContext, Permissions.Root, role);
                    }
                }
            }
        }

        private async Task AssignPermissionsToRoleAsync(ApplicationDbContext dbContext, IReadOnlyList<Permission> permissions, ApplicationRole role)
        {
            var currentClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var permission in permissions)
            {
                if (!currentClaims.Any(c => c.Type == Claims.Permission && c.Value == permission.Name))
                {
                    _logger.LogInformation("Seeding {role} Permission '{permission}' for '{tenantId}' Tenant.", role.Name, permission.Name, _currentTenant.Id);
                    dbContext.RoleClaims.Add(new ApplicationRoleClaim
                    {
                        RoleId = role.Id,
                        ClaimType = Claims.Permission,
                        ClaimValue = permission.Name
                    });
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        private async Task SeedAdminUserAsync()
        {
            if (string.IsNullOrWhiteSpace(_currentTenant.Id) || string.IsNullOrWhiteSpace(_currentTenant.AdminEmail))
            {
                return;
            }

            if (await _userManager.Users.FirstOrDefaultAsync(u => u.Email == _currentTenant.AdminEmail)
                is not ApplicationUser adminUser)
            {
                string adminUserName = $"{_currentTenant.Id.Trim()}.{Roles.Admin}".ToLowerInvariant();
                adminUser = new ApplicationUser()
                {
                    FirstName = _currentTenant.Id.Trim().ToLowerInvariant(),
                    LastName = Roles.Admin,
                    Email = _currentTenant.AdminEmail,
                    UserName = adminUserName,
                    PhoneNumber = _currentTenant.Phonenumber,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    NormalizedEmail = _currentTenant.AdminEmail?.ToUpperInvariant(),
                    NormalizedUserName = adminUserName.ToUpperInvariant(),
                    IsActive = true
                };

                _logger.LogInformation("Seeding Default Admin User for '{tenantId}' Tenant.", _currentTenant.Id);
                var password = new PasswordHasher<ApplicationUser>();
                string defaultPassword = _config.GetSection(nameof(DefaultTenantSettings)).Get<DefaultTenantSettings>()!.Password;
                adminUser.PasswordHash = password.HashPassword(adminUser, defaultPassword);
                await _userManager.CreateAsync(adminUser);
            }

            // Assign role to user
            if (!await _userManager.IsInRoleAsync(adminUser, Roles.Admin))
            {
                _logger.LogInformation("Assigning Admin Role to Admin User for '{tenantId}' Tenant.", _currentTenant.Id);
                await _userManager.AddToRoleAsync(adminUser, Roles.Admin);
            }
        }
    }
}