using AutoMapper;
using NSubstitute;
using Skeleton.ServiceName.Business.Implementations;
using Skeleton.ServiceName.Business.Interfaces;
using Skeleton.ServiceName.Business.Parameters;
using Skeleton.ServiceName.Business.Profiles;
using Skeleton.ServiceName.Data.Interfaces;
using Skeleton.ServiceName.Data.Models;
using Skeleton.ServiceName.MockData.Classes;
using Skeleton.ServiceName.ViewModel.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Skeleton.ServiceName.UnitTest.Business
{
    public class PersonServiceTest
    {
        private readonly IPersonRepository _repositoryMock;

        private readonly IPersonService _personService;

        public PersonServiceTest()
        {
            var myProfile = new AutoMapperDomainProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            _repositoryMock = Substitute.For<IPersonRepository>();
            _personService = new PersonService(_repositoryMock, mapper);
        }

        #region Get

        [Fact]
        public void Person_All()
        {
            // Arrange
            var parameters = new PersonParameters();
            var listFake = PersonMock.ListPerson().AsQueryable();
            _repositoryMock.All.Returns(listFake);

            // Act
            var list = _personService.All(parameters);

            // Assert
            Assert.Equal(2, list.Count);
            Assert.IsType<List<PersonViewModel>>(list);
        }

        [Fact]
        public async Task Person_GetAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var fake = PersonMock.GetPerson(id);

            _repositoryMock.FindAsync(id).Returns(fake);

            // Act
            var user = await _personService.GetAsync(id);

            // Assert
            Assert.Equal(id, user.Id);
            Assert.IsType<PersonViewModel>(user);
        }
        #endregion

        #region Insert
        //Insert
        [Fact]
        public async Task Person_Insert_SuccessAsync()
        {
            // Arrange
            var newViewFake = PersonMock.NewPersonViewModel();

            // Act
            var user = await _personService.InsertAsync(newViewFake);

            // Assert
            await _repositoryMock.Received().InsertAsync(Arg.Any<Person>());
            Assert.IsType<PersonViewModel>(user);
        }

        #endregion

        #region Update
        [Fact]
        public async Task Person_Update_SuccessAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var viewFake = PersonMock.GetPersonViewModel(id);
            var fake = PersonMock.GetPerson(id);

            _repositoryMock.FindNoTrackingAsync(id).Returns(fake);

            viewFake.FirstName = "Alterou";

            // Act
            var user = await _personService.UpdateAsync(viewFake);

            // Assert
            await _repositoryMock.Received().UpdateAsync(Arg.Any<Person>());
            Assert.Equal(id, user.Id);
            Assert.IsType<PersonViewModel>(user);
        }

        #endregion

        #region Delete
        [Fact]
        public async Task Person_Delete_SuccessAsync()
        {
            // Arrange
            var count = 0;
            var id = Guid.NewGuid();
            var fake = PersonMock.GetPerson(id);
            _repositoryMock.FindAsync(id).Returns(fake);
            _repositoryMock.When(x => x.DeleteAsync(fake)).Do(x => count++);

            // Act
            var done = await _personService.DeleteAsync(id);

            // Assert
            Assert.True(done);
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task Person_Delete_NotFoundAsync()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var done = await _personService.DeleteAsync(id);

            // Assert
            Assert.False(done);
            await _repositoryMock.DidNotReceive().DeleteAsync(Arg.Any<Person>());
        }
        #endregion
    }
}
