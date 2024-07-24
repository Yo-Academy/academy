using Academy.Application.Common.Exceptions;
using Academy.Application.Contracts.Persistence;
using Academy.Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace Academy.Application.Settings
{
    public class CreateSettingRequest : IRequest<Result<SettingDto>>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string Key { get; set; } = default!;
        public string Value { get; set; } = default!;
        public string? Type { get; set; }
        public int? DisplayOrder { get; set; }
    }

    public class CreateSettingRequestValidator : CustomValidator<CreateSettingRequest>
    {
        public CreateSettingRequestValidator(IRepository<Setting> settingRepository)
        {
            RuleFor(p => p.Name)
           .NotEmpty()
               .WithMessage(DbRes.T("SettingNameRequiredMsg"))
           .Must((setting, name, _) => !settingRepository.Get(x => x.Name == name && x.Key == setting.Key).Any())
                .WithMessage((_, name) => DbRes.T("SettingAlreadyExistMsg"));

            RuleFor(p => p.Key)
            .NotEmpty()
                .WithMessage(DbRes.T("SettingKeyRequiredMsg"));

            RuleFor(p => p.Value)
           .NotEmpty()
               .WithMessage(DbRes.T("SettingValueRequiredMsg"));
        }
    }

    public class CreateSettingRequestHandler(IRepository<Setting> settingRepository) : IRequestHandler<CreateSettingRequest, Result<SettingDto>>
    {
        private readonly IRepository<Setting> _settingRepository = settingRepository;

        public async Task<Result<SettingDto>> Handle(CreateSettingRequest setting, CancellationToken cancellationToken)
        {
            var settingData = await _settingRepository.AddAsync(setting.Adapt<Setting>());
            return Result.Succeed(settingData.Adapt<SettingDto>());
        }
    }
}
