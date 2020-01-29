using Newtonsoft.Json;
using Skeleton.ServiceName.MockData.Classes;
using Skeleton.ServiceName.ViewModel.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Skeleton.ServiceName.IntegrationTest.API
{
    public class PeopleApiTest : BaseTest
    {
        public PeopleApiTest() : base()
        {

        }

        [Theory]
        [InlineData("GET", "1")]
        public async Task PersonGetAll_TestAsync(string method, string version)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/People/");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenMaster);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseList = JsonConvert.DeserializeObject<IList<PersonViewModel>>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.IsType<List<PersonViewModel>>(responseList);
        }

        [Theory]
        [InlineData("GET", "1")]
        public async Task PersonGetAll_FirstPage_TestAsync(string method, string version)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/People/?pageNumber=2&pageSize=1");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenMaster);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseList = JsonConvert.DeserializeObject<IList<PersonViewModel>>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, responseList.Count);
            Assert.IsType<List<PersonViewModel>>(responseList);
        }

        [Theory]
        [InlineData("GET", "1")]
        public async Task PersonGetAll_Unauthorized_TestAsync(string method, string version)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/People/");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "asdasdasdasdasda");

            // Act
            var response = await _client.SendAsync(request);

            // Assert

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", "1", "0c1704a8-71f1-4dc8-a737-b15421a1661b")]
        public async Task PersonGet_TestAsync(string method, string version, string id)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/People/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenMaster);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<PersonViewModel>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Guid.Parse(id), responseObject.Id);
        }

        [Theory]
        [InlineData("GET", "1", "dce29deb-0000-0000-0000-7cbe056db556")]
        public async Task PersonGet_NotFound_TestAsync(string method, string version, string id)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/People/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenMaster);

            // Act
            var response = await _client.SendAsync(request);

            // Assert

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("POST", "1")]
        public async Task PersonInsert_TestAsync(string method, string version)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/People/");
            var person = PersonMock.NewPersonViewModel();
            request.Content = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenMaster);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<PersonViewModel>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(responseObject);
            Assert.NotNull(responseObject.Id);
        }

        [Theory]
        [InlineData("POST", "1")]
        public async Task PersonInsert_InvalidModel_TestAsync(string method, string version)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/People/");
            var person = PersonMock.NewPersonViewModel();
            person.FirstName = "";

            request.Content = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenMaster);


            // Act
            var response = await _client.SendAsync(request);

            // Assert

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("PUT", "1", "0c1704a8-71f1-4dc8-a737-b15421a1661b")]
        public async Task PersonUpdate_TestAsync(string method, string version, string id)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/People/");
            var person = PersonMock.ListPersonViewModel().FirstOrDefault();
            person.FirstName = "Updated Name";

            request.Content = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenMaster);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<PersonViewModel>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(person.FirstName, responseObject.FirstName);
        }

        [Theory]
        [InlineData("POST", "1", "0c1704a8-71f1-4dc8-a737-b15421a1661b")]
        public async Task PersonUpdate_InvalidModel_TestAsync(string method, string version, string id)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/People/");
            var person = PersonMock.GetPersonViewModel(Guid.Parse(id));
            person.FirstName = "";

            request.Content = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenMaster);


            // Act
            var response = await _client.SendAsync(request);

            // Assert

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("DELETE", "1", "0c1704a8-71f1-4dc8-a737-b15421a1661b")]
        public async Task PersonDelete_TestAsync(string method, string version, string id)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/People/{id}");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenMaster);


            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [InlineData("DELETE", "1", "b16a2319-0000-0000-0000-dc878b482fe8")]
        public async Task PersonDelete_NoContent_TestAsync(string method, string version, string id)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/People/{id}");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenMaster);


            // Act
            var response = await _client.SendAsync(request);

            // Assert

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
