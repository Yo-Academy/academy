using Academy.Application.Common.Exceptions;
using Academy.Application.Multitenancy;

namespace Academy.API.Controllers.Multitenancy
{
    public class TenantsController : VersionNeutralApiController
    {
        [HttpGet]
        [MustHavePermission(Shared.Authorization.Action.View, Resource.Tenants)]
        [OpenApiOperation("Get a list of all tenants.", "")]
        public async Task<ActionResult> GetListAsync()
        {
            return Ok(await Mediator.Send(new GetAllTenantsRequest()));
        }

        [HttpGet("{id}")]
        [MustHavePermission(Shared.Authorization.Action.View, Resource.Tenants)]
        [OpenApiOperation("Get tenant details.", "")]
        public async Task<ActionResult> GetAsync(string id)
        {
            return Ok(await Mediator.Send(new GetTenantRequest(id)));
        }

        [HttpPost]
        [MustHavePermission(Shared.Authorization.Action.Create, Resource.Tenants)]
        [OpenApiOperation("Create a new tenant.", "")]
        public async Task<ActionResult> CreateAsync(CreateTenantRequest request)
        {
            Result<TenantDto> tenant = await Mediator.Send(request);
            return CreatedAtRoute(new { id = tenant.Value?.Id }, tenant);
        }

        [HttpPost("{id}/activate")]
        [MustHavePermission(Shared.Authorization.Action.Update, Resource.Tenants)]
        [OpenApiOperation("Activate a tenant.", "")]
        [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
        public async Task<ActionResult> ActivateAsync(string id)
        {
            return Ok(await Mediator.Send(new ActivateTenantRequest(id)));
        }

        [HttpPost("{id}/deactivate")]
        [MustHavePermission(Shared.Authorization.Action.Update, Resource.Tenants)]
        [OpenApiOperation("Deactivate a tenant.", "")]
        [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
        public async Task<ActionResult> DeactivateAsync(string id)
        {
            return Ok(await Mediator.Send(new DeactivateTenantRequest(id)));
        }

        [HttpPost("{id}/upgrade")]
        [MustHavePermission(Shared.Authorization.Action.UpgradeSubscription, Resource.Tenants)]
        [OpenApiOperation("Upgrade a tenant's subscription.", "")]
        [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
        public async Task<ActionResult> UpgradeSubscriptionAsync(string id, UpgradeSubscriptionRequest request)
        {
            return id != request.TenantId
                ? BadRequest(Result.Fail(new BadRequestException(DbRes.T("BadRequest"))))
                : Ok(await Mediator.Send(request));
        }
    }
}