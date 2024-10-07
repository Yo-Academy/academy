namespace Academy.Application.UserInfo.Dto
{
    public class UserInfoDto
    {
        public DefaultIdType Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string? FatherName { get; set; }
        public string? Email { get; set; }
        public DateOnly DateOfJoin { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public int PinCode { get; set; }
        public string ContactNo { get; set; }
        public string? ProfilePic { get; set; }
        public bool IsACtive { get; set; }
        public DefaultIdType SportsId { get; set; }
        public DefaultIdType BatchId { get; set; }
        public DefaultIdType CoachingId { get; set; }
        public DefaultIdType PlanTypeId { get; set; }
        public DefaultIdType SubscriptionId { get; set; }
        public int EnrollmentFee { get; set; }

        public  SportsDto Sports { get; set; }
        public  BatchDto Batch { get; set; }
        public  CoachingDto Coaching { get; set; }
        public PlanTypeDto PlanType { get; set; }
        public SubscriptionDto Subscription { get; set; }
    }
}
