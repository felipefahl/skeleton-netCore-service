using FluentValidation.TestHelper;
using Skeleton.ServiceName.Utils.Helpers;
using Skeleton.ServiceName.Utils.Resources;
using Skeleton.ServiceName.ViewModel.People;
using Skeleton.ServiceName.ViewModel.Person;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Skeleton.ServiceName.UnitTest.ViewModel
{
    public class PersonViewModelValidationTest
    {
        private readonly PersonViewModelValidation validator;
        public PersonViewModelValidationTest()
        {
            validator = new PersonViewModelValidation();
        }

        [Fact]
        public void FirstName_Required()
        {
            // Arrange

            // Act

            // Assert
            validator.ShouldHaveValidationErrorFor(person => person.FirstName, null as string)
                .WithErrorMessage(string.Format(Global.Required, "FirstName"));
        }

        [Fact]
        public void LastName_Required()
        {
            // Arrange

            // Act

            // Assert
            validator.ShouldHaveValidationErrorFor(person => person.LastName, null as string)
                .WithErrorMessage(string.Format(Global.Required, "LastName"));
        }

        [Fact]
        public void BirthDate_Required()
        {
            // Arrange
            var model = new PersonViewModel();
            // Act

            // Assert
            validator.ShouldHaveValidationErrorFor(person => person.BirthDate, model)
                .WithErrorMessage(string.Format(Global.Required, "BirthDate"));
        }

        [Fact]
        public void BirthDate_MinDate()
        {
            // Arrange
            var minDate = DateTimeHelper.BrazilNow.AddYears(-100);
            var model = new PersonViewModel { BirthDate = DateTimeHelper.BrazilNow.AddYears(-150) };
            // Act

            // Assert
            validator.ShouldHaveValidationErrorFor(person => person.BirthDate, model)
                .WithErrorMessage(string.Format(Global.GreaterThan, "BirthDate", minDate.ToString()));
        }
    }
}
