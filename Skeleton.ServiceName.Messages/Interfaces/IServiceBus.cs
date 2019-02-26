using Skeleton.ServiceName.Messages.Models;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.Messages.Interfaces
{
    public interface IServiceBus
    {
        Task SendAsync(ServiceBusModel serviceBusModel);
    }
}
