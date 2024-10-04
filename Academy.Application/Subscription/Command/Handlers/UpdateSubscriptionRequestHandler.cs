using Academy.Application.Common.Exceptions;
using Academy.Application.Persistence.Repository;
using Academy.Application.Subscription.Command.Models;
using Academy.Application.Subscription.Dto;
using Mapster;
using Entities = Academy.Domain.Entities;


namespace Academy.Application.Subscription.Command.Handlers
{
    public class UpdateSubscriptionRequestHandler : IRequestHandler<UpdateSubscriptionRequest, Result<SubscriptionDto>>
    {
        private readonly IRepository<Entities.Subscription> _repository;
        public UpdateSubscriptionRequestHandler(IRepository<Entities.Subscription> repository)
        {
            _repository = repository;
        }
        public async Task<Result<SubscriptionDto>> Handle(UpdateSubscriptionRequest request, CancellationToken cancellationToken)
        {
            var SubscriptionToUpdate = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (SubscriptionToUpdate == null)
                return Result.Fail(new NotFoundException(DbRes.T("SubscriptionNotFound")));

            SubscriptionToUpdate.Update(request.SportsId, request.BatchId, request.CoachingId, request.PlanTypeId, request.Fee, request.IsActive);

            await _repository.UpdateAsync(SubscriptionToUpdate, cancellationToken);
            return Result.Succeed(SubscriptionToUpdate.Adapt<SubscriptionDto>());
        }
    }
}
