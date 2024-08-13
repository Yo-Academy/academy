using Academy.Application.Sport.Dto;

namespace Academy.Application.Sport.Command.Models
{
    public class DeleteSportRequest : IRequest<Result<SportsDto>>
    {
        public DefaultIdType Id { get; set; }
    }
}
