using AutoMapper;
using Skeleton.ServiceName.Business.Interfaces;
using Skeleton.ServiceName.Messages.Interfaces;

namespace Skeleton.ServiceName.Business.Implementations
{
    internal abstract class ServiceBase : IServiceBase
    {
        protected readonly IMapper _mapper;
        protected readonly IServiceBus _serviceBus;
        protected readonly IApplicationInsights _applicationInsights;

        protected ServiceBase(IMapper mapper,
                           IServiceBus serviceBus,
                           IApplicationInsights applicationInsights)
        {
            _mapper = mapper;
            _serviceBus = serviceBus;
            _applicationInsights = applicationInsights;
        }
    }
}
