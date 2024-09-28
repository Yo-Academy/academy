using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Infrastructure.Multitenancy
{
    public interface ITenantResolverService
    {
        Task<string?> ResolveTenantIdAsync(HttpContext context);
        Task<TenantInfo?> GetTenantInfoAsync(string tenantId);
        Task SwitchTenantAsync(HttpContext context, string tenantId, CancellationToken cancellationToken = default!);
        Task RevertToPreviousTenantAsync(HttpContext context, CancellationToken cancellationToken = default!);
    }
}
