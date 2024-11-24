using Ardalis.Specification;
using Entites = Academy.Domain.Entities;


namespace Academy.Application.UserPaymentInfo.Specifications
{
    public class GetUserPaymentInfoByIdSpec : Specification<Entites.UserPaymentInfo>, ISingleResultSpecification<Entites.UserPaymentInfo>
    {
        public GetUserPaymentInfoByIdSpec(DefaultIdType id)
        {
            Query.Where(x => x.Id == id)
                   .Include(x => x.PaymentType)
                   .Include(x => x.UserInfo);
        }
    }
}
