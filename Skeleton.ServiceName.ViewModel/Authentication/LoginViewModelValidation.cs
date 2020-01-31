using FluentValidation;
using Skeleton.ServiceName.Utils.Resources;

namespace Skeleton.ServiceName.ViewModel.Authentication
{
    public class LoginViewModelValidation : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidation()
        {
            RuleFor(person => person.Email)
                .NotEmpty().WithMessage(string.Format(Global.Required, "Email"));

            RuleFor(device => device.Password)
                .NotEmpty().WithMessage(string.Format(Global.Required, "Password"));
        }
    }
}
