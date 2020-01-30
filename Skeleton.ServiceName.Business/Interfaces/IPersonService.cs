using Skeleton.ServiceName.Business.Parameters;
using Skeleton.ServiceName.Data.Models;
using Skeleton.ServiceName.ViewModel.People;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.Business.Interfaces
{
    public interface IPersonService : IServiceCrud<Person, PersonViewModel>
    {
        IList<PersonViewModel> All(PersonParameters queryStringParameters);
        Task<long> CountAsync(PersonParameters queryStringParameters);
    }
}
