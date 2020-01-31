using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Skeleton.ServiceName.Business.Interfaces;
using Skeleton.ServiceName.Business.Interfaces.Validations;
using Skeleton.ServiceName.Data.Interfaces;
using Skeleton.ServiceName.Data.Models;
using Skeleton.ServiceName.Utils.EfExtensions;
using Skeleton.ServiceName.Utils.Exceptions;
using Skeleton.ServiceName.Utils.Helpers;
using Skeleton.ServiceName.Utils.Models;
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
        private readonly IUserValidationService _userValidationService;

        public UserService(IUserRepository repository,
                          IAccessManager accessManager,
                          IUserValidationService userValidationService,
                          IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _accessManager = accessManager;
            _userValidationService = userValidationService;
        }

        private async Task<UserViewModel> PreInsertAsync(UserViewModel model)
        {
            var hash = PasswordHelper.Hash(model.Password);
            model.Password = hash;
            model.PasswordCheck = hash;
            return await PreSaveAsync(model);
        }

        private async Task<UserViewModel> PreUpdateAsync(UserViewModel model)
        {
            return await PreSaveAsync(model);
        }

        private async Task<UserViewModel> PreSaveAsync(UserViewModel model)
        {
            return await Task.FromResult(model);
        }

        public override async Task<UserViewModel> InsertAsync(UserViewModel model)
        {
            var valid = await _userValidationService.ValidInsertAsync(model);

            if (!valid.success)
                throw new BusinessRuleException(ErrorResponse.FromBusinessRules(Global.ValidationError, valid.errors));

            return await base.InsertAsync(await PreInsertAsync(model));
        }

        public override async Task<UserViewModel> UpdateAsync(UserViewModel model)
        {
            var valid = await _userValidationService.ValidUpdateAsync(model);

            if (!valid.success)
                throw new BusinessRuleException(ErrorResponse.FromBusinessRules(Global.ValidationError, valid.errors));

            var entity = await _repository.FindNoTrackingAsync((Guid)model.Id);
            var userModel = _mapper.Map<User, UserViewModel>(entity);

            userModel.Name = model.Name;
            userModel.Email = model.Email;
            userModel.Profile = model.Profile;

            return await base.UpdateAsync(await PreUpdateAsync(userModel));
        }

        public async Task<LoginResponseViewModel> LoginAsync(LoginViewModel model)
        {

            var user = await _repository.FindByEmailAsync(model.Email);

            var response = new LoginResponseViewModel();

            if (user == null || !CheckCredentials(user.Password, model.Password))
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

        public bool CheckCredentials(string hashedPassword, string password)
        {
            return PasswordHelper.Verify(hashedPassword, password);
        }
    }
}
