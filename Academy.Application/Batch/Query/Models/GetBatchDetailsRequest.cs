using Academy.Application.Batch.Dto;

namespace Academy.Application.Batch.Query.Models
{
    public class GetBatchDetailsRequest : IRequest<Result<BatchDetailsDto>>
    {
        public DefaultIdType Id { get; set; }
    }
   
}
