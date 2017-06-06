using Nancy;

namespace HdbApi.Modules
{
    public class DeleteModules : NancyModule
    {
        public DeleteModules()
        {
            Get["/delete"] = _ => "This is where processes that delete data from HDB will be housed...";
        }
    }
}