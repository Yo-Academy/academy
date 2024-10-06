namespace Academy.Domain.Entities
{
    public class UserInfo : AuditableEntity, IAggregateRoot
    {
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

        [ForeignKey("SportsId")]
        public virtual Sports Sports { get; set; }

        [ForeignKey("BatchId")]
        public virtual Batch Batch { get; set; }

        [ForeignKey("CoachingId")]
        public virtual Coaching Coaching { get; set; }

        [ForeignKey("PlanTypeId")]
        public virtual PlanType PlanType { get; set; }

        [ForeignKey("SubscriptionId")]
        public virtual Subscription Subscription { get; set; }
        public UserInfo()
        {

        }

        public UserInfo(DefaultIdType id, string userId, string userName, string? fatherName, string? email, DateOnly dateOfJoin, DateOnly dateOfBirth, int? age, string address, string gender, string city, int pinCode, string contactNo, string? profilePic, DefaultIdType sportsId, DefaultIdType batchId, DefaultIdType coachingId, DefaultIdType planTypeId, DefaultIdType subscriptionId, int enrollmentFee, bool isACtive)
        {
            Id = id;
            UserId = userId;
            UserName = userName;
            FatherName = fatherName;
            Email = email;
            DateOfJoin = dateOfJoin;
            DateOfBirth = dateOfBirth;
            Age = age;
            Address = address;
            Gender = gender;
            City = city;
            PinCode = pinCode;
            ContactNo = contactNo;
            ProfilePic = profilePic;
            SportsId = sportsId;
            BatchId = batchId;
            CoachingId = coachingId;
            PlanTypeId = planTypeId;
            SubscriptionId = subscriptionId;
            EnrollmentFee = enrollmentFee;
            IsACtive = isACtive;
        }

        public UserInfo Update(string userId, string userName, string? fatherName, string? email, DateOnly dateOfJoin, DateOnly dateOfBirth, int? age, string address, string gender, string city, int pinCode, string contactNo, string? profilePic, DefaultIdType sportsId, DefaultIdType batchId, DefaultIdType coachingId, DefaultIdType planTypeId,DefaultIdType subscriptionId, int enrollmentFee, bool isACtive)
        {
            UserId = userId;
            UserName = userName;
            FatherName = fatherName;
            Email = email;
            DateOfJoin = dateOfJoin;
            DateOfBirth = dateOfBirth;
            Age = age;
            Address = address;
            Gender = gender;
            City = city;
            PinCode = pinCode;
            ContactNo = contactNo;
            ProfilePic = profilePic;
            SportsId = sportsId;
            BatchId = batchId;
            CoachingId = coachingId;
            PlanTypeId = planTypeId;
            SubscriptionId = subscriptionId;
            EnrollmentFee = enrollmentFee;
            IsACtive = isACtive;
            return this;
        }
    }
}
