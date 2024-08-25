using Academy.Application.Common.Exceptions;
using Academy.Application.Persistence.Repository;
using Academy.Application.Subscription.Command.Models;
using Academy.Application.Subscription.Dto;
using Mapster;
using Entities = Academy.Domain.Entities;


namespace Academy.Application.Subscription.Command.Handlers
{
    public class DeleteSubscriptionRequestHandler : IRequestHandler<DeleteSubscriptionRequest, Result<SubscriptionDto>>
    {
        private readonly IRepository<Entities.Subscription> _repository;
        public DeleteSubscriptionRequestHandler(IRepository<Entities.Subscription> repository)
        {
            _repository = repository;
        }
        public async Task<Result<SubscriptionDto>> Handle(DeleteSubscriptionRequest request, CancellationToken cancellationToken)
        {
            var SubscriptionToDelete = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (SubscriptionToDelete == null)
                return Result.Fail(new NotFoundException("Common Lookup Not Found"));

            await _repository.DeleteAsync(SubscriptionToDelete, cancellationToken);
            return Result.Succeed(SubscriptionToDelete.Adapt<SubscriptionDto>());
        }
    }
}
