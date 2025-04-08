using Academy.Application.Common.Exceptions;
using Academy.Application.Persistence.Repository;
using Academy.Application.UserInfo.Command.Models;
using Academy.Application.UserInfo.Dto;
using Mapster;
using Entities = Academy.Domain.Entities;


namespace Academy.Application.UserInfo.Command.Handlers
{
    public class DeleteUserInfoRequestHandler : IRequestHandler<DeleteUserInfoRequest, Result<UserInfoDto>>
    {
        private readonly IRepository<Entities.UserInfo> _repository;
        public DeleteUserInfoRequestHandler(IRepository<Entities.UserInfo> repository)
        {
            _repository = repository;
        }
        public async Task<Result<UserInfoDto>> Handle(DeleteUserInfoRequest request, CancellationToken cancellationToken)
        {
            var UserInfoToDelete = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (UserInfoToDelete == null)
                return Result.Fail(new NotFoundException(DbRes.T("UserInfoNotFound")));

            await _repository.DeleteAsync(UserInfoToDelete, cancellationToken);
            return Result.Succeed(UserInfoToDelete.Adapt<UserInfoDto>());
        }
    }
}
