using Academy.Application.Common.Exceptions;
using Academy.Application.Contracts.Persistence;
using Academy.Domain.Entities;

namespace Academy.Application.Settings
{
    public class DeleteSettingRequest : IRequest<Result<string>>
    {
        public DefaultIdType Id { get; set; }
    }

    public class DeleteSettingRequestHandler(IRepository<Setting> settingRepository) : IRequestHandler<DeleteSettingRequest, Result<string>>
    {
        private readonly IRepository<Setting> _settingRepository = settingRepository;

        public async Task<Result<string>> Handle(DeleteSettingRequest request, CancellationToken cancellationToken)
        {
            var setting = await _settingRepository.GetByIdAsync(request.Id);
            if (setting != null)
            {
                await _settingRepository.DeleteAsync(setting);
                return Result.Succeed(DbRes.T("SettingDeletedSuccessfullyMsg"));
            }
            else
            {
                throw new CustomException(DbRes.T("SettingDoesNotExistMsg"));
            }
        }
    }
}
