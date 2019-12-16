using Skeleton.ServiceName.Data.Models;
using Skeleton.ServiceName.ViewModel.Authentication;
using Skeleton.ServiceName.ViewModel.User;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.Business.Interfaces
{
    public interface IUserService : IServiceCrud<User, UserViewModel>
    {
        Task<LoginResponseViewModel> LoginAsync(LoginViewModel model);
    }
}
