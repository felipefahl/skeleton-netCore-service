using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Skeleton.ServiceName.Messages.Helpers;
using Skeleton.ServiceName.Messages.Interfaces;
using Skeleton.ServiceName.Messages.Models;
using System.Text;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.Messages.Implementations
{
    public class ServiceBus: IServiceBus
    {
        private static QueueClient _queueClient;
        private readonly IApplicationInsights _applicationInsights;

        private const string _stack = "ServiceBus";
        private const string _businessMethod = "PersonBusReceiverService";

        public ServiceBus(ServiceBusSettings serviceBusSettings, 
                            IApplicationInsights applicationInsights)
        {
            _queueClient = new QueueClient(serviceBusSettings.ConnectionString, serviceBusSettings.QueueName);
            _applicationInsights = applicationInsights;

            //RegisterOnMessageHandlerAndReceiveMessages();
        }

        //private void RegisterOnMessageHandlerAndReceiveMessages()
        //{
        //    _queueClient.RegisterMessageHandler(
        //          async (message, token) =>
        //          {
        //              await ProcessMessagesAsync(message, token);
        //          },
        //         new MessageHandlerOptions(ExceptionReceivedHandler) { MaxConcurrentCalls = 10, AutoComplete = false });
        //}

        //private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        //{
        //    var body = Encoding.UTF8.GetString(message.Body);
        //    // Process the message.
        //    Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{body}");

        //    //
        //    ApplicationInsightsChoreographyStackModel choreography = JsonConvert.DeserializeObject<ApplicationInsightsChoreographyStackModel>(body);

        //    var stackList = choreography.Stack;
        //    var person = JsonConvert.DeserializeObject<ApplicationInsightsChoreographyStackModel>(body);

        //    stackList.Add(_stack);
        //    _applicationInsights.ChoreographyStackReceived(stackList, person);

        //    //
        //    // Complete the message so that it is not received again.
        //    // This can be done only if the queue Client is created in ReceiveMode.PeekLock mode (which is the default).
        //    await _queueClient.CompleteAsync(message.SystemProperties.LockToken);

        //    // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
        //    // If queueClient has already been closed, you can choose to not call CompleteAsync() or AbandonAsync() etc.
        //    // to avoid unnecessary exceptions.
        //}

        //private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        //{
        //    Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
        //    var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
        //    Console.WriteLine("Exception context for troubleshooting:");
        //    Console.WriteLine($"- Endpoint: {context.Endpoint}");
        //    Console.WriteLine($"- Entity Path: {context.EntityPath}");
        //    Console.WriteLine($"- Executing Action: {context.Action}");
        //    return Task.CompletedTask;
        //}

        public async Task SendAsync(ServiceBusModel serviceBusModel)
        {

            var message = JsonConvert.SerializeObject(serviceBusModel);
            var content = Encoding.UTF8.GetBytes(message);

            await _queueClient.SendAsync(new Message(content));
        }
    }
}
