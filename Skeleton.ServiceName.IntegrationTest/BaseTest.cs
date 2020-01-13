using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Skeleton.ServiceName.API;
using Skeleton.ServiceName.Data;
using Skeleton.ServiceName.MockData.Classes;
using Skeleton.ServiceName.ViewModel.Authentication;
using System;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Skeleton.ServiceName.IntegrationTest
{
    public class BaseTest
    {
        protected readonly HttpClient _client;
        protected readonly string _tokenMaster;
        protected readonly string _tokenStaff;

        public BaseTest()
        {
            var path = PlatformServices.Default.Application.ApplicationBasePath;
            var setDir = Path.GetFullPath(Path.Combine(path, @"..\..\..\..\Skeleton.ServiceName.API"));


            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost => webHost
                    .UseTestServer()
                    .UseStartup<Startup>()
                    .UseContentRoot(setDir)
                    .UseEnvironment("Test")
                    .UseConfiguration(new ConfigurationBuilder()
                        .SetBasePath(setDir)
                        .AddJsonFile("appsettings.Test.json")
                        .Build()
                ))
                .ConfigureServices(services =>
                {

                    // Create a new service provider.
                    var serviceProvider = new ServiceCollection()
                        .AddEntityFrameworkInMemoryDatabase()
                        .BuildServiceProvider();

                    // Add a database context (ApplicationDbContext) using an in-memory 
                    // database for testing.
                    services.AddDbContext<ServiceNameContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                        options.UseInternalServiceProvider(serviceProvider);
                    });

                    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                    services.AddSingleton<ServiceNameContext>();

                    // Build the service provider.
                    var sp = services.BuildServiceProvider();
                    // Create a scope to obtain a reference to the database contexts
                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var appDb = scopedServices.GetRequiredService<ServiceNameContext>();
                        // Ensure the database is created.
                        appDb.Database.EnsureCreated();

                        try
                        {
                            // Seed the database with some specific test data.
                            DataSeed.Seed(appDb);
                        }
                        catch (Exception ex)
                        {
                        }
                    };
                });

            // Build and start the IHost
            var host = hostBuilder.StartAsync().Result;

            // Create an HttpClient to send requests to the TestServer
            _client = host.GetTestClient();

            var userMaster = UserMock.NewUserLoginViewModel();
            var userStaff = UserMock.NewNotMasterUserLoginViewModel();

            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/api/v1/Authentications/Login");
            request.Content = new StringContent(JsonConvert.SerializeObject(userMaster), Encoding.UTF8, "application/json");
            var response = _client.SendAsync(request).Result;
            var teste = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<LoginResponseViewModel>(response.Content.ReadAsStringAsync().Result);
            if (responseObject.Token != null)
                _tokenMaster = responseObject.Token;

            var requestStaff = new HttpRequestMessage(new HttpMethod("POST"), $"/api/v1/Authentications/Login");
            requestStaff.Content = new StringContent(JsonConvert.SerializeObject(userStaff), Encoding.UTF8, "application/json");
            var responseStaff = _client.SendAsync(requestStaff).Result;
            var responseObjectStaff = JsonConvert.DeserializeObject<LoginResponseViewModel>(responseStaff.Content.ReadAsStringAsync().Result);
            if (responseObjectStaff.Token != null)
                _tokenStaff = responseObjectStaff.Token;

        }
    }
}
