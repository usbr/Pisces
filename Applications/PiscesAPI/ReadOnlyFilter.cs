using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace PiscesAPI
{
    /// <summary>
    /// modified from:
    /// https://github.com/jenyayel/SwaggerSecurityTrimming/blob/master/src/V2/SwaggerAuthorizationFilter.cs
    /// </summary>
    public class ReadOnlyFilter:IDocumentFilter
    {
        private IServiceProvider _provider;

        public ReadOnlyFilter(IServiceProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            this._provider = provider;
        }

        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            var http = this._provider.GetRequiredService<IHttpContextAccessor>();
            var auth = this._provider.GetRequiredService<IAuthorizationService>();

            var descriptions = context.ApiDescriptionsGroups.Items.SelectMany(group => group.Items);

            foreach (var description in descriptions)
            {
                var authAttributes = description.ControllerAttributes()
                    .OfType<AuthorizeAttribute>()
                    .Union(description.ActionAttributes()
                        .OfType<AuthorizeAttribute>());


                var route = "/" + description.RelativePath.TrimEnd('/');
                var path = swaggerDoc.Paths[route];

                // remove method or entire path (if there are no more methods in this path)
                switch (description.HttpMethod)
                {
                    case "DELETE": path.Delete = null; break;
                    //case "GET": path.Get = null; break;
                    case "HEAD": path.Head = null; break;
                    case "OPTIONS": path.Options = null; break;
                    case "PATCH": path.Patch = null; break;
                    case "POST": path.Post = null; break;
                    case "PUT": path.Put = null; break;
                    default: break;// throw new ArgumentOutOfRangeException("Method name not mapped to operation");
                }

                if (path.Delete == null && path.Get == null &&
                    path.Head == null && path.Options == null &&
                    path.Patch == null && path.Post == null && path.Put == null)
                    swaggerDoc.Paths.Remove(route);
            }
        }


        private static bool isForbiddenDueAnonymous(
            IHttpContextAccessor http,
            IEnumerable<AuthorizeAttribute> attributes)
        {
            return attributes.Any() && !http.HttpContext.User.Identity.IsAuthenticated;
        }

        
    }
}