using Academy.Application.Academies.Command.Models;
using Academy.Application.Academies.Dto;
using Academy.Application.Common.Helpers;
using Academy.Application.Common.Storage;
using Academy.Application.Common.Storage.Filters;
using Academy.Application.Common.Storage.Models;
using Academy.Application.Multitenancy;
using Academy.Application.Persistence.Repository;
using Academy.Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.IO;
using static Academy.Shared.Constants;
using Entities = Academy.Domain.Entities;

namespace Academy.Application.Academies.Command.Handlers
{
    public class CreateAcademiesRequestHandler : IRequestHandler<CreateAcademiesRequest, Result<AcademmyDetailsDto>>
    {
        private readonly IRepository<Entities.Academies> _repository;
        private readonly IRepository<AcademySportsMapping> _repositoryAcademyMapping;
        private readonly IStorage _storage;
        private readonly ITenantService _tenantService;

        public CreateAcademiesRequestHandler(IRepository<Entities.Academies> repository,
            IStorage storage,
            IRepository<AcademySportsMapping> repositoryAcademyMapping,
            ITenantService tenantService)
        {
            _repository = repository;
            _storage = storage;
            _repositoryAcademyMapping = repositoryAcademyMapping;
            _tenantService = tenantService;
        }
        public async Task<Result<AcademmyDetailsDto>> Handle(CreateAcademiesRequest request, CancellationToken cancellationToken)
        {
            var id = DefaultIdType.NewGuid();
            string logoName = string.Empty;
            string QRName = string.Empty;

            // Upload Logo
            if (request.LogoImage != null)
            {
                logoName = string.Format("{0}_{1}_logo.{2}", id, request.ShortName, request.LogoImage.GetFileExtension());
                using (var stream = request.LogoImage.OpenReadStream())
                {
                    var res = await _storage.UploadAsync(stream, new UploadOptions() { FileName = logoName, Directory = S3Directory.Logo }, cancellationToken);
                    if (!res.IsSuccess)
                    {
                        return Result.Fail("Logo file uploaded issue.");
                    }
                }
            }

            // Upload QR
            if (request.QRImage != null)
            {
                QRName = string.Format("{0}_{1}_qr.{2}", id, request.ShortName, request.QRImage.GetFileExtension());
                using (var stream = request.QRImage.OpenReadStream())
                {
                    var res = await _storage.UploadAsync(stream, new UploadOptions() { FileName = QRName, Directory = S3Directory.QR }, cancellationToken);
                    if (!res.IsSuccess)
                    {
                        return Result.Fail("Logo file uploaded issue.");
                    }
                }
            }

            Entities.Academies Academies = new Entities.Academies(id, request.Name, request.ShortName, request.AcademyId,
                request.GST, request.Address, request.City, request.Pincode, logoName, QRName);

            var result = new AcademmyDetailsDto();

            //Create A new Tenant for academy
            string tanentId = request.ShortName.GetSubdomainFromShortName();
            CreateTenantRequest requestTenant = new CreateTenantRequest()
            {
                Id = tanentId,
                Name = tanentId
            };

            if (!await _tenantService.ExistsWithNameAsync(tanentId))
            {
                var tenantInfo = await _tenantService.CreateAsync(requestTenant, cancellationToken);
                result.Tenant = tenantInfo.Adapt<TenantDto>();
            }
            else
            {
                return Result.Fail(DbRes.T("AcademyExists"));
            }

            //Inserts RequirementSet Record
            var responseAcademy = await _repository.AddAsync(Academies);

            var academyList = new List<AcademySportsMapping>();
            foreach (var item in request.sports)
            {
                academyList.Add(new AcademySportsMapping(Guid.NewGuid(), Academies.Id, item));
            }

            if (academyList.Count > 0)
            {
                await _repositoryAcademyMapping.AddRangeAsync(academyList);
            }

            if (responseAcademy != null)
            {
                result.Academy = responseAcademy.Adapt<AcademiesDto>();
                return Result.Succeed(result);
            }

            return Result.Fail();
        }
    }

}
