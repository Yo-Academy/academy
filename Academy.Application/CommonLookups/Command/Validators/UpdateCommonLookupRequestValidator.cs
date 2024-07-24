using Academy.Application.CommonLookups.Command.Models;
using Academy.Application.CommonLookups.Specifications;
using Academy.Application.Persistence.Repository;
using Academy.Domain.Entities;

namespace Academy.Application.CommonLookups.Command.Validators;

public class UpdateCommonLookupRequestValidator : CustomValidator<UpdateCommonLookupRequest>
{
    public UpdateCommonLookupRequestValidator(IRepository<CommonLookup> repository)
    {
        RuleFor(p => p.Key)
            .NotEmpty()
            .MaximumLength(100);            

        RuleFor(p => new { p.Category, p.Key })
            .MustAsync(async (commonLookup, combo, _) =>
            {
                var existing = await repository.AnyAsync(new GetCommonLookupByKeySpec(combo.Key, combo.Category, commonLookup.Id));
                return !existing;
            })
            .WithMessage((_, combo) => string.Format("CommonLookup with Type '{0}' and Key '{1}' already Exists.", combo.Category, combo.Key));
           
        RuleFor(p => p.DisplayOrder)
          .NotEmpty()
          .InclusiveBetween(0, int.MaxValue);
    }
}
