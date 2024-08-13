using Academy.Application.Academies.Dto;
using Academy.Application.Sport.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Application.Academies.Query.Models
{
    public class GetSportDetailsRequest : IRequest<Result<SportsDto>>
    {
        public DefaultIdType Id { get; set; }
    }
}
