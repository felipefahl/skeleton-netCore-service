using Skeleton.ServiceName.Data;
using Skeleton.ServiceName.ViewModel.People;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.Business.Interfaces
{
    public interface IPersonService : IServiceCrud<Person, PersonViewModel>
    {
        Task<PersonViewModel> SaveAsync(PersonViewModel model);
    }
}
