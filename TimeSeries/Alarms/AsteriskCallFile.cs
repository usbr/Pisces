using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
            Init(channel, context, extension, priority);

        }

        private void Init(string channel, string context, string extension, string priority)
        {
            file.Add("Channel: " + channel);
            file.Add("Context: " + context);
            file.Add("Extension: " + extension);
            file.Add("Priority: " + priority);
        }

        /// <summary>
        /// Creates call file using AppSettings
        /// </summary>
        /// <param name="phoneNumber"></param>
        public AsteriskCallFile(string phoneNumber)
        {

            string cid = ConfigurationManager.AppSettings["pbx_callerid"];
            string sip = ConfigurationManager.AppSettings["pbx_channel_prefix"];
            string context = ConfigurationManager.AppSettings["pbx_context"];
            string extension = ConfigurationManager.AppSettings["pbx_extension"];
            string priority = ConfigurationManager.AppSettings["pbx_priority"];

            Init(sip + phoneNumber, context, extension, priority);
            AddCallerID(cid);
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

        public string SaveToTempFile()
        {
            var fn = FileUtility.GetTempFileName(".call");
            File.WriteAllLines(fn,file.ToArray());
            return fn;
            
        }
    }
}
