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

            if (PiscesAPIDatabase == "")
                throw new Exception("Error: environment variable  PiscesAPIDatabase must be either 'mysql' or 'postgres'");
            if (ApiConnectionString == "")
                throw new Exception("Error: environment variable  ConnectionString must be set");


            services.Configure<IISOptions>(options =>
            {
                options.AuthenticationDisplayName = "";
                options.AutomaticAuthentication = false;
                options.ForwardClientCertificate = false;
            });

            services.AddMvc();
            //https://github.com/domaindrivendev/Swashbuckle.AspNetCore#swashbuckleaspnetcoreswagger
            services.AddSwaggerGen(c =>
            {
                var filePath = System.IO.Path.Combine(Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationBasePath, "PiscesAPI.xml");
                c.IncludeXmlComments(filePath);
                c.SwaggerDoc(versionName, new Info
                {
                    Title = "Pisces API Data Service",
                    Version = versionName,
                    Description = "Although the US Bureau of Reclamation makes efforts to maintain the accuracy of data found in the Hydromet system databases, " +
                        "the data is largely unverified and should be considered preliminary and subject to change. Data and services are provided with the express " +
                        "understanding that the United States Government makes no warranties, expressed or implied, concerning the accuracy, completeness, " +
                        "usability or suitability for any particular purpose of the information or data obtained by access to this computer system, and the " +
                        "United States shall be under no liability whatsoever to any individual or group entity by reason of any use made thereof." +
                        "\n\r" +
                        "This Application Programming Interface (API) is preliminary or provisional and is subject to revision. " +
                        "It is currently in development and as such, frequent updates, downtimes, and loss of functionality are to " +
                        "be expected. The API has not received final approval by Reclamation. No warranty, expressed or implied, is " +
                        "made as to the functionality of the API nor shall the fact of release constitute any such warranty. " +
                        "The API is provided on the condition that neither Reclamation nor the U.S. Government shall be held liable " +
                        "for any damages resulting from the authorized or unauthorized use of the API.",
                    TermsOfService = "TOS Placeholder",
                    Contact = new Contact
                    {
                        //Name = "Jon Rocha",
                        Email = "jrocha@usbr.gov;ktarbet@usbr.gov"
                    },
                    License = new License
                    {
                        Name = "MIT License",
                        Url = "https://opensource.org/licenses/MIT"
                    }
                    
                });

                c.DocumentFilter<ReadOnlyFilter>();
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
