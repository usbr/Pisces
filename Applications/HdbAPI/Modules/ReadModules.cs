using Nancy;

namespace HdbApi.Modules
{
    public class ReadModules : NancyModule
    {
        public ReadModules()
        {

            Get["/read"] = args =>
            {
                string response = "You are trying to get data from the HDB_DATATYPE table...";

                return response;
            };

            Get["/read/{db}&{sdid}&{t1}&{t2}&{table?R}&{mrid?0}&{format}"] = args =>
            {
                string response = "You are trying to connect to " + 
                    args.db + " to get data for SDID#" + args.sdid +
                    " from " & args.t1 + " to " & args.t2 & " in " + 
                    args.format + " format.";
                
                return response;
            };

            Get["/read/sites/"] = args =>
            {
                string response = "You are trying to get data from the HDB_SITE table";

                var names = this.Request.Query["names"];
                var types = this.Request.Query["types"];
                if (names.value != null)
                { response += " using site name= " + names; }
                if (types.value != null)
                { response += " using site type=" + types; }


                return response;
            };

            Get["/read/datatypes/{names?}&{types?}"] = args =>
            {
                string response = "You are trying to get data from the HDB_DATATYPE table...";

                return response;
            };

            Get["/read/series/{sitenames?}&{sitetypes?}&{datanames?}&{datatypes?}&{sdids?}"] = args =>
            {
                string response = "You are trying to get data from the HDB_DATATYPE table...";

                return response;
            };

            Get["/read/database/{names?}"] = args =>
            {
                string response = "You are trying to get data from the HDB_DATATYPE table...";

                return response;
            };
        }
    }
}