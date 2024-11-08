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
        public bool IsActive { get; set; }
        public DefaultIdType SportsId { get; set; }
        public DefaultIdType BatchId { get; set; }
        public DefaultIdType CoachingId { get; set; }
        

        public DefaultIdType SubscriptionId { get; set; }

        public int EnrollmentFee { get; set; }

        [ForeignKey("SportsId")]
        public virtual Sports Sports { get; set; }

        [ForeignKey("BatchId")]
        public virtual Batch Batch { get; set; }

        [ForeignKey("CoachingId")]
        public virtual Coaching Coaching { get; set; }

        [ForeignKey("SubscriptionId")]
        public virtual Subscription Subscription { get; set; }
        public UserInfo()
        {

        }

        public UserInfo(DefaultIdType id, string userId, string userName, string? fatherName, string? email, DateOnly dateOfJoin, DateOnly dateOfBirth, int? age, string address, string gender, string city, int pinCode, string contactNo, string? profilePic, DefaultIdType sportsId, DefaultIdType batchId, DefaultIdType coachingId, DefaultIdType subscriptionId, int enrollmentFee, bool isActive)
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
            SubscriptionId = subscriptionId;
            EnrollmentFee = enrollmentFee;
            IsActive = isActive;
        }

        public UserInfo Update(string userId, string userName, string? fatherName, string? email, DateOnly dateOfJoin, DateOnly dateOfBirth, int? age, string address, string gender, string city, int pinCode, string contactNo, string? profilePic, DefaultIdType sportsId, DefaultIdType batchId, DefaultIdType coachingId, DefaultIdType subscriptionId, int enrollmentFee, bool isActive)
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
            SubscriptionId = subscriptionId;
            EnrollmentFee = enrollmentFee;
            IsActive = isActive;
            return this;
        }
    }
}
