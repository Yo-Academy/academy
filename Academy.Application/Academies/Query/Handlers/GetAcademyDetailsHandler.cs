using Academy.Application.Academies.Dto;
using Academy.Application.Academies.Query.Models;
using Academy.Application.Academies.Specifications;
using Academy.Application.Common.Exceptions;
using Academy.Application.Persistence.Repository;
using Academy.Domain.Entities;
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
        public GetAcademyDetailsHandler(IReadRepository<Entity.Academies> academyReadRepository,
            IReadRepository<AcademySportsMapping> academySportMappnigReadRepository)
        {
            _academyReadRepository = academyReadRepository;
            _academySportMappnigReadRepository = academySportMappnigReadRepository;
        }

        public async Task<Result<AcademyDetailDto>> Handle(GetAcademyDetailsRequest request, CancellationToken cancellationToken)
        {
            var spec = new GetAcademyByIdSpec(request.Id);
            var academyDetails = await _academyReadRepository.SingleOrDefaultAsync(spec, cancellationToken);
            if (academyDetails == null)
            {
                return Result.Fail(new NotFoundException(DbRes.T("ErrorMessageAcademyNotFound")));
            }

            return Result.Succeed(academyDetails.Adapt<AcademyDetailDto>());
        }
    }
}
