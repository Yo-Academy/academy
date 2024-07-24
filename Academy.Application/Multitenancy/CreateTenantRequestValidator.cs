
namespace Academy.Application.Multitenancy
{
    public class CreateTenantRequestValidator : CustomValidator<CreateTenantRequest>
    {
        public CreateTenantRequestValidator(
            ITenantService tenantService,
            IConnectionStringValidator connectionStringValidator)
        {
            RuleFor(t => t.Id).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MustAsync(async (id, _) => !await tenantService.ExistsWithIdAsync(id))
                .WithMessage((_, id) => string.Format(DbRes.T("TenantAlreadyExistsWithParamsMsg", id)));

            RuleFor(t => t.Name).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MustAsync(async (name, _) => !await tenantService.ExistsWithNameAsync(name!))
                .WithMessage((_, name) => string.Format(DbRes.T("TenantAlreadyExistsWithParamsMsg", name)));

            RuleFor(t => t.ConnectionString).Cascade(CascadeMode.Stop)
                .Must((_, cs) => string.IsNullOrWhiteSpace(cs) || connectionStringValidator.TryValidate(cs))
                .WithMessage(DbRes.T("ConnectionStringInvalidMsg"));

            RuleFor(t => t.AdminEmail).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .EmailAddress();
        }
    }
}