using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.Business.Interfaces
{
    public interface IServiceCrud<TEntity, TEntityViewModel> where TEntity : class 
                                                             where TEntityViewModel : class
    {
        IList<TEntityViewModel> All();
        Task<TEntityViewModel> GetAsync(long id);
        Task<TEntityViewModel> InsertAsync(TEntityViewModel model);
        Task<TEntityViewModel> UpdateAsync(TEntityViewModel model);
        Task<bool> DeleteAsync(long id);
    }
}
