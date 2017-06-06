using Nancy;

namespace HdbApi.Modules
{
    public class StoredProcedureModules : NancyModule
    {
        public StoredProcedureModules()
        {
            Get["/"] = _ => "This is where processes that rely on stored procedures in HDB will be housed...";
        }
    }
}