using Moq;
using Skeleton.ServiceName.Business.Implementations;
using Skeleton.ServiceName.Data;
using Skeleton.ServiceName.Business.Extensions;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using Skeleton.ServiceName.Messages.Interfaces;

namespace Skeleton.ServiceName.UnitTest.Business
{
    public class PersonServiceTest
    {
        private readonly Mock<IRepository<Person>> _personRepositoryMock;
        private readonly Mock<IServiceBus> _serviceBusMock;
        private readonly Mock<IApplicationInsights> _applicationInsightsMock;

        private readonly PersonService _personService;

        public PersonServiceTest()
        {
            _personRepositoryMock = new Mock<IRepository<Person>>();
            _serviceBusMock = new Mock<IServiceBus>();
            _applicationInsightsMock = new Mock<IApplicationInsights>();
            _personService = new PersonService(_personRepositoryMock.Object, _serviceBusMock.Object, _applicationInsightsMock.Object);
        }

        [Fact]
        public async Task Get_person_success()
        {
            //Arrange
            var id = 1;
            var fakePerson = GetFakePerson();

            _personRepositoryMock.Setup(x => x.FindAsync(It.IsAny<long>()))
                .Returns(Task.FromResult(fakePerson));

            //Act
            var returnedPerson = await _personService.GetAsync(id);

            //Assert
            Assert.True(fakePerson.IsEqualTo(returnedPerson));
        }

        [Fact]
        public void Get_all_people_success()
        {
            //Arrange
            var fakePeople = GetFakePersonList();
            _personRepositoryMock.Setup(x => x.All)
                .Returns(fakePeople);

            //Act
            var returnedList = _personService.All().OrderBy(x => x.Id).ToList();
            var modelList = fakePeople.OrderBy(x => x.Id).ToList();

            //Assert
            Assert.Equal(modelList.Count(), returnedList.Count());
            for (int i = 0; i < modelList.Count(); i++)
            {
                Assert.True(modelList[i].IsEqualTo(returnedList[i]));
            }
        }

        private Person GetFakePerson()
        {
            return new Person()
            {
                Id = 1,
                FirstName = "First Name",
                LastName = "Last Name",
                BirthDate = new DateTime(1985, 11, 5)
            };
        }

        private IQueryable<Person> GetFakePersonList()
        {
            var list = new List<Person>()
            {
                 new Person()
                {
                    Id = 1,
                    FirstName = "First Name 1",
                    LastName = "Last Name 1",
                    BirthDate = new DateTime(1985, 11, 1)
                },
                 new Person()
                {
                    Id = 2,
                    FirstName = "First Name 2",
                    LastName = "Last Name 2",
                    BirthDate = new DateTime(1985, 11, 2)
                },
                 new Person()
                {
                    Id = 3,
                    FirstName = "First Name 3",
                    LastName = "Last Name 3",
                    BirthDate = new DateTime(1985, 11, 3)
                },
                 new Person()
                {
                    Id = 4,
                    FirstName = "First Name 4",
                    LastName = "Last Name 4",
                    BirthDate = new DateTime(1985, 11, 4)
                }

            };
            return list.AsQueryable();
        }
    }
}
