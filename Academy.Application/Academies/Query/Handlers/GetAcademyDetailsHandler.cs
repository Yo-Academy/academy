using Academy.Application.Academies.Contracts;
using Academy.Application.Academies.Dto;
using Academy.Application.Academies.Query.Models;
using Academy.Application.Academies.Specifications;
using Academy.Application.Common.Exceptions;
using Academy.Application.Identity.Users.Specifications;
using Academy.Application.Multitenancy;
using Academy.Application.Persistence.Repository;
using Academy.Domain.Entities;
using Academy.Domain.Identity;
using Academy.Shared.Authorization;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = Academy.Domain.Entities;
namespace Academy.Application.Academies.Query.Handlers
{
    public class GetAcademyDetailsHandler : IRequestHandler<GetAcademyDetailsRequest, Result<AcademyDetailDto>>
    {
        private readonly IReadRepository<Entity.Academies> _academyReadRepository;
        private readonly IReadRepository<AcademySportsMapping> _academySportMappnigReadRepository;
        private readonly IReadOnlyRepository<ApplicationUser> _userReadOnlyRepository;
        private readonly ITenantService _tenantService;

        public GetAcademyDetailsHandler(IReadRepository<Entity.Academies> academyReadRepository,
            IReadRepository<AcademySportsMapping> academySportMappnigReadRepository,
            IReadOnlyRepository<ApplicationUser> userReadOnlyRepository,
            ITenantService tenantService)
        {
            _academyReadRepository = academyReadRepository;
            _academySportMappnigReadRepository = academySportMappnigReadRepository;
            _userReadOnlyRepository = userReadOnlyRepository;
            _tenantService = tenantService;
        }

        public async Task<Result<AcademyDetailDto>> Handle(GetAcademyDetailsRequest request, CancellationToken cancellationToken)
        {
            var spec = new GetAcademyByIdSpec(request.Id);
            var academyDetails = await _academyReadRepository.SingleOrDefaultAsync(spec, cancellationToken);
            if (academyDetails == null)
            {
                return Result.Fail(new NotFoundException(DbRes.T("ErrorMessageAcademyNotFound")));
            }

            var owners = await _tenantService.GetUsersAsync(academyDetails.Subdomain ?? string.Empty, Roles.Owner, cancellationToken);
            var admins = await _tenantService.GetUsersAsync(academyDetails.Subdomain ?? string.Empty, Roles.Admin, cancellationToken);

            return Result.Succeed(academyDetails.Adapt<AcademyDetailDto>());
        }
    }
}
