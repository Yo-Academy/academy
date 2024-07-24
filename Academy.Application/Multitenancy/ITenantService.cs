namespace Academy.Application.Multitenancy
{
    public interface ITenantService
    {
        Task<List<TenantDto>> GetAllAsync();
        Task<bool> ExistsWithIdAsync(string id);
        Task<bool> ExistsWithNameAsync(string name);
        Task<TenantDto> GetByIdAsync(string id);
        Task<TenantDto> CreateAsync(CreateTenantRequest request, CancellationToken cancellationToken);
        Task<TenantDto> ActivateAsync(string id);
        Task<TenantDto> DeactivateAsync(string id);
        Task<TenantDto> UpdateSubscription(string id, DateTime extendedExpiryDate);
    }
}