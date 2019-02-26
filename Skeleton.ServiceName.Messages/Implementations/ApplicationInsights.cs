using Microsoft.ApplicationInsights;
using Newtonsoft.Json;
using Skeleton.ServiceName.Messages.Helpers;
using Skeleton.ServiceName.Messages.Interfaces;
using Skeleton.ServiceName.Messages.Models;
using Skeleton.ServiceName.Utils.Enumerators;
using Skeleton.ServiceName.Utils.Helpers;
using System.Collections.Generic;

namespace Skeleton.ServiceName.Messages.Implementations
{
    public class ApplicationInsights : IApplicationInsights
    {
        private readonly ApplicationInsightsSettings _settings;

        private readonly TelemetryClient _client;
        private readonly Dictionary<string, string> _dados;

        public ApplicationInsights(ApplicationInsightsSettings settings)
        {
            _settings = settings;

            _client = new TelemetryClient();
            _dados = new Dictionary<string, string>();
        }

        public void ChoreographyStackReceived(IList<string> stack, object obj)
        {
            ChoreographyStackSent(EChoreographyStackType.Received, stack, obj);
        }


        public void ChoreographyStackSent(IList<string> stack, object obj)
        {
            ChoreographyStackSent(EChoreographyStackType.Sent, stack, obj);
        }

        public void LogAccess(string message, string source)
        {
            Log(ELogType.Access, message, source);
        }

        public void LogAudit(string message, string source)
        {
            Log(ELogType.Audit, message, source);
        }

        public void LogError(string message, string source)
        {
            Log(ELogType.Error, message, source);
        }

        public void LogInformation(string message, string source)
        {
            Log(ELogType.Information, message, source);
        }

        public void LogWarning(string message, string source)
        {
            Log(ELogType.Warning , message, source);
        }

        private void ChoreographyStackSent(EChoreographyStackType type, IList<string> stack, object obj)
        {
            var applicationInsightsChoreographyStack = new ApplicationInsightsChoreographyStackModel() {
                Date = DateTimeHelper.BrazilNow,
                Type = type,
                Stack = stack,
                Object = obj
            };

            TrackEvent(applicationInsightsChoreographyStack, _settings.ChoreographyStackName, _settings.ChoreographyStackDetail);
        }

        private void Log(ELogType type, string message, string source)
        {
            var applicationInsightsLogEvent = new ApplicationInsightsLogEventModel() {
                Date = DateTimeHelper.BrazilNow,
                Type = type,
                Message = message,
                Source = source
            };

            TrackEvent(applicationInsightsLogEvent, _settings.EventLogName, _settings.EventLogDetail);
        }

        private void TrackEvent(object obj, string eventName, string eventDetail)
        {
            var message = JsonConvert.SerializeObject(obj);
            _dados[eventDetail] = message;
            _client.TrackEvent(eventName, _dados);
        }
    }
}
