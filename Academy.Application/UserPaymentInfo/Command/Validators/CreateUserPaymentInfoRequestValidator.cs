using Academy.Application.Persistence.Repository;
using Academy.Application.UserPaymentInfo.Command.Models;
using Entities = Academy.Domain.Entities;

namespace Academy.Application.UserPaymentInfo.Command.Validators
{
    public class CreateUserPaymentInfoRequestValidator : CustomValidator<CreateUserPaymentInfoRequest>
    {
        public CreateUserPaymentInfoRequestValidator(IRepository<Entities.UserPaymentInfo> repository)
        {
            RuleFor(p => p.PaymentTypeId)
             .NotEmpty();

            RuleFor(p => p.UserInfoId)
             .NotEmpty();

            RuleFor(p => p.ReceiptNumber)
             .NotEmpty();

            RuleFor(p => p.PaymentDate)
             .NotEmpty();
            
            RuleFor(p => p.SubscriptionStartDate)
            .NotEmpty();

            RuleFor(p => p.SubscriptionEndDate)
            .NotEmpty();

            RuleFor(p => p.Fee)
             .NotEmpty();
        }
    }
}
