using Academy.Application.Common.Exceptions;
using Academy.Application.Contracts.Persistence;
using Academy.Domain.Entities;
using Mapster;

namespace Academy.Application.Settings
{
    public class UpdateSettingRequest : IRequest<Result<SettingDto>>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string Key { get; set; } = default!;
        public string Value { get; set; } = default!;
        public string? Type { get; set; }
        public int? DisplayOrder { get; set; }
        public DefaultIdType Id { get; set; }

    }

    public class UpdateSettingRequestValidator : CustomValidator<UpdateSettingRequest>
    {
        public UpdateSettingRequestValidator(IRepository<Setting> settingRepository)
        {
            RuleFor(p => p.Name)
           .NotEmpty()
               .WithMessage(DbRes.T("SettingNameRequiredMsg"))
            .Must((setting, name, _) => !settingRepository.Get(x => x.Name == name && x.Key == setting.Key && setting.Id != x.Id).Any())
                .WithMessage((_, name) => DbRes.T("SettingAlreadyExistMsg"));

            RuleFor(p => p.Key)
            .NotEmpty()
                .WithMessage(DbRes.T("SettingKeyRequiredMsg"));

            RuleFor(p => p.Value)
           .NotEmpty()
               .WithMessage(DbRes.T("SettingValueRequiredMsg"));
        }
    }

    public class UpdateSettingRequestHandler(IRepository<Setting> settingRepository) : IRequestHandler<UpdateSettingRequest, Result<SettingDto>>
    {
        private readonly IRepository<Setting> _settingRepository = settingRepository;

        public async Task<Result<SettingDto>> Handle(UpdateSettingRequest setting, CancellationToken cancellationToken)
        {
            var settingData = await _settingRepository.GetByIdAsync(setting.Id);
            if (settingData != null)
            {
                settingData.Name = setting.Name;
                settingData.Key = setting.Key;
                settingData.Value = setting.Value;
                settingData.Description = setting.Description;
                settingData.Type = setting.Type;
                settingData.DisplayOrder = setting.DisplayOrder;
                await _settingRepository.UpdateAsync(settingData);
                return Result.Succeed(settingData.Adapt<SettingDto>());
            }
            else
            {
                throw new CustomException(DbRes.T("SettingDoesNotExistMsg"));
            }
        }
    }
}
