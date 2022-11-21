using ep.Mobile.ViewModels;
using FluentValidation;

namespace ep.Mobile.Validations
{
    public class ResetPasswordValidation : AbstractValidator<ResetPasswordModel>
    {
        public ResetPasswordValidation()
        {
            //RuleFor(x => x.Email).NotNull().EmailAddress();
            RuleFor(x => x.CurrentPassword).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(4);
            RuleFor(x => x.ConfirmPassword).Equal(x => x.NewPassword).WithMessage("Confirm Password must be equal to New Password.");
        }
    }
}
