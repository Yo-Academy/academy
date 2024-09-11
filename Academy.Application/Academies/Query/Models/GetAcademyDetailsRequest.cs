using Academy.Application.Academies.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Application.Academies.Query.Models
{
    public class GetAcademyDetailsRequest : IRequest<Result<AcademyDetailDto>>
    {
        public DefaultIdType Id { get; set; }
    }
}
