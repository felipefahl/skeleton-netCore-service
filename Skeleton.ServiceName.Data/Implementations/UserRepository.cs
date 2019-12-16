using Microsoft.EntityFrameworkCore;
using Skeleton.ServiceName.Data.Interfaces;
using Skeleton.ServiceName.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.Data.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ServiceNameContext context) : base(context)
        {
        }


        public async Task<User> FindByEmailAsync(string email)
        {
            return await _context.Set<User>().Where(x => x.Email == email).FirstOrDefaultAsync();
        }
    }
}
