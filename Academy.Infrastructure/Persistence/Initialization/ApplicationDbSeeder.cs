using Academy.Application.Academies.Command.Models;
using Academy.Application.Multitenancy;
using Academy.Application.Persistence.Repository;
using Academy.Domain.Entities;
using Academy.Infrastructure.Multitenancy;
using Academy.Infrastructure.Persistence.Context;
using Academy.Infrastructure.Persistence.Initialization;
using Academy.Shared.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

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
        private readonly IRepository<Permissions> _permissionsRepository;


        public ApplicationDbSeeder(TenantInfo currentTenant, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, CustomSeederRunner seederRunner, ILogger<ApplicationDbSeeder> logger, IConfiguration config,
            IRepository<Permissions> permissionsRepository)
        {
            _currentTenant = currentTenant;
            _roleManager = roleManager;
            _userManager = userManager;
            _seederRunner = seederRunner;
            _logger = logger;
            _config = config;
            _permissionsRepository = permissionsRepository;
        }

        public async Task SeedDatabaseAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
        {
            // Seed default permissions if there is no permission in table
            await SeedPermisisonsAsync();
            await SeedRolesAsync(dbContext);
            await SeedAdminUserAsync();
            await _seederRunner.RunSeedersAsync(cancellationToken);
        }

        private async Task SeedRolesAsync(ApplicationDbContext dbContext)
        {
            var permissions = await dbContext.Permissions.ToListAsync();
            List<Permission> permissionsList = [];
            foreach (var permission in permissions)
            {
                Permission per = new(permission.Description ?? "", permission.Action, permission.Resource, permission.IsBasic, permission.IsRoot);
                permissionsList.Add(per);
            }
            IReadOnlyList<Permission> readOnlyPermissionsList = permissionsList.AsReadOnly();
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
                if (roleName == Roles.Admin || roleName == Roles.Owner)
                {
                    await AssignPermissionsToRoleAsync(dbContext, readOnlyPermissionsList.Where(x => x.IsBasic).ToList(), role);
                }
                else if (roleName == Roles.SAdmin)
                {
                    await AssignPermissionsToRoleAsync(dbContext, readOnlyPermissionsList, role);

                    if (_currentTenant.Id == _config.GetSection(nameof(DefaultTenantSettings)).Get<DefaultTenantSettings>()!.Id)
                    {
                        await AssignPermissionsToRoleAsync(dbContext, readOnlyPermissionsList.Where(x => x.IsRoot).ToList(), role);
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
                string adminUserName = $"{_currentTenant.Id.Trim()}.{Roles.SAdmin}".ToLowerInvariant();
                adminUser = new ApplicationUser()
                {
                    FirstName = _currentTenant.Id.Trim().ToLowerInvariant(),
                    LastName = Roles.Admin,
                    Email = _currentTenant.AdminEmail,
                    PhoneNumber = _config.GetSection(nameof(DefaultTenantSettings)).Get<DefaultTenantSettings>()!.Phonenumber,
                    UserName = adminUserName,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    NormalizedEmail = _currentTenant.AdminEmail?.ToUpperInvariant(),
                    NormalizedUserName = adminUserName.ToUpperInvariant(),
                    IsActive = true,
                    CountryCode ="91"
                };

                _logger.LogInformation("Seeding Default Admin User for '{tenantId}' Tenant.", _currentTenant.Id);
                var password = new PasswordHasher<ApplicationUser>();
                string defaultPassword = _config.GetSection(nameof(DefaultTenantSettings)).Get<DefaultTenantSettings>()!.Password;
                adminUser.PasswordHash = password.HashPassword(adminUser, defaultPassword);
                await _userManager.CreateAsync(adminUser);
            }

            // Assign role to user
            if (!await _userManager.IsInRoleAsync(adminUser, Roles.SAdmin))
            {
                _logger.LogInformation("Assigning Admin Role to Admin User for '{tenantId}' Tenant.", _currentTenant.Id);
                await _userManager.AddToRoleAsync(adminUser, Roles.SAdmin);
            }
        }

        private async Task SeedPermisisonsAsync()
        {
            if (!await _permissionsRepository.AnyAsync())
            {
                var permissions = PermissionsList.All;

                var enitiyPermissions = new List<Permissions>();
                foreach (var permission in permissions)
                {
                    enitiyPermissions.Add(new Permissions(
                        Guid.NewGuid(),
                        permission.Action, permission.Resource,
                        permission.Description, permission.IsBasic, permission.IsRoot)
                    );
                }

                _logger.LogInformation("Seeding Permissions for '{tenantId}' Tenant.", _currentTenant.Id);
                await _permissionsRepository.AddRangeAsync(enitiyPermissions);
            }
        }

        internal async Task CreateAcadmyUsers(CreateAcademyUserRequest request, CancellationToken cancellationToken)
        {
            await SeedUserWithRoleAsync(request);
        }

        private async Task SeedUserWithRoleAsync(CreateAcademyUserRequest request)
        {
            if (string.IsNullOrWhiteSpace(_currentTenant.Id) || string.IsNullOrWhiteSpace(_currentTenant.AdminEmail))
            {
                return;
            }

            if (await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber && u.TenantId == _currentTenant.Id)
                is not ApplicationUser user)
            {
                string userName = $"{request.PhoneNumber}.{request.Role}".ToLowerInvariant();
                user = new ApplicationUser()
                {
                    FirstName = request.FullName.Split(" ")[0],
                    LastName = request.FullName.Split(" ")[1] ?? "",
                    Email = _currentTenant.AdminEmail,
                    PhoneNumber = request.PhoneNumber,
                    UserName = userName,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    NormalizedUserName = userName.ToUpperInvariant(),
                    IsActive = true,
                    CountryCode = "91"
                };

                _logger.LogInformation("Seeding Default Admin User for '{tenantId}' Tenant.", _currentTenant.Id);
                var password = new PasswordHasher<ApplicationUser>();
                string defaultPassword = _config.GetSection(nameof(DefaultTenantSettings)).Get<DefaultTenantSettings>()!.Password;
                user.PasswordHash = password.HashPassword(user, defaultPassword);
                await _userManager.CreateAsync(user);
            }

            // Assign role to user
            if (!await _userManager.IsInRoleAsync(user, request.Role))
            {
                _logger.LogInformation("Assigning Admin Role to Admin User for '{tenantId}' Tenant.", _currentTenant.Id);
                await _userManager.AddToRoleAsync(user, request.Role);
            }
        }

    }
}