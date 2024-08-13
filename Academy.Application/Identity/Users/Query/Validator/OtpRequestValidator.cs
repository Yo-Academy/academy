using Academy.Application.Identity.Users.Query.Model;

namespace Academy.Application.Identity.Users.Query.Validator
{
    public class OtpRequestValidator : CustomValidator<OtpRequest>
    {
        public OtpRequestValidator()
        {
            RuleFor(x => x.OTP).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(4);
        }
    }
}
