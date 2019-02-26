using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Skeleton.ServiceName.API.Controllers;
using Skeleton.ServiceName.Business.Interfaces;
using Skeleton.ServiceName.ViewModel.People;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Skeleton.ServiceName.UnitTest.Controller
{
    public class PeopleControllerTest
    {
        private readonly Mock<IPersonService> _personServiceMok;
        private readonly Mock<HttpContext> _contextMock;

        private readonly PeopleController _peopleController;

        public PeopleControllerTest()
        {
            _personServiceMok = new Mock<IPersonService>();
            _contextMock = new Mock<HttpContext>();

            _peopleController = new PeopleController(_personServiceMok.Object);
        }

        [Fact]
        public async Task Get_person_success()
        {
            //Arrange
            var id = 1;
            var fakeModel = GetFakeViewModel();

            _personServiceMok.Setup(x => x.GetAsync(It.IsAny<long>()))
                .Returns(Task.FromResult(fakeModel));

            //Act
            var result = await _peopleController.Get(id) as OkObjectResult;

            //Assert
            Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.Equal(((PersonViewModel)result.Value).Id, id);
        }

        [Fact]
        public async Task Post_person_success()
        {
            //Arrange
            var id = 1;

            var fakeModel = GetFakeViewModel();
            var newFakeModel = GetNewFakeViewModel();


            _personServiceMok.Setup(x => x.SaveAsync(It.IsAny<PersonViewModel>()))
                .Returns(Task.FromResult(fakeModel));

            //Act
            var result = await _peopleController.Insert(newFakeModel) as CreatedResult;

            _peopleController.ControllerContext.HttpContext = _contextMock.Object;
            var actionResult = await _peopleController.Insert(newFakeModel);

            //Assert

            Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.Created);
            Assert.Equal(((PersonViewModel)result.Value).Id, id);
            Assert.IsType<CreatedResult>(actionResult);
        }

        [Fact]
        public async Task Put_person_success()
        {
            //Arrange
            var id = 1;

            var fakeModel = GetFakeViewModel();


            _personServiceMok.Setup(x => x.SaveAsync(It.IsAny<PersonViewModel>()))
                .Returns(Task.FromResult(fakeModel));

            //Act
            var result = await _peopleController.Update(fakeModel) as OkObjectResult;

            _peopleController.ControllerContext.HttpContext = _contextMock.Object;
            var actionResult = await _peopleController.Update(fakeModel);

            //Assert

            Assert.Equal(result.StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.Equal(((PersonViewModel)result.Value).Id, id);
            Assert.IsType<OkObjectResult>(actionResult);
        }

        private PersonViewModel GetNewFakeViewModel()
        {
            return new PersonViewModel()
            {
                FirstName = "First Name",
                LastName = "Last Name",
                BirthDate = new DateTime(1985, 11, 5)
            };
        }

        private PersonViewModel GetFakeViewModel()
        {
            return new PersonViewModel()
            {
                Id = 1,
                FirstName = "First Name",
                LastName = "Last Name",
                BirthDate = new DateTime(1985, 11, 5)
            };
        }
    }
}
