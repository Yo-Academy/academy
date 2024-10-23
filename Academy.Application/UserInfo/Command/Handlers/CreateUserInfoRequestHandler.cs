using Academy.Application.Persistence.Repository;
using Academy.Application.UserInfo.Command.Models;
using Academy.Application.UserInfo.Dto;
using Academy.Domain.Entities;
using Mapster;
using System.Net;
using Entities = Academy.Domain.Entities;


namespace Academy.Application.UserInfo.Command.Handlers
{
    public class CreateUserInfoRequestHandler : IRequestHandler<CreateUserInfoRequest, Result<UserInfoDto>>
    {
        private readonly IRepository<Entities.UserInfo> _repository;

        public CreateUserInfoRequestHandler(IRepository<Entities.UserInfo> repository)
        {
            _repository = repository;
        }
        public async Task<Result<UserInfoDto>> Handle(CreateUserInfoRequest request, CancellationToken cancellationToken)
        {
            var id = DefaultIdType.NewGuid();
            Entities.UserInfo UserInfo = new Entities.UserInfo(id, request.UserId, request.UserName, request.FatherName, request.Email, 
                                                                   request.DateOfJoin, request.DateOfBirth, request.Age, request.Address, request.Gender,
                                                                   request.City, request.PinCode, request.ContactNo, request.ProfilePic, request.SportsId,
                                                                   request.BatchId, request.CoachingId, request.SubscriptionId, 
                                                                   request.EnrollmentFee, request.IsActive);

            //Inserts RequirementSet Record
            var responseUserInfo = await _repository.AddAsync(UserInfo);
            return Result.Succeed(responseUserInfo.Adapt<UserInfoDto>());
        }
    }

}
