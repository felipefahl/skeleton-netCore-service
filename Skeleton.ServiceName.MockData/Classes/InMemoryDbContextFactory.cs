using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Skeleton.ServiceName.Data;

namespace Skeleton.ServiceName.MockData.Classes
{
    public class InMemoryDbContextFactory
    {
        public ServiceNameContext GetAuthDbContext(string databaseName, IHttpContextAccessor httpContextAccessor)
        {
            var options = new DbContextOptionsBuilder<ServiceNameContext>()
                            .UseInMemoryDatabase(databaseName: databaseName)
                            .Options;
            var dbContext = new ServiceNameContext(options, httpContextAccessor);
            dbContext.Database.EnsureCreated();
            return dbContext;
        }
    }
}
