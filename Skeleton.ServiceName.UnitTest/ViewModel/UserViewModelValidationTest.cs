using FluentValidation.TestHelper;
using Skeleton.ServiceName.Utils.Resources;
using Skeleton.ServiceName.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Skeleton.ServiceName.UnitTest.ViewModel
{
    public class UserViewModelValidationTest
    {
        private readonly UserViewModelValidation validator;
        public UserViewModelValidationTest()
        {
            validator = new UserViewModelValidation();
        }

        [Fact]
        public void Name_Required()
        {
            // Arrange

            // Act

            // Assert
            validator.ShouldHaveValidationErrorFor(user => user.Name, null as string)
                .WithErrorMessage(string.Format(Global.Required, "Name"));
        }

        [Fact]
        public void Password_Required()
        {
            // Arrange

            // Act

            // Assert
            validator.ShouldHaveValidationErrorFor(user => user.Password, null as string)
                .WithErrorMessage(string.Format(Global.Required, "Password"));
        }

        [Fact]
        public void Email_Required()
        {
            // Arrange

            // Act

            // Assert
            validator.ShouldHaveValidationErrorFor(user => user.Email, null as string)
                .WithErrorMessage(string.Format(Global.Required, "Email"));
        }

        [Fact]
        public void Email_Invalid()
        {
            // Arrange
            var model = new UserViewModel
            {
                Email = "123"
            };

            // Act

            // Assert
            validator.ShouldHaveValidationErrorFor(user => user.Email, model)
                .WithErrorMessage(Global.InvalidEmail);
        }

        [Fact]
        public void PasswordCheck_Required()
        {
            // Arrange
            var model = new UserViewModel
            {
                Password = "123"
            };

            // Act

            // Assert
            validator.ShouldHaveValidationErrorFor(user => user.PasswordCheck, model)
                .WithErrorMessage(string.Format(Global.Required, "PasswordCheck"));
        }

        [Fact]
        public void PasswordCheck_NotMatch()
        {
            // Arrange
            var model = new UserViewModel {
                Password = "123",
                PasswordCheck = "321"
            };

            // Act

            // Assert
            validator.ShouldHaveValidationErrorFor(user => user.PasswordCheck, model)
                .WithErrorMessage(Global.PasswordDoesNotMatch);
        }

        [Fact]
        public void PasswordCheck_Match()
        {
            // Arrange
            var model = new UserViewModel
            {
                Password = "123",
                PasswordCheck = "123"
            };

            // Act

            // Assert
            validator.ShouldNotHaveValidationErrorFor(user => user.PasswordCheck, model);
        }

        [Fact]
        public void Profile_Required()
        {
            // Arrange

            // Act

            // Assert
            validator.ShouldHaveValidationErrorFor(user => user.Profile, null as string)
                .WithErrorMessage(string.Format(Global.Required, "Profile"));
        }
    }
}
