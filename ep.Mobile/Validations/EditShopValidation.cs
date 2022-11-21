using ep.Mobile.ViewModels;
using FluentValidation;

namespace ep.Mobile.Validations
{
    public class EditShopValidation : AbstractValidator<EditShopPageModel>
    {
        public EditShopValidation()
        {
            RuleFor(x => x.ABN).NotEmpty();
            RuleFor(x => x.Email).NotNull().EmailAddress();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.BusinessName).NotEmpty();
            RuleFor(x => x.Owner).NotEmpty();
            RuleFor(x => x.Phone).NotEmpty();
        }
    }
}
