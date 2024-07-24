using Academy.Application.Contracts.Persistence;
using Academy.Domain.Entities;
using Mapster;

namespace Academy.Application.Settings
{
    public class GetSettingDetailsRequest : IRequest<Result<SettingDto>>
    {
        public DefaultIdType Id { get; set; }
    }

    public class GetSettingDetailsRequestHandler(IRepository<Setting> settingRepository) : IRequestHandler<GetSettingDetailsRequest, Result<SettingDto>>
    {
        private readonly IRepository<Setting> _settingRepository = settingRepository;

        public async Task<Result<SettingDto>> Handle(GetSettingDetailsRequest request, CancellationToken cancellationToken) =>
             Result.Succeed((await _settingRepository.GetByIdAsync(request.Id)).Adapt<SettingDto>());
    }
}
