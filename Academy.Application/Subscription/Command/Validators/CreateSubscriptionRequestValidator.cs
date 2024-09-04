using Academy.Application.Persistence.Repository;
using Academy.Application.Subscription.Command.Models;
using Entities = Academy.Domain.Entities;

namespace Academy.Application.Subscription.Command.Validators
{
    public class CreateSubscriptionRequestValidator : CustomValidator<CreateSubscriptionRequest>
    {
        public CreateSubscriptionRequestValidator(IRepository<Entities.Subscription> repository)
        {
            RuleFor(p => p.SportsId)
             .NotEmpty();

            RuleFor(p => p.BatchId)
             .NotEmpty();

            RuleFor(p => p.PlanTypeId)
             .NotEmpty();

            RuleFor(p => p.Fee)
             .NotEmpty();

            RuleFor(p => p.CoachingId)
             .NotEmpty();
        }
    }
}
