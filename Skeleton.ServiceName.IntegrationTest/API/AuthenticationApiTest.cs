using Newtonsoft.Json;
using Skeleton.ServiceName.MockData.Classes;
using Skeleton.ServiceName.ViewModel.Authentication;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Skeleton.ServiceName.IntegrationTest.API
{
    public class AuthenticationApiTest : BaseTest
    {
        public AuthenticationApiTest() : base()
        {
        }

        [Theory]
        [InlineData("POST", "1")]
        public async Task PartnerLogin_TestAsync(string method, string version)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/Authentications/Login");
            var user = UserMock.NewUserLoginViewModel();
            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<LoginResponseViewModel>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(responseObject.Success);
            Assert.NotNull(responseObject.Token);
        }

        [Theory]
        [InlineData("POST", "1")]
        public async Task PartnerLogin_Failed_TestAsync(string method, string version)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/Authentications/Login");
            var user = UserMock.NewUserLoginViewModel();
            user.Password = "pass";

            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<LoginResponseViewModel>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(responseObject.Success);
            Assert.Null(responseObject.Token);
        }

        [Theory]
        [InlineData("POST", "1")]
        public async Task PartnerLogin_InvalidModel_TestAsync(string method, string version)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/v{version}/Authentications/Login");
            var user = UserMock.NewUserLoginViewModel();
            user.Email = null;

            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.SendAsync(request);

            // Assert

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
