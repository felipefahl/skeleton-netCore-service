using Skeleton.ServiceName.Data.Models;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.Data.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> FindByEmailAsync(string email);
    }
}
