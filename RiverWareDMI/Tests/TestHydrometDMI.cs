using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries.Hydromet;
using ReclamationTesting;
using Reclamation.Core;

namespace ReclamationTesting.RiverWareDmiTest
{
    [TestFixture]
    public class TestHydrometDMI
    {
        /// <summary>
        /// reads hydromet data starting at controler start date
        /// and ends based on !count=value in control file
        /// </summary>
        [Test]
        public void InitialConditions()
        {
           string cf = Globals.TestDataPath+ "\\RiverWare\\initial_conditions_control.txt";
           DateTime t = DateTime.Now.Date.AddDays(-1);
            //c:\temp\Storage.Jackson.txt
            
            Reclamation.Riverware.HydrometDMI dmi;
            dmi = new Reclamation.Riverware.HydrometDMI(HydrometHost.PN, cf, t, DateTime.Now);
            dmi.ExportTextFiles();

        }

        /// <summary>
        /// Reads hydromet data using riverWare controler dates
        /// </summary>
        [Test]
        public void ReadHydrometUingControlerDates()
        {
            string cf = Globals.TestDataPath + "\\RiverWare\\importHydromet_control.txt";
            DateTime t1 = DateTime.Now.Date.AddDays(-100);
            DateTime t2 = DateTime.Now.Date.AddDays(-50);
            //c:\temp\Storage.Jackson.txt

            Reclamation.Riverware.HydrometDMI dmi;
            dmi = new Reclamation.Riverware.HydrometDMI(HydrometHost.PN, cf, t1, t2);
            dmi.ExportTextFiles();

        }
    }
}
