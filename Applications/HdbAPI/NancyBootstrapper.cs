using Nancy;
using Nancy.Diagnostics;
using System.Configuration;

namespace HdbApi
{
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper
        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get { return new DiagnosticsConfiguration { Password = ConfigurationManager.AppSettings["NancyDiagnosticsPassword"] }; }
        }
    }
}