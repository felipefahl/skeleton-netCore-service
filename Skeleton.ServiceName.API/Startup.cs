using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Skeleton.ServiceName.Business.Profiles;
using Skeleton.ServiceName.Data;
using Skeleton.ServiceName.Utils;
using Skeleton.ServiceName.Utils.Middlewares;
using Skeleton.ServiceName.Utils.Models;
using Skeleton.ServiceName.Utils.Security;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Skeleton.ServiceName.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        private IWebHostEnvironment CurrentEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            //services.AddApplicationInsightsTelemetry(Configuration);

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperDomainProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddLocalization(options => options.ResourcesPath = "../Skeleton.ServiceName.Utils/Resources");

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    var serializerOptions = options.JsonSerializerOptions;
                    serializerOptions.IgnoreNullValues = true;
                    serializerOptions.IgnoreReadOnlyProperties = true;
                    serializerOptions.WriteIndented = true;
                });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            ConfigureAuthService(services);
            ConfigureDatabase(services);
            ConfigureScope(services);
            ConfigureSwagger(services);
        }

        private void ConfigureAuthService(IServiceCollection services)
        {
            // Configurando a dependência para a classe de validação
            // de credenciais e geração de tokens

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);


            var signingConfigurations = new SigningConfigurations(tokenConfigurations.Secret);
            services.AddSingleton(signingConfigurations);

            services.AddSingleton<IAccessManager, AccessManager>(sp =>
            {

                return new AccessManager(signingConfigurations, tokenConfigurations);
            });

            // Aciona a extensão que irá configurar o uso de
            // autenticação e autorização via tokens
            services.AddJwtSecurity(
                signingConfigurations, tokenConfigurations);

            if (CurrentEnvironment.EnvironmentName == "Development")
            {
                services.AddMvc(opts =>
                {
                    opts.Filters.Add(new AllowAnonymousFilter());
                });
            }

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public virtual void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<ServiceNameContext>(options =>
                options.UseInMemoryDatabase("InMemoryDatabase"));
            //services.AddEntityFrameworkSqlServer()
            //    .AddDbContext<ServiceNameContext>(
            //        options => options.UseSqlServer(
            //            Configuration.GetConnectionString("DataBase")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Ativando middlewares para uso do Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Skeleton API");
            });
        }

        private void ConfigureScope(IServiceCollection services)
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            //Services
            ConfigureServicesDI(services, assemblyPath);

            //Data
            ConfigureRepositoriesDI(services, assemblyPath);
        }

        /// <summary>
        /// Method to auto register all services to the Dependency Injection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblyPath"></param>
        private void ConfigureServicesDI(IServiceCollection services, string assemblyPath)
        {
            //recovers the service dll
            var assembly = Assembly.LoadFrom(Path.Combine(assemblyPath, "Skeleton.ServiceName.Business.dll"));

            //find all interfaces
            var interfaceTypes = assembly.DefinedTypes.Where(x => x.IsInterface);
            //find all concrete classes
            var concreteTypes = assembly.DefinedTypes.Where(x => x.IsClass && !x.IsAbstract);

            foreach (var interfaceType in interfaceTypes)
            {
                //for each interface, find the matching concrete implementation and register to the Dependency Injection
                var concreteType = concreteTypes.FirstOrDefault(x => x.ImplementedInterfaces.Contains(interfaceType));
                if (concreteType != null)
                {
                    services.AddScoped(interfaceType, concreteType);
                }
            }
        }

        /// <summary>
        /// Method to auto register all services to the Dependency Injection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblyPath"></param>
        private void ConfigureRepositoriesDI(IServiceCollection services, string assemblyPath)
        {
            //recovers the service dll
            var assembly = Assembly.LoadFrom(Path.Combine(assemblyPath, "Skeleton.ServiceName.Data.dll"));

            //find all interfaces
            var interfaceTypes = assembly.DefinedTypes.Where(x => x.IsInterface);
            //find all concrete classes
            var concreteTypes = assembly.DefinedTypes.Where(x => x.IsClass && !x.IsAbstract);

            foreach (var interfaceType in interfaceTypes)
            {
                //for each interface, find the matching concrete implementation and register to the Dependency Injection
                var concreteType = concreteTypes.FirstOrDefault(x => x.ImplementedInterfaces.Contains(interfaceType));
                if (concreteType != null)
                {
                    services.AddScoped(interfaceType, concreteType);
                }
            }
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            // Configurando o serviço de documentação do Swagger
            services.AddSwaggerGen(c =>
            {
                // Swagger 2.+ support
                var security = new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                };

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(security);

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Skeleton API",
                    Version = "v1",
                    Description = "Net Core API 3.0 Skeleton - v1.0.1",
                    Contact = new OpenApiContact
                    {
                        Name = "Pecege",
                        Url = new Uri("https://fahl.com")
                    }
                });


                string caminhoAplicacao =
                    PlatformServices.Default.Application.ApplicationBasePath;
                string nomeAplicacao =
                    PlatformServices.Default.Application.ApplicationName;
                string caminhoXmlDoc =
                    Path.Combine(caminhoAplicacao, $"{nomeAplicacao}.xml");

                c.IncludeXmlComments(caminhoXmlDoc);
            });
        }
    }
}
