using Academy.Application.Academies.Command.Models;
using Academy.Application.Academies.Dto;
using Academy.Application.Academies.Specifications;
using Academy.Application.Common.Exceptions;
using Academy.Application.Common.Storage;
using Academy.Application.Common.Storage.Filters;
using Academy.Application.Common.Storage.Models;
using Academy.Application.Multitenancy;
using Academy.Application.Persistence.Repository;
using Academy.Domain.Entities;
using Academy.Shared;
using Mapster;
using static Academy.Shared.Constants;
using Entites = Academy.Domain.Entities;

namespace Academy.Application.Academies.Command.Handlers
{
    public class UpdateAcademiesRequestHandler : IRequestHandler<UpdateAcademiesRequest, Result<AcademiesDto>>
    {
        private readonly IRepository<Entites.Academies> _repository;
        private readonly IRepository<AcademySportsMapping> _repositoryAcademyMapping;
        private readonly IStorage _storage;
        private readonly ITenantService _tenantService;
        private readonly IReadRepository<Setting> _readRepoSetting;
        public UpdateAcademiesRequestHandler(IRepository<Entites.Academies> repository,
            IStorage storage,
            IRepository<AcademySportsMapping> repositoryAcademyMapping,
            ITenantService tenantService,
            IReadRepository<Setting> readRepoSetting)
        {
            _repository = repository;
            _storage = storage;
            _repositoryAcademyMapping = repositoryAcademyMapping;
            _tenantService = tenantService;
            _readRepoSetting = readRepoSetting;
        }
        public async Task<Result<AcademiesDto>> Handle(UpdateAcademiesRequest request, CancellationToken cancellationToken)
        {
            var AcademiesToUpdate = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (AcademiesToUpdate == null)
                return Result.Fail(new NotFoundException(DbRes.T("ErrorMessageAcademyNotFound")));

            string logoName = string.Empty;
            string QRName = string.Empty;

            // Upload Logo
            if (request.Logo != null)
            {
                logoName = string.Format("{0}_{1}_logo{2}", AcademiesToUpdate.Id, request.ShortName, request.Logo.GetFileExtension());
                using (var stream = request.Logo.OpenReadStream())
                {
                    var res = await _storage.UploadAsync(stream, new UploadOptions() { FileName = logoName, Directory = S3Directory.Logo }, cancellationToken);
                    if (!res.IsSuccess)
                    {
                        return Result.Fail("Logo file uploaded issue.");
                    }
                }
            }

            // Upload QR
            if (request.QR != null)
            {
                QRName = string.Format("{0}_{1}_qr{2}", AcademiesToUpdate.Id, request.ShortName, request.QR.GetFileExtension());
                using (var stream = request.QR.OpenReadStream())
                {
                    var res = await _storage.UploadAsync(stream, new UploadOptions() { FileName = QRName, Directory = S3Directory.QR }, cancellationToken);
                    if (!res.IsSuccess)
                    {
                        return Result.Fail("Logo file uploaded issue.");
                    }
                }
            }

            AcademiesToUpdate.Update(request.Name, request.ShortName, request.GST, request.Address, request.City,
                request.Pincode, request.IsActive);
            AcademiesToUpdate.Logo = logoName;
            AcademiesToUpdate.QRCode = QRName;

            await _repository.UpdateAsync(AcademiesToUpdate, cancellationToken);

            var getMappings = new GetAcademySportsByAcademyIdSpec(AcademiesToUpdate.Id);
            var sports = await _repositoryAcademyMapping.ListAsync(getMappings);

            if (sports != null)
                await _repositoryAcademyMapping.DeleteRangeAsync(sports);

            var academyList = new List<AcademySportsMapping>();
            foreach (var item in request.Sports)
            {
                academyList.Add(new AcademySportsMapping(Guid.NewGuid(), AcademiesToUpdate.Id, item));
            }

            if (academyList.Count > 0)
            {
                await _repositoryAcademyMapping.AddRangeAsync(academyList);
            }

            return Result.Succeed(AcademiesToUpdate.Adapt<AcademiesDto>());
        }
    }
}
