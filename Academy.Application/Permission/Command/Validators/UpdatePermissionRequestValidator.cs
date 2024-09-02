using Academy.Domain.Entities;
using Academy.Application.Permission.Command.Models;
using Academy.Application.Contracts;
using Academy.Application.CommonLookups.Specifications;
using Academy.Application.Persistence.Repository;

namespace Academy.Application.Permission.Command.Validators
{
    public class UpdatePermissionRequestValidator : CustomValidator<UpdatePermissionRequest>
    {
        public UpdatePermissionRequestValidator(IRepository<Permissions> repository)
        {
            RuleFor(p => p.Action)
                .NotEmpty()
                .WithMessage(DbRes.T("ActionRequiredMsg"));

            RuleFor(p => p.Resource)
                .NotEmpty()
                .WithMessage(DbRes.T("ResourceRequiredMsg"));

            RuleFor(p => new { p.Action, p.Resource, p.Id })
            .MustAsync(async (combo, _) =>
            {
                var existing = await repository.AnyAsync(new GetPermissionByActionAndResourceSpec(combo.Action, combo.Resource, combo.Id));
                return !existing;
            })
            .WithMessage((_, combo) => string.Format("Permission with Action '{0}' and Resource '{1}' already Exists.", combo.Action, combo.Resource));
        }
    }
}
