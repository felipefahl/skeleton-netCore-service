using NSubstitute;
using Skeleton.ServiceName.Business.Implementations.Validations;
using Skeleton.ServiceName.Data.Interfaces;
using Skeleton.ServiceName.MockData.Classes;
using Skeleton.ServiceName.Utils.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Skeleton.ServiceName.UnitTest.Business
{
    public class UserValidateServiceTest
    {
        private readonly IUserRepository _repositoryMock;

        private readonly UserValidationService _userValidationService;
        public UserValidateServiceTest()
        {
            _repositoryMock = Substitute.For<IUserRepository>();
            _userValidationService = new UserValidationService(_repositoryMock);
        }

        #region Insert
        [Fact]
        public async Task ValidInsert_Success_Async()
        {
            // Arrange
            var newViewFake = UserMock.NewSameEmailAdminUserViewModel();

            // Act
            var valid = await _userValidationService.ValidInsertAsync(newViewFake);

            // Assert
            Assert.True(valid.success);
            Assert.Empty(valid.errors);
        }

        [Fact]
        public async Task ValidInsert_Error_EmailAlreadyExistsAsync()
        {
            // Arrange
            var newViewFake = UserMock.NewSameEmailAdminUserViewModel();
            var listFake = UserMock.ListUser().AsQueryable();

            _repositoryMock.All.Returns(listFake);

            // Act
            var valid = await _userValidationService.ValidInsertAsync(newViewFake);

            // Assert
            Assert.False(valid.success);
            Assert.Contains(Global.EmailAlreadyRegistered, valid.errors);
        }

        [Fact]
        public async Task ValidInsert_Error_PasswordDoesNotMatchAsync()
        {
            // Arrange
            var newViewFake = UserMock.NewMasterUserViewModel();

            newViewFake.Password = "123";
            newViewFake.PasswordCheck = "321";

            // Act
            var valid = await _userValidationService.ValidInsertAsync(newViewFake);

            // Assert
            Assert.False(valid.success);
            Assert.Contains(Global.PasswordDoesNotMatch, valid.errors);
        }
        #endregion

        #region Update
        [Fact]
        public async Task ValidUpdate_Success_Async()
        {
            // Arrange
            var newViewFake = UserMock.NewSameEmailAdminUserViewModel();

            // Act
            var valid = await _userValidationService.ValidUpdateAsync(newViewFake);

            // Assert
            Assert.True(valid.success);
            Assert.Empty(valid.errors);
        }

        [Fact]
        public async Task ValidUpdate_Error_EmailAlreadyExistsAsync()
        {
            // Arrange
            var newViewFake = UserMock.NewSameEmailAdminUserViewModel();
            var listFake = UserMock.ListUser().AsQueryable();

            _repositoryMock.All.Returns(listFake);

            // Act
            var valid = await _userValidationService.ValidInsertAsync(newViewFake);

            // Assert
            Assert.False(valid.success);
            Assert.Contains(Global.EmailAlreadyRegistered, valid.errors);
        }
        #endregion
    }
}
