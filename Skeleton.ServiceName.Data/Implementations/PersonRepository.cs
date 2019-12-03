using Skeleton.ServiceName.Data.Interfaces;
using Skeleton.ServiceName.Data.Models;

namespace Skeleton.ServiceName.Data.Implementations
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(ServiceNameContext context) : base(context)
        {
        }
    }
}
