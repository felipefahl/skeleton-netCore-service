using AutoMapper;
using Skeleton.ServiceName.Business.Interfaces;
using Skeleton.ServiceName.Data;
using Skeleton.ServiceName.Data.Models;
using Skeleton.ServiceName.Utils.EfExtensions;
using Skeleton.ServiceName.Utils.Models;
using Skeleton.ServiceName.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.Business.Implementations
{
    public abstract class ServiceCrud<TEntity, TEntityViewModel> : ServiceBase, IServiceCrud<TEntity, TEntityViewModel> where TEntity : BaseEntity
                                                                                                                          where TEntityViewModel : BaseViewModel
    {
        protected readonly IRepository<TEntity> _repository;

        protected ServiceCrud(IRepository<TEntity> repository,
                           IMapper mapper)
            :base(mapper)
        {
            _repository = repository;
        }

        public virtual IList<TEntityViewModel> All(QueryStringParameters queryStringParameters)
        {
            var query = _repository.All.Paged(queryStringParameters.PageNumber, queryStringParameters.PageSize);
            var list = _mapper.Map<IEnumerable<TEntity>, IList<TEntityViewModel>>(query);

            return list;
        }

        public virtual async Task<TEntityViewModel> InsertAsync(TEntityViewModel model)
        {
            var entity = _mapper.Map<TEntity>(model);

            await _repository.InsertAsync(entity);

            return _mapper.Map<TEntityViewModel>(entity);
        }

        public virtual async Task<TEntityViewModel> UpdateAsync(TEntityViewModel model)
        {
            var oldEntity = await _repository.FindNoTrackingAsync((Guid)model.Id);

            if (oldEntity == null)
            {
                return null;
            }


            var entity = _mapper.Map<TEntity>(model);

            entity.DateCreated = oldEntity.DateCreated;
            entity.UserCreated = oldEntity.UserCreated;

            await _repository.UpdateAsync(entity);

            return _mapper.Map<TEntityViewModel>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repository.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            await _repository.DeleteAsync(entity);

            return true;
        }

        public async Task<TEntityViewModel> GetAsync(Guid id)
        {
            var entity = await _repository.FindAsync(id);
            return _mapper.Map<TEntity, TEntityViewModel>(entity);
        }

        public async Task<long> CountAsync()
        {
            return await _repository.CountAsync();
        }
    }
}
