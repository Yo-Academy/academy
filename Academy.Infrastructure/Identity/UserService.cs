using Academy.Application.Common.Events;
using Academy.Application.Common.Mailing;
using Academy.Application.Common.Storage;
using Academy.Application.Contracts.Persistence;
using Academy.Application.Identity.Users;
using Academy.Infrastructure.Auth;
using Academy.Infrastructure.Multitenancy;
using Academy.Infrastructure.Persistence.Context;
using Academy.Shared.Authorization;
using Finbuckle.MultiTenant;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Academy.Infrastructure.Identity
{
    internal partial class UserService : IUserService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _db;
        private readonly SecuritySettings _securitySettings;
        private readonly IEventPublisher _events;
        private readonly ITenantInfo _currentTenant;
        private readonly IStorage _storage;
        private readonly ICacheService _cache;
        private readonly ICacheKeyService _cacheKeys;
        private readonly IJobService _jobService;
        private readonly IEmailHelper _emailHelper;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly IConfiguration _config;
        private readonly ITenantResolver _tenantResolver;
        private readonly IMultiTenantContextAccessor _multiTenantContextAccessor;
        private readonly TenantDbContext _dbTenant;

        public UserService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ApplicationDbContext db,
            IEventPublisher events,
            ITenantInfo currentTenant,
            IOptions<SecuritySettings> securitySettings,
            IStorage storage,
            ICacheService cache,
            ICacheKeyService cacheKeys,
            IJobService jobService,
            IEmailHelper emailHelper,
            IEmailTemplateRepository emailTemplateRepository,
            IConfiguration config,
            ITenantResolver tenantResolver,
            IMultiTenantContextAccessor multiTenantContextAccessor,
            TenantDbContext dbTenant)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _events = events;
            _currentTenant = currentTenant;
            _securitySettings = securitySettings.Value;
            _storage = storage;
            _cache = cache;
            _cacheKeys = cacheKeys;
            _jobService = jobService;
            _emailHelper = emailHelper;
            _emailTemplateRepository = emailTemplateRepository;
            _config = config;
            _tenantResolver = tenantResolver;
            _multiTenantContextAccessor = multiTenantContextAccessor;
            _dbTenant = dbTenant;
        }

        //public async Task<PaginationResponse<UserDetailsDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken)
        //{
        //    var spec = new EntitiesByPaginationFilterSpec<ApplicationUser>(filter);

        //    var users = await _userManager.Users
        //        .WithSpecification(spec)
        //        .ProjectToType<UserDetailsDto>()
        //        .ToListAsync(cancellationToken);
        //    int count = await _userManager.Users
        //        .CountAsync(cancellationToken);

        //    return new PaginationResponse<UserDetailsDto>(users, count, filter.PageNumber, filter.PageSize);
        //}

        public async Task<bool> ExistsWithNameAsync(string name)
        {
            EnsureValidTenant();
            return await _userManager.FindByNameAsync(name) is not null;
        }

        public async Task<bool> ExistsWithEmailAsync(string email, DefaultIdType? exceptId = null)
        {
            EnsureValidTenant();
            return await _userManager.FindByEmailAsync(email.Normalize()) is ApplicationUser user && user.Id != exceptId;
        }

        public async Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, DefaultIdType? exceptId = null)
        {
            EnsureValidTenant();
            return await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber) is ApplicationUser user && user.Id != exceptId;
        }

        private void EnsureValidTenant()
        {
            if (string.IsNullOrWhiteSpace(_currentTenant?.Id))
            {
                throw new UnauthorizedException(DbRes.T("InvalidTenantMsg"));
            }
        }

        public async Task<Result<List<UserDetailsDto>>> GetListAsync(CancellationToken cancellationToken) =>
            Result.Succeed((await _userManager.Users
                    .AsNoTracking()
                    .ToListAsync(cancellationToken))
                .Adapt<List<UserDetailsDto>>());

        public Task<int> GetCountAsync(CancellationToken cancellationToken) =>
            _userManager.Users.AsNoTracking().CountAsync(cancellationToken);

        public async Task<Result<UserDetailsDto>> GetAsync(DefaultIdType userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            _ = user ?? throw new NotFoundException(DbRes.T("UserNotFoundMsg"));

            return Result.Succeed(user.Adapt<UserDetailsDto>());
        }

        public async Task<Result<UserDetailsDto>> ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);

            _ = user ?? throw new NotFoundException(DbRes.T("UserNotFoundMsg"));

            bool isAdmin = await _userManager.IsInRoleAsync(user, Roles.Admin);
            if (isAdmin)
            {
                throw new ConflictException(DbRes.T("AdminProfileStatusNotToggledMsg"));
            }

            user.IsActive = request.ActivateUser;

            await _userManager.UpdateAsync(user);

            await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id));

            return Result.Succeed(user.Adapt<UserDetailsDto>());
        }

    }
}