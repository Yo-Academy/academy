using Academy.Application.Persistence.Repository;
using Academy.Application.UserPaymentInfo.Dto;
using Academy.Application.UserPaymentInfo.Query.Models;
using Mapster;
using Entity = Academy.Domain.Entities;


namespace Academy.Application.UserPaymentInfo.Query.Handlers
{
    public class GetUserPaymentInfoDetailHandler : IRequestHandler<GetUserPaymentInfoDetailRequest, Result<UserPaymentInfoDto>>
    {
        private readonly IReadRepository<Entity.UserPaymentInfo> _userPaymentInfoReadRepository;

        public GetUserPaymentInfoDetailHandler(IReadRepository<Entity.UserPaymentInfo> userPaymentInfoReadRepository)
        {
            _userPaymentInfoReadRepository = userPaymentInfoReadRepository;
        }

        public async Task<Result<UserPaymentInfoDto>> Handle(GetUserPaymentInfoDetailRequest request, CancellationToken cancellationToken)
        {
            var userPaymentInfoDetail = new UserPaymentInfoDto();
            var data = await _userPaymentInfoReadRepository.GetByIdAsync(request.Id, cancellationToken);
            if (data != null)
            {
                userPaymentInfoDetail = data.Adapt<UserPaymentInfoDto>();
            }
            return Result.Succeed(userPaymentInfoDetail);
        }
    }
}
