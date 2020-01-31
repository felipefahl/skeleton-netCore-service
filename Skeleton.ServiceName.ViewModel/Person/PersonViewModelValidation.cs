using FluentValidation;
using Skeleton.ServiceName.Utils.Helpers;
using Skeleton.ServiceName.Utils.Resources;
using Skeleton.ServiceName.ViewModel.People;
using System;

namespace Skeleton.ServiceName.ViewModel.Person
{
    public class PersonViewModelValidation : AbstractValidator<PersonViewModel>
    {
        private static DateTime _minBirthDate = DateTimeHelper.BrazilNow.AddYears(-100);

        public PersonViewModelValidation()
        {
            RuleFor(person => person.FirstName)
                .NotEmpty().WithMessage(string.Format(Global.Required, "FirstName"));

            RuleFor(device => device.LastName)
                .NotEmpty().WithMessage(string.Format(Global.Required, "LastName"));

            RuleFor(device => device.BirthDate)
                .NotEmpty().WithMessage(string.Format(Global.Required, "BirthDate"))
                .Must(MinBirthDate).WithMessage(string.Format(Global.GreaterThan, "BirthDate", _minBirthDate.ToString()));
        }

        private static bool MinBirthDate(DateTime birthDate)
        {
            return birthDate >= _minBirthDate;
        }
    }
}
