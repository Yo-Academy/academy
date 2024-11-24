using Academy.Application.Persistence.Repository;
using Academy.Application.PaymentType.Command.Models;
using Academy.Application.PaymentType.Dto;
using Mapster;
using Entites = Academy.Domain.Entities;

namespace Academy.Application.PaymentType.Command.Handlers
{
    public class CreatePaymentTypeRequestHandler : IRequestHandler<CreatePaymentTypeRequest, Result<PaymentTypeDto>>
    {
        private readonly IRepository<Entites.PaymentType> _repository;

        public CreatePaymentTypeRequestHandler(IRepository<Entites.PaymentType> repository)
        {
            _repository = repository;
        }
        public async Task<Result<PaymentTypeDto>> Handle(CreatePaymentTypeRequest request, CancellationToken cancellationToken)
        {
            var id = DefaultIdType.NewGuid();
            Entites.PaymentType PaymentType = new Entites.PaymentType(id, request.Code, request.Name);

            //Inserts RequirementSet Record
            var responsePaymentType = await _repository.AddAsync(PaymentType);

            return Result.Succeed(responsePaymentType.Adapt<PaymentTypeDto>());
        }
    }

}
