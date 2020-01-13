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
    public class PersonRepositoryTest
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ServiceNameContext _dbContext;
        private IPersonRepository _repository;

        public PersonRepositoryTest()
        {
            _httpContextAccessor = new HttpContextAccessor();
        }

        [Fact]
        public void Person_All()
        {
            // Arrange
            var listPeople = PersonMock.ListPerson().OrderBy(x => x.Id).ToList();
            _dbContext = new InMemoryDbContextFactory().GetAuthDbContext("Person_All", _httpContextAccessor);
            _repository = new PersonRepository(_dbContext);
            DataSeed.Seed(_dbContext);


            // Act
            var people = _repository.All;

            // Assert
            Assert.Equal(listPeople.Count, people.Count());
            Assert.IsAssignableFrom<IQueryable<Person>>(people);
        }

        [Fact]
        public void Person_Update()
        {
            // Arrange
            var id = Guid.NewGuid();
            var fakePerson = PersonMock.GetPerson(id);
            var newName = "new Name";

            _dbContext = new InMemoryDbContextFactory().GetAuthDbContext("Person_Update", _httpContextAccessor);
            _repository = new PersonRepository(_dbContext);
            DataSeed.Seed(_dbContext);

            _dbContext.People.AddRange(fakePerson);
            _dbContext.SaveChanges();

            fakePerson.FirstName = newName;

            // Act
            _repository.Update(fakePerson);
            var person = _repository.Find(id);

            // Assert
            Assert.Equal(newName, person.FirstName);
            Assert.Equal(id, fakePerson.Id);
        }

        [Fact]
        public void Person_Delete()
        {
            // Arrange
            var id = Guid.NewGuid();
            var fakePerson = PersonMock.GetPerson(id);

            _dbContext = new InMemoryDbContextFactory().GetAuthDbContext("Person_Delete", _httpContextAccessor);
            _repository = new PersonRepository(_dbContext);
            DataSeed.Seed(_dbContext);
            _dbContext.People.AddRange(fakePerson);
            _dbContext.SaveChanges();

            // Act
            _repository.Delete(fakePerson);
            var person = _repository.Find(id);

            // Assert
            Assert.Null(person);
        }

        [Fact]
        public void Person_Get()
        {
            // Arrange
            var id = Guid.NewGuid();
            var fakePerson = PersonMock.GetPerson(id);
            _dbContext = new InMemoryDbContextFactory().GetAuthDbContext("Person_Get", _httpContextAccessor);
            _repository = new PersonRepository(_dbContext);
            DataSeed.Seed(_dbContext);
            _dbContext.People.AddRange(fakePerson);
            _dbContext.SaveChanges();


            // Act
            var person = _repository.Find(id);

            // Assert
            Assert.Equal(fakePerson, person);
            Assert.Equal(id, fakePerson.Id);
        }

        [Fact]
        public void Person_Insert()
        {
            // Arrange
            var fakePerson = PersonMock.NewPerson();

            _dbContext = new InMemoryDbContextFactory().GetAuthDbContext("Person_Insert", _httpContextAccessor);
            _repository = new PersonRepository(_dbContext);
            DataSeed.Seed(_dbContext);

            // Act
            _repository.Insert(fakePerson);
            var person = _repository.Find(fakePerson.Id);

            // Assert
            Assert.Equal(fakePerson.FirstName, person.FirstName);
            Assert.Equal(fakePerson.Id, fakePerson.Id);
            Assert.NotNull(person);
        }

        [Fact]
        public async Task Person_GetAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var fakePerson = PersonMock.GetPerson(id);
            _dbContext = new InMemoryDbContextFactory().GetAuthDbContext("Person_GetAsync", _httpContextAccessor);
            _repository = new PersonRepository(_dbContext);
            DataSeed.Seed(_dbContext);
            _dbContext.People.AddRange(fakePerson);
            _dbContext.SaveChanges();


            // Act
            var person = await _repository.FindAsync(id);

            // Assert
            Assert.Equal(fakePerson, person);
            Assert.Equal(id, fakePerson.Id);
        }



        [Fact]
        public async Task Person_UpdateAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var fakePerson = PersonMock.GetPerson(id);
            var newName = "new Name";

            _dbContext = new InMemoryDbContextFactory().GetAuthDbContext("Person_UpdateAsync", _httpContextAccessor);
            _repository = new PersonRepository(_dbContext);
            DataSeed.Seed(_dbContext);
            _dbContext.People.AddRange(fakePerson);
            _dbContext.SaveChanges();

            fakePerson.FirstName = newName;

            // Act
            await _repository.UpdateAsync(fakePerson);
            var person = await _repository.FindAsync(id);

            // Assert
            Assert.Equal(newName, person.FirstName);
            Assert.Equal(id, fakePerson.Id);
        }

        [Fact]
        public async Task Person_DeleteAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var fakePerson = PersonMock.GetPerson(id);

            _dbContext = new InMemoryDbContextFactory().GetAuthDbContext("Person_DeleteAsync", _httpContextAccessor);
            _repository = new PersonRepository(_dbContext);
            DataSeed.Seed(_dbContext);
            _dbContext.People.AddRange(fakePerson);
            _dbContext.SaveChanges();

            // Act
            await _repository.DeleteAsync(fakePerson);
            var person = await _repository.FindAsync(id);

            // Assert
            Assert.Null(person);
        }

        [Fact]
        public async Task Person_InsertAsync()
        {
            // Arrange
            var fakePerson = PersonMock.NewPerson();

            _dbContext = new InMemoryDbContextFactory().GetAuthDbContext("Person_InsertAsync", _httpContextAccessor);
            _repository = new PersonRepository(_dbContext);
            DataSeed.Seed(_dbContext);

            // Act
            await _repository.InsertAsync(fakePerson);
            var person = await _repository.FindAsync(fakePerson.Id);

            // Assert
            Assert.Equal(fakePerson.FirstName, person.FirstName);
            Assert.Equal(fakePerson.Id, fakePerson.Id);
            Assert.NotNull(person);
        }
    }
}
