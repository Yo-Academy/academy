using Academy.Application.Common.Exceptions;
using Academy.Application.Persistence.Repository;
using Academy.Application.UserPaymentInfo.Command.Models;
using Academy.Application.UserPaymentInfo.Dto;
using Mapster;
using Entities = Academy.Domain.Entities;

namespace Academy.Application.UserPaymentInfo.Command.Handlers
{
    public class DeleteUserPaymentInfoRequestHandler : IRequestHandler<DeleteUserPaymentInfoRequest, Result<UserPaymentInfoDto>>
    {
        private readonly IRepository<Entities.UserPaymentInfo> _repository;
        public DeleteUserPaymentInfoRequestHandler(IRepository<Entities.UserPaymentInfo> repository)
        {
            _repository = repository;
        }
        public async Task<Result<UserPaymentInfoDto>> Handle(DeleteUserPaymentInfoRequest request, CancellationToken cancellationToken)
        {
            var UserPaymentInfoToDelete = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (UserPaymentInfoToDelete == null)
                return Result.Fail(new NotFoundException(DbRes.T("UserPaymentInfoNotFound")));

            await _repository.DeleteAsync(UserPaymentInfoToDelete, cancellationToken);
            return Result.Succeed(UserPaymentInfoToDelete.Adapt<UserPaymentInfoDto>());
        }
    }
}
