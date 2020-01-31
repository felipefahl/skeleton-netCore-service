using FluentValidation;
using Skeleton.ServiceName.Utils.Resources;

namespace Skeleton.ServiceName.ViewModel.User
{
    public class UserViewModelValidation : AbstractValidator<UserViewModel>
    {

        public UserViewModelValidation()
        {
            RuleFor(user => user.Name)
                .NotEmpty().WithMessage(string.Format(Global.Required, "Name"));

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage(string.Format(Global.Required, "Email"))
                .EmailAddress().WithMessage(Global.InvalidEmail);

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage(string.Format(Global.Required, "Password"));

            RuleFor(user => user.PasswordCheck)
                .NotEmpty().WithMessage(string.Format(Global.Required, "PasswordCheck"))
                .Equal(user => user.Password).When(user => !string.IsNullOrEmpty(user.Password)).WithMessage(Global.PasswordDoesNotMatch);

            RuleFor(user => user.Profile)
                .NotEmpty().WithMessage(string.Format(Global.Required, "Profile"));
        }
    }
}
