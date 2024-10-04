using Academy.Application.Persistence.Repository;
using Academy.Application.Subscription.Command.Models;
using Academy.Application.Subscription.Dto;
using Mapster;
using Entities = Academy.Domain.Entities;


namespace Academy.Application.Subscription.Command.Handlers
{
    public class CreateSubscriptionRequestHandler : IRequestHandler<CreateSubscriptionRequest, Result<SubscriptionDto>>
    {
        private readonly IRepository<Entities.Subscription> _repository;

        public CreateSubscriptionRequestHandler(IRepository<Entities.Subscription> repository)
        {
            _repository = repository;
        }
        public async Task<Result<SubscriptionDto>> Handle(CreateSubscriptionRequest request, CancellationToken cancellationToken)
        {
            var id = DefaultIdType.NewGuid();
            Entities.Subscription Subscription = new Entities.Subscription(id, request.SportsId, request.BatchId, request.CoachingId, request.PlanTypeId, request.Fee, request.IsActive);

            //Inserts RequirementSet Record
            var responseSubscription = await _repository.AddAsync(Subscription);

            return Result.Succeed(responseSubscription.Adapt<SubscriptionDto>());
        }
    }

}
