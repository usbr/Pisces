using Nancy;

namespace HdbApi.Modules
{
    public class UpdateModules : NancyModule
    {
        public UpdateModules()
        {
            Get["/update"] = _ => "This is where processes that update existing data in HDB will be housed...";
        }
    }
}