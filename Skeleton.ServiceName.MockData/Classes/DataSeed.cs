using Skeleton.ServiceName.Data;

namespace Skeleton.ServiceName.MockData.Classes
{
    public static class DataSeed
    {
        public static void Seed(ServiceNameContext _dbContext)
        {
            var users = UserMock.ListUser();
            var people = PersonMock.ListPerson();

            _dbContext.Users.AddRange(users);
            _dbContext.People.AddRange(people);

            _dbContext.SaveChanges();
        }
    }
}
