using Academy.Application.Academies.Command.Models;
using Academy.Application.Academies.Specifications;
using Academy.Application.CommonLookups.Specifications;
using Academy.Application.Persistence.Repository;
using Academy.Shared;
using Entites = Academy.Domain.Entities;

namespace Academy.Application.Academies.Command.Validators
{
    public class CreateAcademiesRequestValidator : CustomValidator<CreateAcademiesRequest>
    {
        public CreateAcademiesRequestValidator(IRepository<Entites.Academies> repository)
        {
            RuleFor(p => p.Name)
               .NotEmpty()
               .MaximumLength(50)
               .Matches(Constants.ValidationRegex.Name);

            RuleFor(p => p.ShortName)
               .NotEmpty()
               .MaximumLength(100)
               .Matches(Constants.ValidationRegex.Name);

            RuleFor(p => p.Name)
                .MustAsync(async (name, _) =>
                {
                    var existing = await repository.AnyAsync(new GetAcademyByNameSpec(name));
                    return !existing;
                })
                .WithMessage((_, name) => string.Format(DbRes.T("AcademyExistsWithName"), name));

            RuleFor(p => p.Address)
              .NotEmpty()
              .MaximumLength(255);

            RuleFor(p => p.City)
              .NotEmpty();
            //.Matches(Constants.ValidationRegex.Pincode)
            //.WithMessage((_) => DbRes.T("EnterValidPincode"));

            RuleFor(p => p.GST)
                .Length(15, 15)
                .WithMessage("GST must be 15 characters if entered.")
                .When(x => !string.IsNullOrEmpty(x.GST));
        }
    }
}
