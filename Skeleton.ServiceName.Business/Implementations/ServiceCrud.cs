using AutoMapper;
using Skeleton.ServiceName.Business.Interfaces;
using Skeleton.ServiceName.Data;
using Skeleton.ServiceName.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.Business.Implementations
{
    internal abstract class ServiceCrud<TEntity, TEntityViewModel> : ServiceBase, IServiceCrud<TEntity, TEntityViewModel> where TEntity : class
                                                                                                                          where TEntityViewModel : class
    {
        protected readonly IRepository<TEntity> _repository;

        protected ServiceCrud(IRepository<TEntity> repository,
                           IMapper mapper,
                           IServiceBus serviceBus,
                           IApplicationInsights applicationInsights)
            :base(mapper, serviceBus, applicationInsights)
        {
            _repository = repository;
        }

        public IList<TEntityViewModel> All()
        {
            var list = _mapper.Map<IEnumerable<TEntity>, IList<TEntityViewModel>>(_repository.All);

            return list;
        }

        public async Task<TEntityViewModel> InsertAsync(TEntityViewModel model)
        {
            var entity = _mapper.Map<TEntity>(model);

            await _repository.InsertAsync(entity);

            return _mapper.Map<TEntityViewModel>(entity);
        }

        public async Task<TEntityViewModel> UpdateAsync(TEntityViewModel model)
        {
            var entity = _mapper.Map<TEntity>(model);

            await _repository.UpdateAsync(entity);

            return _mapper.Map<TEntityViewModel>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _repository.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            await _repository.DeleteAsync(entity);

            return true;
        }

        public async Task<TEntityViewModel> GetAsync(long id)
        {
            var entity = await _repository.FindAsync(id);
            return _mapper.Map<TEntity, TEntityViewModel>(entity);
        }
    }
}
