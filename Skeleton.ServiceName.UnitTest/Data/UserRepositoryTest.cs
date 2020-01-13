using Microsoft.AspNetCore.Http;
using Skeleton.ServiceName.Data;
using Skeleton.ServiceName.Data.Implementations;
using Skeleton.ServiceName.Data.Interfaces;
using Skeleton.ServiceName.Data.Models;
using Skeleton.ServiceName.MockData.Classes;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Skeleton.ServiceName.UnitTest.Data
{
    public class UserRepositoryTest
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ServiceNameContext _dbContext;
        private IUserRepository _repository;

        public UserRepositoryTest()
        {
            _httpContextAccessor = new HttpContextAccessor();
        }

        [Fact]
        public void User_All()
        {
            // Arrange
            var listUsers = UserMock.ListUser().OrderBy(x => x.Id).ToList();
            _dbContext = new InMemoryDbContextFactory().GetAuthDbContext("User_All", _httpContextAccessor);
            _repository = new UserRepository(_dbContext);
            DataSeed.Seed(_dbContext);


            // Act
            var users = _repository.All;

            // Assert
            Assert.Equal(listUsers.Count, users.Count());
            Assert.IsAssignableFrom<IQueryable<User>>(users);
        }

        [Fact]
        public void User_Update()
        {
            // Arrange
            var id = Guid.NewGuid();
            var fakeUser = UserMock.GetMasterUser(id);
            var newName = "new Name";

            _dbContext = new InMemoryDbContextFactory().GetAuthDbContext("User_Update", _httpContextAccessor);
            _repository = new UserRepository(_dbContext);
            DataSeed.Seed(_dbContext);

            _dbContext.Users.AddRange(fakeUser);
            _dbContext.SaveChanges();

            fakeUser.Name = newName;

            // Act
            _repository.Update(fakeUser);
            var user = _repository.Find(id);

            // Assert
            Assert.Equal(newName, user.Name);
            Assert.Equal(id, fakeUser.Id);
        }

        [Fact]
        public void User_Delete()
        {
            // Arrange
            var id = Guid.NewGuid();
            var fakeUser = UserMock.GetMasterUser(id);

            _dbContext = new InMemoryDbContextFactory().GetAuthDbContext("User_Delete", _httpContextAccessor);
            _repository = new UserRepository(_dbContext);
            DataSeed.Seed(_dbContext);
            _dbContext.Users.AddRange(fakeUser);
            _dbContext.SaveChanges();

            // Act
            _repository.Delete(fakeUser);
            var user = _repository.Find(id);

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public void User_Get()
        {
            // Arrange
            var id = Guid.NewGuid();
            var fakeUser = UserMock.GetMasterUser(id);
            _dbContext = new InMemoryDbContextFactory().GetAuthDbContext("User_Get", _httpContextAccessor);
            _repository = new UserRepository(_dbContext);
            DataSeed.Seed(_dbContext);
            _dbContext.Users.AddRange(fakeUser);
            _dbContext.SaveChanges();


            // Act
            var user = _repository.Find(id);

            // Assert
            Assert.Equal(fakeUser, user);
            Assert.Equal(id, fakeUser.Id);
        }

        [Fact]
        public void User_Insert()
        {
            // Arrange
            var fakeUser = UserMock.NewMasterUser();

            _dbContext = new InMemoryDbContextFactory().GetAuthDbContext("User_Insert", _httpContextAccessor);
            _repository = new UserRepository(_dbContext);
            DataSeed.Seed(_dbContext);

            // Act
            _repository.Insert(fakeUser);
            var user = _repository.Find(fakeUser.Id);

            // Assert
            Assert.Equal(fakeUser.Name, user.Name);
            Assert.Equal(fakeUser.Id, fakeUser.Id);
            Assert.NotNull(user);
        }

        [Fact]
        public async Task User_GetAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var fakeUser = UserMock.GetMasterUser(id);
            _dbContext = new InMemoryDbContextFactory().GetAuthDbContext("User_GetAsync", _httpContextAccessor);
            _repository = new UserRepository(_dbContext);
            DataSeed.Seed(_dbContext);
            _dbContext.Users.AddRange(fakeUser);
            _dbContext.SaveChanges();


            // Act
            var user = await _repository.FindAsync(id);

            // Assert
            Assert.Equal(fakeUser, user);
            Assert.Equal(id, fakeUser.Id);
        }



        [Fact]
        public async Task User_UpdateAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var fakeUser = UserMock.GetMasterUser(id);
            var newName = "new Name";

            _dbContext = new InMemoryDbContextFactory().GetAuthDbContext("User_UpdateAsync", _httpContextAccessor);
            _repository = new UserRepository(_dbContext);
            DataSeed.Seed(_dbContext);
            _dbContext.Users.AddRange(fakeUser);
            _dbContext.SaveChanges();

            fakeUser.Name = newName;

            // Act
            await _repository.UpdateAsync(fakeUser);
            var user = await _repository.FindAsync(id);

            // Assert
            Assert.Equal(newName, user.Name);
            Assert.Equal(id, fakeUser.Id);
        }

        [Fact]
        public async Task User_DeleteAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var fakeUser = UserMock.GetMasterUser(id);

            _dbContext = new InMemoryDbContextFactory().GetAuthDbContext("User_DeleteAsync", _httpContextAccessor);
            _repository = new UserRepository(_dbContext);
            DataSeed.Seed(_dbContext);
            _dbContext.Users.AddRange(fakeUser);
            _dbContext.SaveChanges();

            // Act
            await _repository.DeleteAsync(fakeUser);
            var user = await _repository.FindAsync(id);

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public async Task User_InsertAsync()
        {
            // Arrange
            var fakeUser = UserMock.NewMasterUser();

            _dbContext = new InMemoryDbContextFactory().GetAuthDbContext("User_InsertAsync", _httpContextAccessor);
            _repository = new UserRepository(_dbContext);
            DataSeed.Seed(_dbContext);

            // Act
            await _repository.InsertAsync(fakeUser);
            var user = await _repository.FindAsync(fakeUser.Id);

            // Assert
            Assert.Equal(fakeUser.Name, user.Name);
            Assert.Equal(fakeUser.Id, fakeUser.Id);
            Assert.NotNull(user);
        }

        [Fact]
        public async Task User_FindByEmailAsync()
        {
            // Arrange
            var fakeUser = UserMock.ListUser().FirstOrDefault();
            _dbContext = new InMemoryDbContextFactory().GetAuthDbContext("User_FindByEmailAsync", _httpContextAccessor);
            _repository = new UserRepository(_dbContext);
            DataSeed.Seed(_dbContext);

            // Act
            var user = await _repository.FindByEmailAsync(fakeUser.Email);

            // Assert
            Assert.Equal(fakeUser.Id, user.Id);
        }

        [Fact]
        public async Task User_FindByEmailAsync_NULLAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            _dbContext = new InMemoryDbContextFactory().GetAuthDbContext("User_FindByEmailAsync_NULLAsync", _httpContextAccessor);
            _repository = new UserRepository(_dbContext);
            DataSeed.Seed(_dbContext);
            _dbContext.SaveChanges();

            // Act
            var user = await _repository.FindByEmailAsync("");

            // Assert
            Assert.Null(user);
        }
    }
}
