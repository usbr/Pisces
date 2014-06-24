using System;
using System.Data;

namespace Reclamation.TimeSeries
{
    public delegate void CustomScenarioEventHandler(object sender, ScenarioTableEventArgs ea);

    public class ScenarioTableEventArgs : EventArgs
    {

        /// <summary>
        /// Used to customize the list of scenarios.
        /// you can add extra columns to the list of scenarios.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="message"></param>
        public ScenarioTableEventArgs(DataTable table, string message)
        {
            this.table = table;
            this.message = message;
        }
        public string message;
        public DataTable table;

    }	
}
