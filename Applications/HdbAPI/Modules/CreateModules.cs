using Nancy;

namespace HdbApi.Modules
{
    public class CreateModules : NancyModule
    {
        public CreateModules()
        {
            Get["/create"] = _ => "This is where processes that create new entries in HDB will be housed...";
        }
    }
}