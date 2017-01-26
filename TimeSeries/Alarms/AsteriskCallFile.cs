using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reclamation.TimeSeries.Alarms
{
    public class AsteriskCallFile
    {
        List<string> file = new List<string>();
        public AsteriskCallFile(string channel,
               string context, string extension, string priority = "1")
        {
            file.Add("Channel: "+channel);
            file.Add("Context: "+context);
            file.Add("Extension: "+extension);
            file.Add("Priority: "+priority);

        }

        /// <summary>
        /// </summary>
        /// <param name="cid"> Hydromet <(208) 555-1111></param>
        public void AddCallerID(string cid)
        {
            file.Add("CallerID: "+cid);
        }

        /// <summary>
        /// Adds a channel variable
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddVariable(string name, string value)
        {
            file.Add("Set: " + name + "=" + value);
        }
    }
}
