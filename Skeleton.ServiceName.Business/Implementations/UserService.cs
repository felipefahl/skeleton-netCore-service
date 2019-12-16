using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Skeleton.ServiceName.Business.Interfaces;
using Skeleton.ServiceName.Data.Interfaces;
using Skeleton.ServiceName.Data.Models;
using Skeleton.ServiceName.Utils.Helpers;
using Skeleton.ServiceName.Utils.Resources;
using Skeleton.ServiceName.Utils.Security;
using Skeleton.ServiceName.ViewModel.Authentication;
using Skeleton.ServiceName.ViewModel.User;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.Business.Implementations
{
    public class UserService : ServiceCrud<User, UserViewModel>, IUserService
    {
        private new readonly IUserRepository _repository;

        private readonly IAccessManager _accessManager;
        public UserService(IUserRepository repository,
                          IAccessManager accessManager, 
                          IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _accessManager = accessManager;
        }

        private async Task<UserViewModel> PreInsertAsync(UserViewModel model)
        {
            if (!PasswordHelper.Compare(model.Password, model.PasswordCheck))
                throw new Exception(Global.PasswordDoesNotMatch);

            var hash = PasswordHelper.Hash(model.Password);

            model.Password = hash;
            model.PasswordCheck = hash;

            return await PreSaveAsync(model);
        }

        private async Task<UserViewModel> PreSaveAsync(UserViewModel model)
        {
            await Valid(model);

            return model;
        }

        private async Task Valid(UserViewModel model)
        {

            var user = await _repository
                .All
                .Where(x => x.Email == model.Email)
                .Where(x => x.Id != model.Id)
                .ToListAsync();

            if (user.Any())
                throw new Exception(Global.EmailAlreadyRegistered);
        }

        public override async Task<UserViewModel> InsertAsync(UserViewModel model)
        {
            return await base.InsertAsync(await PreInsertAsync(model));
        }

        public override async Task<UserViewModel> UpdateAsync(UserViewModel model)
        {
            var entity = await _repository.FindNoTrackingAsync((Guid)model.Id);
            var userModel = _mapper.Map<User, UserViewModel>(entity);

            userModel.Name = model.Name;
            userModel.Email = model.Email;
            userModel.Profile = model.Profile;

            return await base.UpdateAsync(await PreSaveAsync(userModel));
        }

        public async Task<LoginResponseViewModel> LoginAsync(LoginViewModel model)
        {

            var user = await _repository.FindByEmailAsync(model.Email);

            var response = new LoginResponseViewModel();

            if (user == null || !ValidateCredentials(user.Password, model.Password))
            {
                response.Success = false;
                response.Message = Global.EmailAndPasswordInvalid;
                return response;
            }
            var securityUserModel = _mapper.Map<SecurityUserModel>(user);

            response.Success = true;
            response.UserId = user.Id;
            response.Name = user.Name;
            response.Email = user.Email;
            response.Profile = user.Profile;
            response.Active = user.Active;
            response.Token = _accessManager.GenerateToken(securityUserModel);

            return response;
        }

        public bool ValidateCredentials(string hashedPassword, string password)
        {
            return PasswordHelper.Verify(hashedPassword, password);
        }
    }
}
