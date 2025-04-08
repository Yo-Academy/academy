using Academy.Application.Common.Exceptions;
using Academy.Application.Persistence.Repository;
using Academy.Application.UserPaymentInfo.Command.Models;
using Academy.Application.UserPaymentInfo.Dto;
using Mapster;
using Entities = Academy.Domain.Entities;

namespace Academy.Application.UserPaymentInfo.Command.Handlers
{
    public class UpdateUserPaymentInfoRequestHandler : IRequestHandler<UpdateUserPaymentInfoRequest, Result<UserPaymentInfoDto>>
    {
        private readonly IRepository<Entities.UserPaymentInfo> _repository;
        public UpdateUserPaymentInfoRequestHandler(IRepository<Entities.UserPaymentInfo> repository)
        {
            _repository = repository;
        }
        public async Task<Result<UserPaymentInfoDto>> Handle(UpdateUserPaymentInfoRequest request, CancellationToken cancellationToken)
        {
            var UserPaymentInfoToUpdate = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (UserPaymentInfoToUpdate == null)
                return Result.Fail(new NotFoundException("UserPaymentInfo Not Found"));

            UserPaymentInfoToUpdate.Update(request.PaymentTypeId,request.UserInfoId,request.ReceiptNumber,
                request.ReceiptDate,request.PaymentDate, request.SubscriptionStartDate, request.SubscriptionEndDate, 
                request.Fee,request.Remark,request.PaymentImage);

            await _repository.UpdateAsync(UserPaymentInfoToUpdate, cancellationToken);
            return Result.Succeed(UserPaymentInfoToUpdate.Adapt<UserPaymentInfoDto>());
        }
    }
}
