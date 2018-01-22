using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace PiscesAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public static string ApiConnectionString = "";
        public static string PiscesAPIDatabase = "mysql";
        private string versionName = "v0";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            PiscesAPIDatabase = Environment.GetEnvironmentVariable("PiscesAPIDatabase");
            ApiConnectionString = Environment.GetEnvironmentVariable("ConnectionString");

            // Try to get DB info from appsettings.json if ENV is not defined
            if (PiscesAPIDatabase == null && ApiConnectionString == null)
            {
                PiscesAPIDatabase = Configuration["DatabaseProvider:DefaultDB"];
                ApiConnectionString = Configuration["ConnectionString:DefaultConnection"];
            }

            if (PiscesAPIDatabase == "" || PiscesAPIDatabase == null)
                throw new Exception("Error: 'PiscesAPIDatabase' must be either 'mysql' or 'postgres' as an Environment Variable or in appsettings.json");
            if (ApiConnectionString == "" || ApiConnectionString == null)
                throw new Exception("Error: 'ConnectionString' must be set as an Environment Variable or in appsettings.json");


            services.Configure<IISOptions>(options =>
            {
                options.AuthenticationDisplayName = "";
                options.AutomaticAuthentication = false;
                options.ForwardClientCertificate = false;
            });

            services.AddMvc();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //https://github.com/domaindrivendev/Swashbuckle.AspNetCore#swashbuckleaspnetcoreswagger
            services.AddSwaggerGen(c =>
            {
                var filePath = System.IO.Path.Combine(Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationBasePath, "PiscesAPI.xml");
                c.IncludeXmlComments(filePath);
                c.SwaggerDoc(versionName, new Info
                {
                    Title = "Pisces API "+Reclamation.Core.AssemblyUtility.GetVersion(),
                    Version = versionName,
                    Description = "https://www.usbr.gov/pn/hydromet/disclaimer.html",
                    Contact = new Contact
                    {
                        Email = "jrocha@usbr.gov;ktarbet@usbr.gov",
                    },
                    License = new License
                    {
                        Name = "License",
                        Url = "https://opensource.org/licenses/MIT"

                    }
                    
                    
                });
                //c.DocumentFilter<ReadOnlyFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/" + versionName + "/swagger.json", "Pisces API V0");
            });
        }
    }




    /// <summary>
    /// Custom Middleware for handling error reponses
    /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware?tabs=aspnetcore2x
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var correlationId = Guid.NewGuid();

            var metadata = new PiscesAPI.Models.ErrorInfoModel
            {
                Message = exception.Message + exception.StackTrace + ". Contact the developer for more information.",// "An unexpected error occurred! Please use the Error ID to contact support",
                TimeStamp = DateTime.UtcNow,
                RequestUri = new Uri(context.Request.GetDisplayUrl()),//.Request.HttpContext.Request.RequestUri,
                ErrorId = correlationId
            };

            var result = JsonConvert.SerializeObject(metadata);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;
            return context.Response.WriteAsync(result);
        }
    }

}
