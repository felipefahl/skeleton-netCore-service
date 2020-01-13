using Skeleton.ServiceName.Data.Models;
using Skeleton.ServiceName.Utils.Helpers;
using Skeleton.ServiceName.Utils.Models;
using Skeleton.ServiceName.Utils.Security;
using Skeleton.ServiceName.ViewModel.Authentication;
using Skeleton.ServiceName.ViewModel.User;
using System;
using System.Collections.Generic;

namespace Skeleton.ServiceName.MockData.Classes
{
    public static class UserMock
    {
        public static IList<User> ListUser() => new List<User> {
            new User {
                Id = Guid.Parse("93321b82-f7c7-417e-8a92-540c182b67ac"),
                Name = "Teste",
                Email = "teste@teste.com",
                Active = true,
                Password = PasswordHelper.Hash("senha123321"),
                PasswordCheck = PasswordHelper.Hash("senha123321"),
                Profile = UserProfile.Master
            },
            new User {
                Id = Guid.Parse("422c975c-3cb6-4b62-b835-c8f6533b31bc"),
                Name = "Teste 2",
                Email = "teste2@teste.com",
                Active = true,
                Password = PasswordHelper.Hash("senha123321"),
                PasswordCheck = PasswordHelper.Hash("senha123321"),
                Profile = UserProfile.Staff
            }
        };

        public static User GetMasterUser(Guid id) => new User
        {
            Id = id,
            Name = "Teste",
            Email = "teste@teste.com",
            Active = true,
            Password = PasswordHelper.Hash("senha123321"),
            PasswordCheck = PasswordHelper.Hash("senha123321"),
            Profile = UserProfile.Master
        };

        public static User GetAdminUser(Guid id) => new User
        {
            Id = id,
            Name = "Teste",
            Email = "teste@teste.com",
            Active = true,
            Password = PasswordHelper.Hash("senha123321"),
            PasswordCheck = PasswordHelper.Hash("senha123321"),
            Profile = UserProfile.Staff
        };

        public static SecurityUserModel MapSecurityUserModel(User user) => new SecurityUserModel
        {
            Email = user.Email,
            Id = user.Id,
            Name = user.Name,
            Profile = user.Profile
        };

        public static User NewMasterUser() => new User
        {
            Name = "Teste",
            Email = "teste@teste.com",
            Active = true,
            Password = PasswordHelper.Hash("senha123321"),
            PasswordCheck = PasswordHelper.Hash("senha123321"),
            Profile = UserProfile.Master
        };

        public static IList<UserViewModel> ListUserViewModel() => new List<UserViewModel> {
            new UserViewModel {
                Id = Guid.Parse("93321b82-f7c7-417e-8a92-540c182b67ac"),
                Name = "Teste",
                Email = "teste@teste.com",
                Active = true,
                Password = "senha123321",
                PasswordCheck = "senha123321",
                Profile = UserProfile.Master
            },
            new UserViewModel {
                Id = Guid.Parse("422c975c-3cb6-4b62-b835-c8f6533b31bc"),
                Name = "Teste",
                Email = "teste@teste.com",
                Active = true,
                Password = "senha123321",
                PasswordCheck = "senha123321",
                Profile = UserProfile.Staff
            }
        };
        public static UserViewModel GetMasterUserViewModel(Guid id) => new UserViewModel
        {
            Id = id,
            Name = "Teste",
            Email = "teste@teste.com",
            Active = true,
            Password = "senha123321",
            PasswordCheck = "senha123321",
            Profile = UserProfile.Master
        };

        public static UserViewModel GetAdminUserViewModel(Guid id) => new UserViewModel
        {
            Id = id,
            Name = "Teste 2",
            Email = "teste2@teste.com",
            Active = true,
            Password = "senha123321",
            PasswordCheck = "senha123321",
            Profile = UserProfile.Staff
        };

        public static UserViewModel NewMasterUserViewModel() => new UserViewModel
        {
            Name = "Teste Master",
            Email = "testeNew@teste.com",
            Active = true,
            Password = "senha123321",
            PasswordCheck = "senha123321",
            Profile = UserProfile.Master
        };

        public static UserViewModel NewSameEmailAdminUserViewModel() => new UserViewModel
        {
            Name = "Teste Admin",
            Email = "teste2@teste.com",
            Active = true,
            Password = "senha123321",
            PasswordCheck = "senha123321",
            Profile = UserProfile.Staff
        };

        public static LoginViewModel NewUserLoginViewModel() => new LoginViewModel
        {
            Email = "teste@teste.com",
            Password = "senha123321"
        };

        public static LoginViewModel NewNotMasterUserLoginViewModel() => new LoginViewModel
        {
            Email = "teste2@teste.com",
            Password = "senha123321"
        };
    }
}
