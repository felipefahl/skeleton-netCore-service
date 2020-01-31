using Skeleton.ServiceName.Business.Interfaces.Validations;
using Skeleton.ServiceName.Data.Interfaces;
using Skeleton.ServiceName.Utils.EfExtensions;
using Skeleton.ServiceName.Utils.Helpers;
using Skeleton.ServiceName.Utils.Resources;
using Skeleton.ServiceName.ViewModel.User;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.Business.Implementations.Validations
{
    public class UserValidationService : IUserValidationService
    {
        private readonly IUserRepository _repository;

        public UserValidationService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<(bool success, IList<string> errors)> ValidInsertAsync(UserViewModel model)
        {
            var errors = new List<string>();

            if (!PasswordHelper.Compare(model.Password, model.PasswordCheck)) 
                errors.Add(Global.PasswordDoesNotMatch);

            var validChange = await ValidChangeAsync(model);
            errors.AddRange(validChange.errors);

            return (!errors.Any(), errors);
        }

        public async Task<(bool success, IList<string> errors)> ValidUpdateAsync(UserViewModel model)
        {
            var errors = new List<string>();

            var validChange = await ValidChangeAsync(model);
            errors.AddRange(validChange.errors);

            return (!errors.Any(), errors);
        }

        private async Task<(bool success, IList<string> errors)> ValidChangeAsync(UserViewModel model)
        {
            var errors = new List<string>();

            var user = await _repository
                .All
                .Where(x => x.Email == model.Email)
                .Where(x => x.Id != model.Id)
                .ToListAsyncSafe();

            if (user.Any())
                errors.Add(Global.EmailAlreadyRegistered);

            return (!errors.Any(), errors);
        }
    }
}
