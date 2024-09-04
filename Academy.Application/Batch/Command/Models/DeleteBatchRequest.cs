using Academy.Application.Batch.Dto;

namespace Academy.Application.Batch.Command.Models
{
    public class DeleteBatchRequest : IRequest<Result<BatchDto>>
    {
        public DefaultIdType Id { get; set; }
    }
}
