using FluentValidation.TestHelper;
using Skeleton.ServiceName.Utils.Resources;
using Skeleton.ServiceName.ViewModel.Authentication;
using Xunit;

namespace Skeleton.ServiceName.UnitTest.ViewModel
{
    public class LoginViewModelValidationTest
    {
        private  readonly LoginViewModelValidation validator;
        public LoginViewModelValidationTest()
        {
            validator = new LoginViewModelValidation();
        }

        [Fact]
        public void Password_Required()
        {
            // Arrange

            // Act

            // Assert
            validator.ShouldHaveValidationErrorFor(login => login.Password, null as string)
                .WithErrorMessage(string.Format(Global.Required, "Password"));
        }

        [Fact]
        public void Email_Required()
        {
            // Arrange

            // Act

            // Assert
            validator.ShouldHaveValidationErrorFor(login => login.Email, null as string)
                .WithErrorMessage(string.Format(Global.Required, "Email"));
        }
    }
}
