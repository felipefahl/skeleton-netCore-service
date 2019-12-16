using Skeleton.ServiceName.Data.Interfaces;
using Skeleton.ServiceName.Data.Models;
using Skeleton.ServiceName.Utils.Helpers;
using Skeleton.ServiceName.Utils.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Skeleton.ServiceName.Data
{
    [ExcludeFromCodeCoverage]
    public class UserInitializer
    {
        private readonly ServiceNameContext _context;
        private readonly IUserRepository _userRepository;

        public UserInitializer(
            ServiceNameContext context,
            IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public void Initialize()
        {
            CreateUser(
                    new User()
                    {
                        Name = "ServiceName",
                        Email = "ServiceName@ServiceName.com",
                        Password = "ServiceName",
                        PasswordCheck = "ServiceName",
                        Profile = UserProfile.Master,
                        UserCreated = "ServiceName.Auth",
                        DateCreated = DateTime.UtcNow
                    });
        }

        private void CreateUser(User user)
        {
            if (_userRepository.FindByEmailAsync(user.Email).Result == null)
            {
                var hash = PasswordHelper.Hash(user.Password);
                user.Password = hash;
                user.PasswordCheck = hash;
                _userRepository.InsertAsync(user);
            }
        }
    }
}
