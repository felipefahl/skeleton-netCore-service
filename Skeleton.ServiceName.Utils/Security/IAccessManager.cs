using System;

namespace Skeleton.ServiceName.Utils.Security
{
    public interface IAccessManager
    {
        string GenerateToken(SecurityUserModel model);
        bool ValidateToken(string authToken);
    }
}
