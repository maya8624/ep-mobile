using ep.Mobile.PageModels;
using FluentValidation;

namespace ep.Mobile.Validations
{
    public class LoginValidation : AbstractValidator<LoginPageModel>
    {
        public LoginValidation()
        {
            RuleFor(x => x.Email).NotNull().EmailAddress();
            RuleFor(x => x.Password).NotNull().MinimumLength(4);
        }
    }
}
