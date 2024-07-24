using Academy.Application.CommonLookups.Command.Models;
using Academy.Application.CommonLookups.Specifications;
using Academy.Application.Persistence.Repository;
using Academy.Domain.Entities;

namespace Academy.Application.CommonLookups.Command.Validators;

public class CreateCommonLookupRequestValidator : CustomValidator<CreateCommonLookupRequest>
{
    public CreateCommonLookupRequestValidator(IRepository<CommonLookup> repository)
    {
        RuleFor(p => p.Key)
        .NotEmpty()
        .MaximumLength(100);            

        RuleFor(p => new { p.Category, p.Key })
            .MustAsync(async (combo, _) =>
            {
                var existing = await repository.AnyAsync(new GetCommonLookupByKeySpec(combo.Key, combo.Category));
                return !existing;
            })
            .WithMessage((_, combo) => string.Format("CommonLookup with Category '{0}' and Key '{1}' already Exists.", combo.Category, combo.Key));

        RuleFor(p => p.DisplayOrder)
          .NotEmpty()
          .InclusiveBetween(0, int.MaxValue);

        RuleFor(p => p.CommonLookUpTranslations)
            .Must(list => list != null && list.Count > 0)
                .WithMessage(DbRes.T("LookupTranslationsRequiredMsg"));
    }
}
