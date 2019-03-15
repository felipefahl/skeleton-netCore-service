using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using Skeleton.ServiceName.Business.Interfaces;
using Skeleton.ServiceName.Data;
using Skeleton.ServiceName.Data.Models;
using Skeleton.ServiceName.Messages.Interfaces;
using Skeleton.ServiceName.Messages.Models;
using Skeleton.ServiceName.Utils.Enumerators;
using Skeleton.ServiceName.Utils.Helpers;
using Skeleton.ServiceName.ViewModel.People;

[assembly: InternalsVisibleTo(assemblyName: "Skeleton.ServiceName.UnitTest")]
namespace Skeleton.ServiceName.Business.Implementations
{
    internal class PersonService : ServiceCrud<Person, PersonViewModel>, IPersonService
    {
        public PersonService(IRepository<Person> person,
                             IMapper mapper,
                             IServiceBus serviceBus,
                             IApplicationInsights applicationInsights)
            :base(person, mapper, serviceBus, applicationInsights)
        {

        }
    }
}
