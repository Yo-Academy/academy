using Academy.Application.PaymentType.Command.Validators;
using Academy.Application.PaymentType.Dto;

namespace Academy.Application.PaymentType.Command.Models
{
    public class CreatePaymentTypeRequest : IRequest<Result<PaymentTypeDto>>
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
