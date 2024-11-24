using Academy.Application.Persistence.Repository;
using Academy.Application.PaymentType.Command.Models;
using Entites = Academy.Domain.Entities;

namespace Academy.Application.PaymentType.Command.Validators
{
    public class CreatePaymentTypeRequestValidator : CustomValidator<CreatePaymentTypeRequest>
    {
        public CreatePaymentTypeRequestValidator(IRepository<Entites.PaymentType> repository)
        {
            RuleFor(m => m.Code).NotEmpty();
            RuleFor(m => m.Name).NotEmpty();
        }
    }
}
