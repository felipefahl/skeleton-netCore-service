using AutoMapper;
using Skeleton.ServiceName.Business.Interfaces;

namespace Skeleton.ServiceName.Business.Implementations
{
    public abstract class ServiceBase : IServiceBase
    {
        protected readonly IMapper _mapper;

        protected ServiceBase(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
