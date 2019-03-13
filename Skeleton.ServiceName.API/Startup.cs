using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Skeleton.ServiceName.API.Helpers;
using Skeleton.ServiceName.Business.Implementations;
using Skeleton.ServiceName.Business.Interfaces;
using Skeleton.ServiceName.Business.Profiles;
using Skeleton.ServiceName.Data;
using Skeleton.ServiceName.Messages.Helpers;
using Skeleton.ServiceName.Messages.Implementations;
using Skeleton.ServiceName.Messages.Interfaces;
using Skeleton.ServiceName.Utils;
using Skeleton.ServiceName.Utils.Middlewares;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Skeleton.ServiceName.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        private IHostingEnvironment CurrentEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperDomainProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddLocalization(options => options.ResourcesPath = "../Skeleton.ServiceName.Utils/Resources");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.Configure<ApiBehaviorOptions>(options => {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });


            services.AddDbContext<ServiceNameContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("DataBase"));
            });

            ConfigureEventBus(services);
            ConfigureApplicationInsights(services);

            ConfigureAuthService(services);
            ConfigureScope(services);
        }

        private void ConfigureApplicationInsights(IServiceCollection services)
        {
            // Ativando o Application Insights
            services.AddApplicationInsightsTelemetry(Configuration);

            var applicationInsightsSettings = new ApplicationInsightsSettings();
            new ConfigureFromConfigurationOptions<ApplicationInsightsSettings>(
                Configuration.GetSection("ApplicationInsightsSettings"))
                    .Configure(applicationInsightsSettings);
            services.AddSingleton(applicationInsightsSettings);

            services.AddSingleton<IApplicationInsights, ApplicationInsights>(sp =>
            {
                return new ApplicationInsights(applicationInsightsSettings);
            });
        }

        private void ConfigureAuthService(IServiceCollection services)
        {

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            
            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            if (CurrentEnvironment.EnvironmentName == "Development")
            {
                services.AddMvc(opts =>
                {
                    opts.Filters.Add(new AllowAnonymousFilter());
                });
            }
        }

        private void ConfigureEventBus(IServiceCollection services)
        {
            var serviceBusConfigurations = new ServiceBusSettings();
            new ConfigureFromConfigurationOptions<ServiceBusSettings>(
                Configuration.GetSection("ServiceBusConfigurations"))
                    .Configure(serviceBusConfigurations);
            services.AddSingleton(serviceBusConfigurations);

            services.AddSingleton<IServiceBus, ServiceBus>(sp =>
            {
                var iApplicationInsights = sp.GetRequiredService<IApplicationInsights>();

                return new ServiceBus(serviceBusConfigurations, iApplicationInsights);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHttpExceptionMiddleware();

            }
            else
            {
                app.UseHttpExceptionMiddleware();
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var supportedCultures = CultureHelper.GetCultures();

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });
            
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void ConfigureScope(IServiceCollection services)
        {
            //DataBase
            services.AddTransient<IRepository<Person>, Repository<Person>>();

            //Interfaces

            //Interfaces - Sender Bus Messages

            //TODO: Implementar uma maneira de auto mapear todas as dependências
            services.AddScoped(sp =>
            {
                var person = sp.GetRequiredService<IRepository<Person>>();
                var iMapper = sp.GetRequiredService<IMapper>();
                var iServiceBus = sp.GetRequiredService<IServiceBus>();
                var iApplicationInsights = sp.GetRequiredService<IApplicationInsights>();

                var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var serviceAssembly = Assembly.LoadFrom(Path.Combine(assemblyPath, "Skeleton.ServiceName.Business.dll"));

                var serviceConcreteType = serviceAssembly.DefinedTypes.First(x => x.IsClass && !x.IsAbstract && x.ImplementedInterfaces.Contains(typeof(IPersonService)));
                var serviceObject = (IPersonService)Activator.CreateInstance(serviceConcreteType, person, iMapper, iServiceBus, iApplicationInsights);
                return serviceObject;
            });
        }
    }
}
