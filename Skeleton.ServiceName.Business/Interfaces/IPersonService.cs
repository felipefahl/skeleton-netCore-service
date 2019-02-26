using Skeleton.ServiceName.ViewModel.People;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.Business.Interfaces
{
    public interface IPersonService
    {
        IList<PersonViewModel> All();
        Task<PersonViewModel> GetAsync(long id);
        Task<PersonViewModel> SaveAsync(PersonViewModel model);
        Task<bool> DeleteAsync(long id);

    }
}
