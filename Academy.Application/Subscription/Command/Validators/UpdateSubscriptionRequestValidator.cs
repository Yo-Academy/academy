using Academy.Application.Persistence.Repository;
using Academy.Application.Subscription.Command.Models;
using Entites = Academy.Domain.Entities;


namespace Academy.Application.Subscription.Command.Validators
{
    public class UpdateSubscriptionRequestValidator : CustomValidator<UpdateSubscriptionRequest>
    {
        public UpdateSubscriptionRequestValidator(IRepository<Entites.Subscription> repository)
        {

        }
    }
}
