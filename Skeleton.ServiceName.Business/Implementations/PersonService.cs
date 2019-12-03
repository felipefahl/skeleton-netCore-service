using AutoMapper;
using Skeleton.ServiceName.Business.Interfaces;
using Skeleton.ServiceName.Data.Interfaces;
using Skeleton.ServiceName.Data.Models;
using Skeleton.ServiceName.ViewModel.People;

namespace Skeleton.ServiceName.Business.Implementations
{
    public class PersonService : ServiceCrud<Person, PersonViewModel>, IPersonService
    {
        public PersonService(IPersonRepository repository,
                             IMapper mapper)
            :base(repository, mapper)
        {

        }
    }
}
