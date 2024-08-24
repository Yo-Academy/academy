using Academy.Application.Coaching.Command.Models;
using Academy.Application.Persistence.Repository;
using Entites = Academy.Domain.Entities;


namespace Academy.Application.Coaching.Command.Validators
{
    public class CreateCoachingRequestValidator : CustomValidator<CreateCoachingRequest>
    {
        public CreateCoachingRequestValidator(IRepository<Entites.Coaching> repository)
        {
            RuleFor(p => p.Code)
             .NotEmpty();

            RuleFor(p => p.Name)
              .NotEmpty();
        }
    }
}
