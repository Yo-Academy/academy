using Academy.Application.Persistence.Repository;
using Academy.Application.UserPaymentInfo.Command.Models;
using Academy.Application.UserPaymentInfo.Dto;
using Mapster;
using Entities = Academy.Domain.Entities;

namespace Academy.Application.UserPaymentInfo.Command.Handlers
{
    public class CreateUserPaymentInfoRequestHandler : IRequestHandler<CreateUserPaymentInfoRequest, Result<UserPaymentInfoDto>>
    {
        private readonly IRepository<Entities.UserPaymentInfo> _repository;

        public CreateUserPaymentInfoRequestHandler(IRepository<Entities.UserPaymentInfo> repository)
        {
            _repository = repository;
        }
        public async Task<Result<UserPaymentInfoDto>> Handle(CreateUserPaymentInfoRequest request, CancellationToken cancellationToken)
        {
            var id = DefaultIdType.NewGuid();
            Entities.UserPaymentInfo UserPaymentInfo = new Entities.UserPaymentInfo(id, request.PaymentTypeId, 
                                                        request.UserInfoId, request.ReceiptNumber, request.ReceiptDate, request.PaymentDate, request.SubscriptionStartDate,
                                                        request.SubscriptionStartDate,request.Fee, request.Remark, request.PaymentImage);

            //Inserts RequirementSet Record
            var responseUserPaymentInfo = await _repository.AddAsync(UserPaymentInfo);

            return Result.Succeed(responseUserPaymentInfo.Adapt<UserPaymentInfoDto>());
        }
    }

}
