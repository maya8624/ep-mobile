using ep.Mobile.ViewModels;
using FluentValidation;

namespace ep.Mobile.Validations
{
    public class CustomerValidation : AbstractValidator<CustomerPageModel>
    {
        public CustomerValidation()
        {
            RuleFor(x => x.OrderNo).NotNull();
            RuleFor(x => x.Name).NotNull();
            RuleFor(x => x.Mobile).NotNull();
        }
    }
}
