using Moq;
using Skeleton.ServiceName.Business.Implementations;
using Skeleton.ServiceName.Data;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using Skeleton.ServiceName.Messages.Interfaces;
using AutoMapper;
using Skeleton.ServiceName.ViewModel.People;
using Skeleton.ServiceName.Data.Models;

namespace Skeleton.ServiceName.UnitTest.Business
{
    public class PersonServiceTest
    {
        private readonly Mock<IRepository<Person>> _personRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IServiceBus> _serviceBusMock;
        private readonly Mock<IApplicationInsights> _applicationInsightsMock;

        private readonly PersonService _personService;

        public PersonServiceTest()
        {
            _personRepositoryMock = new Mock<IRepository<Person>>();
            _mapperMock = new Mock<IMapper>();
            _serviceBusMock = new Mock<IServiceBus>();
            _applicationInsightsMock = new Mock<IApplicationInsights>();
            _personService = new PersonService(_personRepositoryMock.Object, _mapperMock.Object, _serviceBusMock.Object, _applicationInsightsMock.Object);
        }

        [Fact]
        public async Task Get_person_success()
        {
            //Arrange
            var id = 1;
            var fakePerson = GetFakePerson();
            var fakeModel = GetFakePersonViewModel();

            _mapperMock.Setup(m => m.Map<Person, PersonViewModel>(It.IsAny<Person>())).Returns(fakeModel);
            _personRepositoryMock.Setup(x => x.FindAsync(It.IsAny<long>()))
                .Returns(Task.FromResult(fakePerson));

            //Act
            var returnedPerson = await _personService.GetAsync(id);

            //Assert
            Assert.Equal(fakePerson.Id, returnedPerson.Id);
            Assert.Equal(fakeModel, returnedPerson);
            Assert.IsType<PersonViewModel>(returnedPerson);
        }

        [Fact]
        public void Get_all_people_success()
        {
            //Arrange
            var fakePeople = GetFakePersonList();
            var fakeModelList = GetFakePersonViewModelList();

            _mapperMock.Setup(m => m.Map<IEnumerable<Person>, IList<PersonViewModel>>(It.IsAny<IQueryable<Person>>())).Returns(fakeModelList);

            _personRepositoryMock.Setup(x => x.All)
                .Returns(fakePeople);

            //Act
            var returnedList = _personService.All();

            //Assert
            Assert.Equal(fakeModelList.Count(), returnedList.Count());
            Assert.Equal(fakeModelList, returnedList);
            Assert.IsType<List<PersonViewModel>>(returnedList);
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

        private PersonViewModel GetFakePersonViewModel()
        {
            return new PersonViewModel()
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

        private IList<PersonViewModel> GetFakePersonViewModelList()
        {
            var list = new List<PersonViewModel>()
            {
                 new PersonViewModel()
                {
                    Id = 1,
                    FirstName = "First Name 1",
                    LastName = "Last Name 1",
                    BirthDate = new DateTime(1985, 11, 1)
                },
                 new PersonViewModel()
                {
                    Id = 2,
                    FirstName = "First Name 2",
                    LastName = "Last Name 2",
                    BirthDate = new DateTime(1985, 11, 2)
                },
                 new PersonViewModel()
                {
                    Id = 3,
                    FirstName = "First Name 3",
                    LastName = "Last Name 3",
                    BirthDate = new DateTime(1985, 11, 3)
                },
                 new PersonViewModel()
                {
                    Id = 4,
                    FirstName = "First Name 4",
                    LastName = "Last Name 4",
                    BirthDate = new DateTime(1985, 11, 4)
                }

            };
            return list;
        }
    }
}
