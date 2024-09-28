using Academy.Application.Academies.Command.Models;
using Academy.Application.Academies.Dto;
using Academy.Application.Common.Helpers;
using Academy.Application.Common.Storage;
using Academy.Application.Common.Storage.Filters;
using Academy.Application.Common.Storage.Models;
using Academy.Application.Multitenancy;
using Academy.Application.Persistence.Repository;
using Academy.Domain.Entities;
using Academy.Shared;
using Mapster;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.IO;
using System.Numerics;
using static Academy.Shared.Constants;
using Entities = Academy.Domain.Entities;

namespace Academy.Application.Academies.Command.Handlers
{
    public class CreateAcademiesRequestHandler : IRequestHandler<CreateAcademiesRequest, Result<AcademyDetailsDto>>
    {
        private readonly IRepository<Entities.Academies> _repository;
        private readonly IStorage _storage;
        private readonly ITenantService _tenantService;
        private readonly IReadRepository<Setting> _readRepoSetting;

        public CreateAcademiesRequestHandler(IRepository<Entities.Academies> repository,
            IStorage storage,
            ITenantService tenantService,
            IReadRepository<Setting> readRepoSetting)
        {
            _repository = repository;
            _storage = storage;
            _tenantService = tenantService;
            _readRepoSetting = readRepoSetting;
        }
        public async Task<Result<AcademyDetailsDto>> Handle(CreateAcademiesRequest request, CancellationToken cancellationToken)
        {
            var id = DefaultIdType.NewGuid();
            string logoName = string.Empty;
            string QRName = string.Empty;

            // Upload Logo
            if (request.Logo != null)
            {
                logoName = string.Format("{0}_{1}_logo{2}", id, request.ShortName, request.Logo.GetFileExtension());
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
                QRName = string.Format("{0}_{1}_qr{2}", id, request.ShortName, request.QR.GetFileExtension());
                using (var stream = request.QR.OpenReadStream())
                {
                    var res = await _storage.UploadAsync(stream, new UploadOptions() { FileName = QRName, Directory = S3Directory.QR }, cancellationToken);
                    if (!res.IsSuccess)
                    {
                        return Result.Fail("Logo file uploaded issue.");
                    }
                }
            }

            // Generate AcademyId 
            int cnt = await _repository.CountAsync();
            int PadLeft = 4;
            if (cnt > 9999)
            {
                PadLeft += 2;
            }

            // Create entity object
            Entities.Academies Academies = new Entities.Academies(id, request.Name, request.ShortName,
                request.GST, request.Address, request.City, request.Pincode, logoName, QRName,
                request.ShortName.GetSubdomainFromShortName(), cnt++, PadLeft);

            var result = new AcademyDetailsDto();

            //Create A new Tenant for academy
            string tanentId = request.ShortName.GetSubdomainFromShortName();
            CreateTenantRequest requestTenant = new CreateTenantRequest()
            {
                Id = tanentId,
                Name = tanentId,
                AdminEmail = tanentId + DefaultDomain
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
            try
            {
                Academies.Subdomain = tanentId;

                Academies.AcademySportsMappings = new List<AcademySportsMapping>();
                foreach (var item in request.Sports)
                {
                    Academies.AcademySportsMappings.Add(new AcademySportsMapping(Guid.NewGuid(), Academies.Id, item));
                }
                
                var responseAcademy = await _repository.AddAsync(Academies);

                //if (academyList.Count > 0)
                //{
                //    await _repositoryAcademyMapping.AddRangeAsync(academyList);
                //}

                if (responseAcademy != null)
                {
                    result.Academy = responseAcademy.Adapt<AcademiesDto>();
                    return Result.Succeed(result);
                }
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
            return Result.Fail();
        }
    }

}
