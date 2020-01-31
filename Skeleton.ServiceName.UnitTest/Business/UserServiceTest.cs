using AutoMapper;
using Newtonsoft.Json;
using NSubstitute;
using Skeleton.ServiceName.Business.Implementations;
using Skeleton.ServiceName.Business.Interfaces.Validations;
using Skeleton.ServiceName.Business.Parameters;
using Skeleton.ServiceName.Business.Profiles;
using Skeleton.ServiceName.Data.Interfaces;
using Skeleton.ServiceName.Data.Models;
using Skeleton.ServiceName.MockData.Classes;
using Skeleton.ServiceName.Utils.Exceptions;
using Skeleton.ServiceName.Utils.Helpers;
using Skeleton.ServiceName.Utils.Models;
using Skeleton.ServiceName.Utils.Resources;
using Skeleton.ServiceName.Utils.Security;
using Skeleton.ServiceName.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Skeleton.ServiceName.UnitTest.Business
{
    public class UserServiceTest
    {
        private readonly IUserRepository _repositoryMock;
        private readonly IAccessManager _accessManagerMock;
        private readonly IUserValidationService _userValidationServiceMock;

        private readonly UserService _userService;

        public UserServiceTest()
        {
            var myProfile = new AutoMapperDomainProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            _repositoryMock = Substitute.For<IUserRepository>();
            _accessManagerMock = Substitute.For<IAccessManager>();
            _userValidationServiceMock = Substitute.For<IUserValidationService>();
            _userService = new UserService(_repositoryMock, _accessManagerMock, _userValidationServiceMock, mapper);
        }

        #region Get

        [Fact]
        public void User_AllAsync()
        {
            // Arrange
            var parameters = new UserParameters();
            var listFake = UserMock.ListUser().AsQueryable();
            var listVewFake = UserMock.ListUserViewModel();
            _repositoryMock.All.Returns(listFake);

            // Act
            var list = _userService.All(parameters);

            // Assert
            Assert.Equal(listVewFake.Count, list.Count);
            Assert.IsType<List<UserViewModel>>(list);
        }

        [Fact]
        public async Task User_GetAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var fake = UserMock.GetMasterUser(id);

            _repositoryMock.FindAsync(id).Returns(fake);

            // Act
            var user = await _userService.GetAsync(id);

            // Assert
            Assert.Equal(id, user.Id);
            Assert.IsType<UserViewModel>(user);
        }
        #endregion

        #region Insert
        //Insert
        [Fact]
        public async Task User_Insert_SuccessAsync()
        {
            // Arrange
            var newViewFake = UserMock.NewMasterUserViewModel();
            _userValidationServiceMock.ValidInsertAsync(newViewFake).Returns((true, new List<string>()));

            // Act
            var user = await _userService.InsertAsync(newViewFake);

            // Assert
            await _repositoryMock.Received().InsertAsync(Arg.Any<User>());
            Assert.IsType<UserViewModel>(user);
        }        

        [Fact]
        public async Task User_Insert_Error_PasswordDoesNotMatchAsync()
        {
            // Arrange
            var newViewFake = UserMock.NewMasterUserViewModel();

            _userValidationServiceMock.ValidInsertAsync(newViewFake).Returns((false, new List<string> { Global.ValidationError }));

            // Act

            // Assert
            try
            {
                await _userService.InsertAsync(newViewFake);
            }
            catch (BusinessRuleException ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorResponse>(ex.Message);
                Assert.Equal(Global.ValidationError, error.Message);
            }
            await _repositoryMock.DidNotReceive().InsertAsync(Arg.Any<User>());
        }

        #endregion

        #region Update
        [Fact]
        public async Task User_Update_SuccessAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var fake = UserMock.GetMasterUser(id);
            var viewFake = UserMock.GetMasterUserViewModel(id);
            _userValidationServiceMock.ValidUpdateAsync(viewFake).Returns((true, new List<string>()));

            viewFake.Name = "Alterou";

            _repositoryMock.FindNoTrackingAsync(id).Returns(fake);

            // Act
            var user = await _userService.UpdateAsync(viewFake);

            // Assert
            Assert.Equal(id, user.Id);
            Assert.Equal(viewFake.Name, user.Name);
            Assert.IsType<UserViewModel>(user);
        }

        [Fact]
        public async Task User_Update_Error_ValidationAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var newFake = UserMock.GetMasterUser(id);
            var newViewFake = UserMock.GetMasterUserViewModel(id);

            _userValidationServiceMock.ValidUpdateAsync(newViewFake).Returns((false, new List<string> { Global.ValidationError }));
            _repositoryMock.FindNoTrackingAsync(id).Returns(newFake);

            // Act

            // Assert
            try
            {
                await _userService.UpdateAsync(newViewFake);
            }
            catch (BusinessRuleException ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorResponse>(ex.Message);
                Assert.Equal(Global.ValidationError, error.Message);
            }

            await _repositoryMock.DidNotReceive().UpdateAsync(newFake);
        }
        #endregion

        #region Delete
        [Fact]
        public async Task User_Delete_SuccessAsync()
        {
            // Arrange
            var count = 0;
            var id = Guid.NewGuid();
            var fake = UserMock.GetMasterUser(id);
            _repositoryMock.FindAsync(id).Returns(fake);
            _repositoryMock.When(x => x.DeleteAsync(fake)).Do(x => count++);

            // Act
            var done = await _userService.DeleteAsync(id);

            // Assert
            Assert.True(done);
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task User_Delete_NotFoundAsync()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var done = await _userService.DeleteAsync(id);

            // Assert
            Assert.False(done);
            await _repositoryMock.DidNotReceive().DeleteAsync(Arg.Any<User>());
        }
        #endregion

        #region Login
        [Fact]
        public async Task User_Login_NotMaster_SuccessAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var fake = UserMock.GetAdminUser(id);
            var viewFake = UserMock.NewUserLoginViewModel();

            fake.Password = PasswordHelper.Hash(viewFake.Password);
            fake.PasswordCheck = PasswordHelper.Hash(viewFake.Password);

            _accessManagerMock.GenerateToken(Arg.Any<SecurityUserModel>()).Returns("sfasfasfasfasfasfaffafasasfasf");
            _repositoryMock.FindByEmailAsync(fake.Email).Returns(fake);

            // Act
            var response = await _userService.LoginAsync(viewFake);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(viewFake.Email, response.Email);
            Assert.NotEqual(UserProfile.Master, response.Profile);
            Assert.NotEmpty(response.Token);
            Assert.NotNull(response.Token);
        }

        [Fact]
        public async Task User_Login_Master_SuccessAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var fake = UserMock.GetMasterUser(id);
            var viewFake = UserMock.NewUserLoginViewModel();

            fake.Password = PasswordHelper.Hash(viewFake.Password);
            fake.PasswordCheck = PasswordHelper.Hash(viewFake.Password);


            _accessManagerMock.GenerateToken(Arg.Any<SecurityUserModel>()).Returns("sfasfasfasfasfasfaffafasasfasf");
            _repositoryMock.FindByEmailAsync(fake.Email).Returns(fake);

            // Act
            var response = await _userService.LoginAsync(viewFake);

            // Assert
            Assert.True(response.Success);
            Assert.Equal(viewFake.Email, response.Email);
            Assert.Equal(UserProfile.Master, response.Profile);
            Assert.NotEmpty(response.Token);
            Assert.NotNull(response.Token);
        }

        [Fact]
        public async Task User_Login_ErrorAsync()
        {
            // Arrange
            var partnerId = Guid.NewGuid();
            var viewFake = UserMock.NewUserLoginViewModel();


            // Act
            var response = await _userService.LoginAsync(viewFake);

            // Assert
            Assert.False(response.Success);
            Assert.Equal(Global.EmailAndPasswordInvalid, response.Message);
            Assert.Null(response.Token);
        }
        #endregion
    }
}
