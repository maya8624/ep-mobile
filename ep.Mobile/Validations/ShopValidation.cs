using ep.Mobile.PageModels;
using FluentValidation;

namespace ep.Mobile.Validations
{
    public class ShopValidation : AbstractValidator<ShopPageModel>
    {
        public ShopValidation()
        {
            RuleFor(x => x.ABN).NotEmpty();
            RuleFor(x => x.Email).NotNull().EmailAddress();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.BusinessName).NotEmpty();
            RuleFor(x => x.Owner).NotEmpty();
            RuleFor(x => x.Phone).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(4);
            RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(x => x.Password);
        }
    }
}
