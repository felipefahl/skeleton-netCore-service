using Newtonsoft.Json;
using Skeleton.ServiceName.MockData.Classes;
using Skeleton.ServiceName.ViewModel.User;
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
    public class UsersApiTest : BaseTest
    {
        public UsersApiTest() : base()
        {

        }

        [Theory]
        [InlineData("GET", "1")]
        public async Task UserGetAll_TestAsync(string method, string version)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/Users/");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenMaster);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseList = JsonConvert.DeserializeObject<IList<UserViewModel>>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.IsType<List<UserViewModel>>(responseList);
        }

        [Theory]
        [InlineData("GET", "1")]
        public async Task UserGetAll_Unauthorized_TestAsync(string method, string version)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/Users/");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "asdasdasdasdasda");

            // Act
            var response = await _client.SendAsync(request);

            // Assert

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", "1")]
        public async Task UserGetAll_Forbidden_TestAsync(string method, string version)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/Users/");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenStaff);

            // Act
            var response = await _client.SendAsync(request);

            // Assert

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", "1", "93321b82-f7c7-417e-8a92-540c182b67ac")]
        public async Task UserGet_TestAsync(string method, string version, string id)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/Users/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenMaster);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<UserViewModel>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Guid.Parse(id), responseObject.Id);
        }

        [Theory]
        [InlineData("GET", "1", "dce29deb-0000-0000-0000-7cbe056db556")]
        public async Task UserGet_NotFound_TestAsync(string method, string version, string id)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/Users/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenMaster);

            // Act
            var response = await _client.SendAsync(request);

            // Assert

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("POST", "1")]
        public async Task UserInsert_TestAsync(string method, string version)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/Users/");
            var user = UserMock.NewMasterUserViewModel();
            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenMaster);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<UserViewModel>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(responseObject);
            Assert.NotNull(responseObject.Id);
        }

        [Theory]
        [InlineData("POST", "1")]
        public async Task UserInsert_InvalidModel_TestAsync(string method, string version)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/Users/");
            var user = UserMock.NewMasterUserViewModel();
            user.Name = "";

            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenMaster);


            // Act
            var response = await _client.SendAsync(request);

            // Assert

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("PUT", "1", "93321b82-f7c7-417e-8a92-540c182b67ac")]
        public async Task UserUpdate_TestAsync(string method, string version, string id)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/Users/");
            var user = UserMock.ListUserViewModel().FirstOrDefault();
            user.Name = "Updated Name";

            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenMaster);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<UserViewModel>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(user.Name, responseObject.Name);
        }

        [Theory]
        [InlineData("POST", "1", "93321b82-f7c7-417e-8a92-540c182b67ac")]
        public async Task UserUpdate_InvalidModel_TestAsync(string method, string version, string id)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/Users/");
            var user = UserMock.GetAdminUserViewModel(Guid.Parse(id));
            user.Name = "";

            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenMaster);


            // Act
            var response = await _client.SendAsync(request);

            // Assert

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("DELETE", "1", "93321b82-f7c7-417e-8a92-540c182b67ac")]
        public async Task UserDelete_TestAsync(string method, string version, string id)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/Users/{id}");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenMaster);


            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [InlineData("DELETE", "1", "b16a2319-0000-0000-0000-dc878b482fe8")]
        public async Task UserDelete_NoContent_TestAsync(string method, string version, string id)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/Users/{id}");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenMaster);


            // Act
            var response = await _client.SendAsync(request);

            // Assert

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
