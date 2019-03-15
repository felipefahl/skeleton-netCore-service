using Skeleton.ServiceName.Data.Models;
using Skeleton.ServiceName.ViewModel.People;
namespace Skeleton.ServiceName.Business.Interfaces
{
    public interface IPersonService : IServiceCrud<Person, PersonViewModel>
    {
    }
}
