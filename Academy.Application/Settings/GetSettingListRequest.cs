using Academy.Application.Contracts.Persistence;
using Academy.Domain.Entities;

namespace Academy.Application.Settings
{
    public class GetSettingListRequest() : IRequest<Result<List<Setting>>>;

    public class GetSettingListRequestHandler(IRepository<Setting> settingRepository) : IRequestHandler<GetSettingListRequest, Result<List<Setting>>>
    {
        private readonly IRepository<Setting> _settingRepository = settingRepository;

        public async Task<Result<List<Setting>>> Handle(GetSettingListRequest request, CancellationToken cancellationToken) =>
            Result.Succeed(_settingRepository.Get(x => !x.IsDeleted).OrderByDescending(x => x.CreatedOn).ToList());
    }
}
