using Skeleton.ServiceName.ViewModel.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.Business.Interfaces.Validations
{
    public interface IUserValidationService
    {
        public Task<(bool success, IList<string> errors)> ValidInsertAsync(UserViewModel model);
        public Task<(bool success, IList<string> errors)> ValidUpdateAsync(UserViewModel model);
    }
}
