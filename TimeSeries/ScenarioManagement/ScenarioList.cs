using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.ScenarioManagement
{
    /// <summary>
    /// Manages of group of scenarios
    /// </summary>
    class ScenarioList
    {
        DataTable siteMapping; //SiteMapping defines a list of sites with and internal and external identity
        DataTable scenarioMapping;
        public ScenarioList(string filename)
        {
            NpoiExcel xls = new NpoiExcel(filename);

           siteMapping = xls.ReadDataTable("siteMapping");
           scenarioMapping = xls.ReadDataTable("scenarioMapping");
        }
    }
}
