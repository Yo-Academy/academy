using Academy.Application.Academies.Dto;

namespace Academy.Application.Academies.Command.Models
{
    public class DeleteAcademiesRequest : IRequest<Result<AcademiesDto>>
    {
        public DefaultIdType Id { get; set; }
    }
}
