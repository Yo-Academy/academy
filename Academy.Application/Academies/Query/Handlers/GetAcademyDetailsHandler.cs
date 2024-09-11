using Academy.Application.Academies.Dto;
using Academy.Application.Academies.Query.Models;
using Academy.Application.Persistence.Repository;
using Academy.Domain.Entities;
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
        public GetAcademyDetailsHandler(IReadRepository<Entity.Academies> academyReadRepository)
        {
            _academyReadRepository = academyReadRepository;
        }

        public async Task<Result<AcademyDetailDto>> Handle(GetAcademyDetailsRequest request, CancellationToken cancellationToken)
        {

            return Result.Succeed(new AcademyDetailDto());
        }
    }
}
