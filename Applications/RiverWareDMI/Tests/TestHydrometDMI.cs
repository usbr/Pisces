using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries.Hydromet;
using ReclamationTesting;
using Reclamation.Core;
using System.IO;

namespace ReclamationTesting.RiverWareDmiTest
{
    [TestFixture]
    public class TestHydrometDMI
    {
        string[] tempFiles = new string[] 
        { 
            @"c:\temp\Storage.Jackson.txt",
            @"c:\temp\Storage.IslandPark.txt",
            @"c:\temp\Storage.Palisades.txt",
            @"c:\temp\Storage.AmericanFalls.txt",
            @"c:\temp\Inflow.BlackfootToAmericanFalls_Routing.txt",
            @"c:\temp\Inflow.IslandParkToSnake_Routing2.txt",
            @"c:\temp\Inflow.ShelleyToBlackfoot_Routing.txt",
            @"c:\temp\Inflow.HeiseToShelley_Routing.txt",
            @"c:\temp\Outflow.JacksonToPalisades_Routing.txt"
        };

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
            dmi = new Reclamation.Riverware.HydrometDMI(HydrometHost.PNLinux, cf, t, DateTime.Now);
            dmi.ExportTextFiles();

            foreach (var item in tempFiles)
            {
                File.Delete(item);
            }
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
            dmi = new Reclamation.Riverware.HydrometDMI(HydrometHost.PNLinux, cf, t1, t2);
            dmi.ExportTextFiles();

            foreach (var item in tempFiles)
            {
                File.Delete(item);
            }
        }
    }
}
