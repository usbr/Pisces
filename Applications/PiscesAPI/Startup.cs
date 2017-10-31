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

namespace PiscesAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("v0", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Pisces API", Version = "v0" });
                var filePath = System.IO.Path.Combine(Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationBasePath, "PiscesAPI.xml");
                c.IncludeXmlComments(filePath);
                c.SwaggerDoc("v0", new Info
                {
                    Title = "Pisces API Data Service",
                    Version = "v0",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore " +
                        "et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex " +
                        "ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat " + 
                        "nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim "+
                        "id est laborum." +
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
                        Email = "jrocha@usbr.gov"
                    },
                    License = new License
                    {
                        Name = "MIT License",
                        Url = "https://opensource.org/licenses/MIT"
                    }
                });

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v0/swagger.json", "Pisces API V0");
            });
        }
    }
}
