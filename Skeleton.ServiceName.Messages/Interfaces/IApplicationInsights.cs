using System.Collections.Generic;

namespace Skeleton.ServiceName.Messages.Interfaces
{
    public interface IApplicationInsights
    {
        void LogError(string message, string source);
        void LogWarning(string message, string source);
        void LogInformation(string message, string source);
        void LogAudit(string message, string source);
        void LogAccess(string message, string source);

        void ChoreographyStackSent(IList<string> stack, object obj);
        void ChoreographyStackReceived(IList<string> stack, object obj);
    }
}
