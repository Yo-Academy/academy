using Academy.Application.Common.Exceptions;
using Academy.Application.Persistence.Repository;
using Academy.Application.UserInfo.Command.Models;
using Academy.Application.UserInfo.Dto;
using Mapster;
using Entities = Academy.Domain.Entities;

namespace Academy.Application.UserInfo.Command.Handlers
{
    public class UpdateUserInfoRequestHandler : IRequestHandler<UpdateUserInfoRequest, Result<UserInfoDto>>
    {
        private readonly IRepository<Entities.UserInfo> _repository;
        public UpdateUserInfoRequestHandler(IRepository<Entities.UserInfo> repository)
        {
            _repository = repository;
        }
        public async Task<Result<UserInfoDto>> Handle(UpdateUserInfoRequest request, CancellationToken cancellationToken)
        {
            var UserInfoToUpdate = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (UserInfoToUpdate == null)
                return Result.Fail(new NotFoundException("UserInfo Not Found"));

            UserInfoToUpdate.Update(request.UserId, request.UserName, request.FatherName, request.Email,
                                                                   request.DateOfJoin, request.DateOfBirth, request.Age, request.Address, request.Gender,
                                                                   request.City, request.PinCode, request.ContactNo, request.ProfilePic, request.SportsId,
                                                                   request.BatchId, request.CoachingId, request.SubscriptionId,
                                                                   request.EnrollmentFee, request.IsActive);

            await _repository.UpdateAsync(UserInfoToUpdate, cancellationToken);
            return Result.Succeed(UserInfoToUpdate.Adapt<UserInfoDto>());
        }
    }
}
