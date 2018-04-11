using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OneTravelApi.DataLayer;
using OneTravelApi.Services;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;

namespace OneTravelApi
{
    public class Startup
    {
        public static string WebRootPath { get; private set; }

        public static string MapPath(string path, string basePath = null)
        {
            if (string.IsNullOrEmpty(basePath))
            {
                basePath = WebRootPath;
            }

            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(basePath, path);
        }
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IHostingEnvironment env)
        {
            _hostingEnvironment = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddJsonOptions(a => a.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
                .AddJsonOptions(a => a.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddMvc();
            services.AddOptions();
            services.AddScoped<IEntityMapper, OneTravelEntityMapper>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddEntityFrameworkSqlServer().AddDbContext<OneTravelDbContext>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryBookingService, CategoryBookingService>();
            services.AddScoped<ICategoryBookingStatusService, CategoryBookingStatusService>();
            services.AddScoped<ICategoryCityService, CategoryCityService>();
            services.AddScoped<ICategoryGroupPartnerService, CategoryGroupPartnerService>();
            services.AddScoped<ICategoryLocalTravelService, CategoryLocalTravelService>();
            services.AddScoped<ICategoryPriorityService, CategoryPriorityService>();
            services.AddScoped<ICategoryRequestService, CategoryRequestService>();
            services.AddScoped<ICategoryRequestStatusService, CategoryRequestStatusService>();
            services.AddScoped<IPartnerService, PartnerService>();
            services.AddScoped<IPartnerContactService, PartnerContactService>();

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddSingleton(Configuration);

            services.AddAutoMapper();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "One Travel API",
                    Description = "One Travel Admin",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Tham Davies", Email = "thamdv96@gmail.com", Url = "https://facebook.com/thamdavies.dev" },
                    License = new License { Name = "Use under LICX", Url = "https://example.com/license" }
                });

                // Set the comments path for the Swagger JSON and UI.
                var basePath = _hostingEnvironment.WebRootPath;
                var xmlPath = Path.Combine(basePath, "OneTravelAPI.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "OneTravel API V1");
            });
            
            app.UseMvc();

            loggerFactory.AddFile("Logs/OneTravelApi-{Date}.txt");

            WebRootPath = env.WebRootPath;
        }
    }
}
