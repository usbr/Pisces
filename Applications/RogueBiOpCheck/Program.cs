using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reclamation.Core;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using System.IO;

namespace RogueBiOpCheck
{
    class Program
    {
        private static string roguePath = Directory.GetCurrentDirectory();

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Usage();
                Console.WriteLine("ERROR: wrong number of program arguments");
                return;
            }

            var outFile = Path.GetFullPath(args[0]);
            if (!Directory.Exists(Path.GetDirectoryName(outFile)))
            {
                Usage();
                Console.WriteLine("ERROR: directory to write pisces file not found:");
                Console.WriteLine(Path.GetDirectoryName(outFile));
                return;
            }

            DateTime t1;
            if (!DateTime.TryParse(string.Format("1/1/{0} 00:00:00", args[1]), out t1))
            {
                Usage();
                Console.WriteLine("ERROR: unable to parse date from CY provided - " + args[1]);
                return;
            }

            DateTime t2;
            if (!DateTime.TryParse(string.Format("12/31/{0} 23:59:59", args[1]), out t2))
            {
                Usage();
                Console.WriteLine("ERROR: unable to parse date from CY provided - " + args[1]);
                return;
            }

            ProcessRogueBiOP(t1, t2, outFile);
        }

        static void Usage()
        {
            Console.WriteLine("This routine checks Rogue River basin gages for compliance with BiOp thresholds.");
            Console.WriteLine("");
            Console.WriteLine("Please clean up the 15-minute data for the following gages before initiating");
            Console.WriteLine("this routine:");
            Console.WriteLine("  EMI Q,  EMI QC,  BASO Q,  TALO QC,   BCTO Q,  PHXO Q,  GILO Q");
            Console.WriteLine("GILO GH, DICO QC,  SLBO QC,  ANTO Q,  ANTO QC");
            Console.WriteLine("");
            Console.WriteLine("The routine converts the 15-minute data to hourly and performs the checks");
            Console.WriteLine("against the hourly datasets.");
            Console.WriteLine("");
            Console.WriteLine("Please keep in mind that it takes a while to run this routine (several hours");
            Console.WriteLine("for a year's worth of data).");
            Console.WriteLine("");
            Console.WriteLine("See the 'ProgramDetails.doc' textfile for details about calculation procedures");
            Console.WriteLine("and outputs");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Usage: RogueBiOpCheck.exe  output.pdb  CY");
            Console.WriteLine("Where:");
            Console.WriteLine("       output.pdb is the Pisces output file to create");
            Console.WriteLine("       CY is the complete calender year of data to run");
            Console.WriteLine("");
        }

        // ENTRY POINT FOR PROCESSING
        static void ProcessRogueBiOP(DateTime t1, DateTime t2, string piscesFile)
        {
            // Toggle to read flagged data
            HydrometInstantSeries.KeepFlaggedData = true;

            // Create pisces database to store data
            if (File.Exists(piscesFile))
                File.Delete(piscesFile);
            var DB = new SQLiteServer(piscesFile);
            var pDB = new TimeSeriesDatabase(DB);

            // PROCESS INSTANT DATA
            PiscesFolder rFldr = pDB.AddFolder("RawData");
            Console.Write("Processing Instant Series... ");
            var emiQ = GetInstantSeries("EMI", "Q", t1, t2, pDB, rFldr);
            var emiQC = GetInstantSeries("EMI", "QC", t1, t2, pDB, rFldr);

            var basoQ = GetInstantSeries("BASO", "Q", t1, t2, pDB, rFldr);
            var taloQC = GetInstantSeries("TALO", "QC", t1, t2, pDB, rFldr);
            var bctoQ = GetInstantSeries("BCTO", "Q", t1, t2, pDB, rFldr);
            var phxoQC = GetInstantSeries("PHXO", "QC", t1, t2, pDB, rFldr);

            var giloQ = GetInstantSeries("GILO", "Q", t1, t2, pDB, rFldr);
            var giloGH = GetInstantSeries("GILO", "GH", t1, t2, pDB, rFldr);
            var dicoQC = GetInstantSeries("DICO", "QC", t1, t2, pDB, rFldr);
            var slboQC = GetInstantSeries("SLBO", "QC", t1, t2, pDB, rFldr);

            var antoQ = GetInstantSeries("ANTO", "Q", t1, t2, pDB, rFldr);
            var antoQC = GetInstantSeries("ANTO", "QC", t1, t2, pDB, rFldr);
            var antoGH = GetInstantSeries("ANTO", "GH", t1, t2, pDB, rFldr);

            Console.WriteLine("Done importing instant data!");

            // PROCESS HOURLY DATA
            PiscesFolder dFldr = pDB.AddFolder("HourlyData");
            Console.WriteLine("");
            Console.Write("Processing Hourly Series... ");
            var emiQ_h = ProcessHourlySeries(emiQ, "EMI_Q", pDB, dFldr);
            var emiQC_h = ProcessHourlySeries(emiQC, "EMI_QC", pDB, dFldr);

            var basoQ_h = ProcessHourlySeries(basoQ, "BASO_Q", pDB, dFldr);
            var taloQC_h = ProcessHourlySeries(taloQC, "TALO_QC", pDB, dFldr);
            var bctoQ_h = ProcessHourlySeries(bctoQ, "BCTO_Q", pDB, dFldr);
            var phxoQC_h = ProcessHourlySeries(phxoQC, "PHXO_QC", pDB, dFldr);

            var giloQ_h = ProcessHourlySeries(giloQ, "GILO_Q", pDB, dFldr);
            var giloGH_h = ProcessHourlySeries(giloGH, "GILO_GH", pDB, dFldr);
            var dicoQC_h = ProcessHourlySeries(dicoQC, "DICO_QC", pDB, dFldr);
            var slboQC_h = ProcessHourlySeries(slboQC, "SLBO_QC", pDB, dFldr);

            var antoQ_h = ProcessHourlySeries(antoQ, "ANTO_Q", pDB, dFldr);
            var antoQC_h = ProcessHourlySeries(antoQC, "ANTO_QC", pDB, dFldr);
            var antoGH_h = ProcessHourlySeries(antoGH, "ANTO_GH", pDB, dFldr);

            Console.WriteLine("Done computing hourly data!");



            // CHECK BIOP STUFF
            Console.WriteLine("");
            Console.WriteLine("Data Processing: Checking Ramping Rates and Flows...");
            Console.WriteLine("");
            PiscesFolder ckFldr = pDB.AddFolder("RampingRateChecks");

            Console.WriteLine("Checking EMI flows");
            Series EMIHourlyDownRamp = CheckEMIHourlyDownRampingRate(emiQ_h);
            EMIHourlyDownRamp = CheckSourceSeries(emiQ_h, EMIHourlyDownRamp);
            pDB.AddSeries(EMIHourlyDownRamp, ckFldr);

            Series EMIDailyDownRamp = CheckEMIDailyDownRampingRate(emiQ_h);
            EMIDailyDownRamp = CheckSourceSeries(emiQ_h, EMIDailyDownRamp);
            pDB.AddSeries(EMIDailyDownRamp, ckFldr);

            Series EMIHourlyUpRamp = CheckEMIUpRampingRate(emiQ_h);
            EMIHourlyUpRamp = CheckSourceSeries(emiQ_h, EMIHourlyUpRamp);
            pDB.AddSeries(EMIHourlyUpRamp, ckFldr);

            Console.WriteLine("Checking BASO flows");
            Series BASOHourlyDownRamp = CheckBASODownRampingRate(basoQ_h, taloQC_h);
            BASOHourlyDownRamp = CheckSourceSeries(basoQ_h, BASOHourlyDownRamp);
            BASOHourlyDownRamp = CheckSourceSeries(taloQC_h, BASOHourlyDownRamp);
            pDB.AddSeries(BASOHourlyDownRamp, ckFldr);

            Console.WriteLine("Checking BCTO flows");
            Series BCTOHourlyDownRamp = CheckBCTODownRampingRate(bctoQ_h, phxoQC_h);
            BCTOHourlyDownRamp = CheckSourceSeries(bctoQ_h, BCTOHourlyDownRamp);
            BCTOHourlyDownRamp = CheckSourceSeries(phxoQC_h, BCTOHourlyDownRamp);
            pDB.AddSeries(BCTOHourlyDownRamp, ckFldr);

            Console.WriteLine("Checking GILO flows and gage height");
            Series GILOUpRamp = new Series(); Series GILODownRamp = new Series();
            CheckGILOFlowRampingRate(giloQ_h, slboQC_h, dicoQC_h, out GILODownRamp, out GILOUpRamp);
            GILODownRamp = CheckSourceSeries(giloQ_h, GILODownRamp);
            GILODownRamp = CheckSourceSeries(slboQC_h, GILODownRamp);
            GILODownRamp = CheckSourceSeries(dicoQC_h, GILODownRamp);
            GILOUpRamp = CheckSourceSeries(giloQ_h, GILOUpRamp);
            GILOUpRamp = CheckSourceSeries(slboQC_h, GILOUpRamp);
            GILOUpRamp = CheckSourceSeries(dicoQC_h, GILOUpRamp);
            pDB.AddSeries(GILOUpRamp, ckFldr);
            pDB.AddSeries(GILODownRamp, ckFldr);

            Series GILOGageUpRamp = CheckGILOGageRampingRate(giloGH_h, slboQC_h, dicoQC_h);
            GILOGageUpRamp = CheckSourceSeries(giloGH_h, GILOGageUpRamp);
            GILOGageUpRamp = CheckSourceSeries(slboQC_h, GILOGageUpRamp);
            GILOGageUpRamp = CheckSourceSeries(dicoQC_h, GILOGageUpRamp);
            pDB.AddSeries(GILOGageUpRamp, ckFldr);

            Console.WriteLine("Checking ANTO flows and gage height");
            Series ANTOUpRamp = new Series(); Series ANTODownRamp = new Series();
            CheckANTOFlowRampingRate(antoQ_h, antoQC_h, out ANTODownRamp, out ANTOUpRamp);
            ANTODownRamp = CheckSourceSeries(antoQ_h, ANTODownRamp);
            ANTODownRamp = CheckSourceSeries(antoQC_h, ANTODownRamp);
            ANTOUpRamp = CheckSourceSeries(antoQ_h, ANTOUpRamp);
            ANTOUpRamp = CheckSourceSeries(antoQC_h, ANTOUpRamp);
            pDB.AddSeries(ANTOUpRamp, ckFldr);
            pDB.AddSeries(ANTODownRamp, ckFldr);

            Series ANTOGageUpRamp = CheckANTOGageRampingRate(antoGH_h, antoQC_h);
            ANTOGageUpRamp = CheckSourceSeries(antoGH_h, ANTOGageUpRamp);
            ANTOGageUpRamp = CheckSourceSeries(antoQC_h, ANTOGageUpRamp);
            pDB.AddSeries(ANTOGageUpRamp, ckFldr);
        }

        // Populates Pisces DB with instant 15-minute data
        private static Series GetInstantSeries(string CBTT, string PCODE, DateTime t1, DateTime t2, TimeSeriesDatabase pDB,
            PiscesFolder rFldr)
        {
            Console.Write(CBTT + "_" + PCODE + ", ");
            Series s = new HydrometInstantSeries(CBTT, PCODE);
            s.Read(t1, t2);
            s.Name = CBTT + "_" + PCODE + "15min";
            pDB.AddSeries(s, rFldr);
            return s;
        }

        // Does the conversion from an instant series to an hourly series.
        private static Series ProcessHourlySeries(Series sIn, string sName, TimeSeriesDatabase pDB, PiscesFolder dFldr)
        {
            Console.Write(sName + ", ");
            // Hourly averaging
            Series s3 = Reclamation.TimeSeries.Math.Average(sIn, TimeInterval.Hourly);
            s3.Provider = "Series";
            s3.Name = sName;
            pDB.AddSeries(s3, dFldr);
            return s3;
        }

        // Clears a violation if the series that is being checked has a missing value for the previous and current time-step
        private static Series CheckSourceSeries(Series sourceS, Series checkS)
        {
            Series checkNewS = checkS.Clone();
            for (int i = 1; i < checkS.Count(); i++)
            {
                DateTime tprev = checkS[i - 1].DateTime;
                DateTime t = checkS[i].DateTime;

                if (checkS[t].Value < 0.0)
                {
                    double checkVal1 = 0.0; 
                    double checkVal2 = 0.0;
                    try
                    { 
                        checkVal1 = sourceS[tprev].Value; 
                        checkVal2 = sourceS[t].Value; 
                    }
                    catch
                    { 
                        checkVal1 = -99.0; 
                        checkVal2 = -99.0; 
                    }

                    if (checkVal1 < 0.0 || checkVal2 < 0.0)
                        checkNewS.Add(t, 0.0);
                    else
                        checkNewS.Add(checkS[t]);
                }
                else
                { 
                    checkNewS.Add(checkS[t]); 
                }
            }
            return checkNewS;
        }

        // Checks hourly GILO flow against hourly SLBO and DICO canal flows
        private static void CheckGILOFlowRampingRate(Series GILO, Series SLBO, Series DICO, out Series sOut_Down,
            out Series sOut_Up)
        {
            sOut_Down = new Series();
            sOut_Up = new Series();
            sOut_Down.Name = "GILO_DailyDownRampingCheck";
            sOut_Down.Provider = "Series";
            sOut_Up.Name = "GILO_DailyUpRampingCheck";
            sOut_Up.Provider = "Series";

            for (int i = 0; i < GILO.Count - 24; i++)
            {
                DateTime t1 = GILO[i].DateTime;
                DateTime t2 = t1.AddHours(23);
               
                // Calculate GILO change within a 24 hour period
                double ithQ = GILO[t1].Value;
                double jthQ = GILO[t2].Value;
                double rampQ = jthQ - ithQ;
                
                // Calculate total change in SLBO and DICO flows within the same period
                double ithQ1 = 0;
                double jthQ1 = 0;
                if (SLBO.IndexOf(t1) > 0)
                    double.TryParse(SLBO[t1].Value.ToString(), out ithQ1);
                if (SLBO.IndexOf(t2) > 0)
                    double.TryParse(SLBO[t2].Value.ToString(), out jthQ1);
                double rampQ1 = jthQ1 - ithQ1;
                
                double ithQ2 = 0;
                double jthQ2 = 0;
                if (DICO.IndexOf(t1) > 0)
                    double.TryParse(DICO[t1].Value.ToString(), out ithQ2);
                if (DICO.IndexOf(t2) > 0)
                    double.TryParse(DICO[t2].Value.ToString(), out jthQ2);
                double rampQ2 = jthQ2 - ithQ2;
                double rampImpact = rampQ1 + rampQ2;

                // Check if GILO Q increase is more than 100%
                if (jthQ > 2.0 * ithQ && rampImpact < 0.0)
                {
                    if (System.Math.Abs(rampImpact) < rampQ)
                    {
                        // Assigns -50 if GILO increase is not due to decrease in DICO and SLBO operations
                        sOut_Up.Add(t2, -50.0);
                    }
                    else
                    {
                        // Assigns a -99 if increase is due to change in DICO and SLBO operations
                        sOut_Up.Add(t2, -99.99); 
                    }
                }
                else
                { 
                    // Assigns a 0 if GILO flow did not increase in the last 24 hours
                    sOut_Up.Add(t2, 0.0); 
                } 

                // Check if GILO Q decrease is more than 50%; same comments as above if-then conditionals
                if (jthQ < 0.5 * ithQ && rampImpact > 0.0)
                {
                    if (System.Math.Abs(rampQ) > rampImpact)
                        sOut_Down.Add(t2, -50.0);
                    else
                        sOut_Down.Add(t2, -99.99);
                }
                else
                { 
                    sOut_Down.Add(t2, 0.0); 
                }
            }
        }

        // Check GILO GH ramping rate against SLBO and DICO flows
        private static Series CheckGILOGageRampingRate(Series GILOgh, Series SLBO, Series DICO)
        {
            Series sOut = new Series();
            sOut.Name = "GILO_HourlyGageHeightRampingCheck";
            sOut.Provider = "Series";

            for (int i = 0; i < GILOgh.Count - 1; i++)
            {
                DateTime t1 = GILOgh[i].DateTime;
                DateTime t2 = GILOgh[i + 1].DateTime;
                
                // Get GH average and difference for processing
                double ghDiff = (GILOgh[t2].Value - GILOgh[t1].Value) * 12.0;//Convert to inches
                double ghAvg = ((GILOgh[t2].Value + GILOgh[t1].Value) * 12.0) / 2.0;
                
                // Get change in DICO and SLBO flows
                double projQDiff = (SLBO[t2].Value - SLBO[t1].Value) + (DICO[t2].Value - DICO[t1].Value);

                // Data calculated from GILO Rating Table. The flow required to cause a 1" change in GH is estimated below.
                // The calculation process is dependent on the average GH for the time step and the total change 
                // in flow from both DICO and SLBO.
                // As an example, if the GH between two consecutive hours increased by the allowable 1" threshold and the 
                // current average GH between the hourly readings is 16", the table below shows that a change in flow of 12.53cfs
                // would cause the GH to change by 1". If the calculated change in total flow is >12.53cfs, then we can say 
                // that the change in flow caused the increase in GH. Same calculation for the 2" threshold but using 2X the table 
                // 'CFS / INCH' value.
                //
                //FEET		CFS	 	   INCH			CFS / INCH	 	   	APPROX CFS / INCH
                //0.22		00.0		2.64		
                //0.50		12.4		6.00		3.69				4
                //1.00		62.8		12.0		8.4	        	   	8
                //1.50		138.		18.0		12.53			   	12      <-- Values used in example
                //2.00		236.		24.0		16.33			   	16
                //2.50		351.		30.0		19.16			   	20
                //3.00		481.		36.0		21.66	
                //
                // Determine the 'CFS / INCH' value that would cause a 1" change in GH
                //var a = rtfInterpolate(3.0, 2.5, 2.75, 481.0, 351.0);
                double maxQDiff = 0.0;
                if (ghAvg <= 6.0)
                    maxQDiff = 3.69;
                else if (ghAvg > 6.0 && ghAvg <= 12.0)
                    maxQDiff = rtfInterpolate(12.0, 6.0, ghAvg, 8.4, 3.69);
                else if (ghAvg > 12.0 && ghAvg <= 18.0)
                    maxQDiff = rtfInterpolate(18.0, 12.0, ghAvg, 12.53, 8.4);
                else if (ghAvg > 18.0 && ghAvg <= 24.0)
                    maxQDiff = rtfInterpolate(24.0, 18.0, ghAvg, 16.33, 12.53);
                else if (ghAvg > 24.0 && ghAvg <= 30.0)
                    maxQDiff = rtfInterpolate(30.0, 24.0, ghAvg, 19.16, 16.33);
                else if (ghAvg > 30.0 && ghAvg <= 36.0)
                    maxQDiff = rtfInterpolate(36.0, 30.0, ghAvg, 21.66, 19.16);
                else if (ghAvg > 36.0)
                    maxQDiff = 21.66;

                // If month is March or April, GH threshold is 1"
                if (t1.Month == 3 || t1.Month == 4)
                {
                    if (ghDiff < -1.0) // Check if GH dropped by more than 1"
                    {
                        if (projQDiff > maxQDiff) // GH dropped by >1", check if change in project flows caused it.
                            sOut.Add(t2, -99.99); // Change in flow > allowable, assign -99.
                        else
                            sOut.Add(t2, -50.00); // Change in flow < allowable, assign -50.
                    }
                    else // GH didn't drop by more than 1". Assign 0.0
                    { 
                        sOut.Add(t2, 0.00); 
                    }
                }
                // All other months have a threshold of 2"
                else
                {
                    if (ghDiff < -2.0) // Check if GH dropped by more than 2"
                    {
                        if (projQDiff > 2.0 * maxQDiff)
                            sOut.Add(t2, -99.99);
                        else
                            sOut.Add(t2, -50.00);
                    }
                    else
                    { 
                        sOut.Add(t2, 0.00); 
                    }
                }

            }
            return sOut;
        }

        // Check BASO flow against TALO flow increase
        private static Series CheckBASODownRampingRate(Series BASO, Series TALO)
        {
            Series sOut = new Series();

            for (int i = 0; i < BASO.Count - 1; i++)
            {
                DateTime t1Temp = BASO[i].DateTime;
                DateTime t2Temp = BASO[i + 1].DateTime;

                double BASOithQ = BASO[t1Temp].Value;
                double BASOjthQ = BASO[t2Temp].Value;
                
                // Get diversion canal flow change (rampQ)
                double TALOithQ = 0.0;
                double TALOjthQ = 0.0;
                if (TALO.IndexOf(t1Temp) > 0)
                    double.TryParse(TALO[t1Temp].Value.ToString(), out TALOithQ);
                if (TALO.IndexOf(t2Temp) > 0)
                    double.TryParse(TALO[t2Temp].Value.ToString(), out TALOjthQ);
                double rampQ = TALOjthQ - TALOithQ;

                // Check diversion canal flow change against allowable threshold based on stream flow
                // Assign -99 if diversion increase violates threshold and 0.0 if not in violation
                if ((BASOjthQ < 20.0) && rampQ > 5.0)
                    sOut.Add(t2Temp, -99.99);
                else if ((BASOjthQ >= 20.0 && BASOjthQ < 70.0) && rampQ > 10.0)
                    sOut.Add(t2Temp, -99.99);
                else if ((BASOjthQ >= 70.0) && rampQ > 20.0)
                    sOut.Add(t2Temp, -99.99);
                else
                    sOut.Add(t2Temp, 0.0);
            }
            sOut.Name = "BASO_HourlyDownRampingCheck";
            sOut.Provider = "Series";
            return sOut;
        }

        // Check BCTO flow against PHXO flow increase
        private static Series CheckBCTODownRampingRate(Series BCTO, Series PHXO)
        {
            Series sOut = new Series();

            for (int i = 0; i < BCTO.Count - 1; i++)
            {
                DateTime t1Temp = BCTO[i].DateTime;
                DateTime t2Temp = BCTO[i + 1].DateTime;

                double BCTOithQ = BCTO[t1Temp].Value;
                double BCTOjthQ = BCTO[t2Temp].Value;

                // Get diversion canal flow change (rampQ)
                double PHXOithQ = 0.0;
                double PHXOjthQ = 0.0;
                if (PHXO.IndexOf(t1Temp) > 0)
                    double.TryParse(PHXO[t1Temp].Value.ToString(), out PHXOithQ);
                if (PHXO.IndexOf(t2Temp) > 0)
                    double.TryParse(PHXO[t2Temp].Value.ToString(), out PHXOjthQ);
                double rampQ = PHXOjthQ - PHXOithQ;

                // Check diversion canal flow change against allowable threshold based on stream flow
                // Assign -99 if diversion increase violates threshold and 0.0 if not in violation
                if ((BCTOjthQ < 20.0) && rampQ > 5.0)
                    sOut.Add(t2Temp, -99.99);
                else if ((BCTOjthQ >= 20.0 && BCTOjthQ < 80.0) && rampQ > 10.0)
                    sOut.Add(t2Temp, -99.99);
                else if ((BCTOjthQ >= 80.0) && rampQ > 20.0)
                    sOut.Add(t2Temp, -99.99);
                else
                    sOut.Add(t2Temp, 0.0);
            }
            sOut.Name = "BCTO_HourlyDownRampingCheck";
            sOut.Provider = "Series";
            return sOut;
        }

        private static bool IsIrrigationSeason(DateTime t)
        {
            // Irrigation Season defined as 4/1 - 10/15
            if (t >= new DateTime(t.Year, 4, 1, 0, 0, 0) && t <= new DateTime(t.Year, 10, 15, 0, 0, 0))
                return true;
            else
                return false;
        }

        // Check EMI outflow increase ramping rate
        private static Series CheckEMIUpRampingRate(Series emiQ)
        {
            Series sOut = new Series();

            for (int i = 0; i < emiQ.Count - 1; i++)
            {
                if (IsIrrigationSeason(emiQ[i + 1].DateTime))
                {
                    // Get EMI hourly outflow increase
                    double ithQ = emiQ[i].Value;
                    double jthQ = emiQ[i + 1].Value;
                    double rampQ = jthQ - ithQ;

                    // Check previous hour's flow and the flow increase against the allowable increase threshold
                    // Assign -99 if in violation and 0.0 if not in violation
                    if ((ithQ >= 2.0 && ithQ <= 6.0) && rampQ > 8.0)
                        sOut.Add(emiQ[i + 1].DateTime, -99.99);
                    else if ((ithQ > 6.0 && ithQ <= 20.0) && rampQ > 10.0)
                        sOut.Add(emiQ[i + 1].DateTime, -99.99);
                    else if ((ithQ > 20.0 && ithQ <= 40.0) && rampQ > 15.0)
                        sOut.Add(emiQ[i + 1].DateTime, -99.99);
                    else if ((ithQ > 40.0 && ithQ <= 100.0) && rampQ > 20.0)
                        sOut.Add(emiQ[i + 1].DateTime, -99.99);
                    else if ((ithQ > 100.0) && rampQ > 30.0)
                        sOut.Add(emiQ[i + 1].DateTime, -99.99);
                    else
                        sOut.Add(emiQ[i + 1].DateTime, 0.0);
                }
                else 
                { 
                    sOut.Add(emiQ[i + 1].DateTime, 0.0); 
                }
            }
            sOut.Name = "EMI_HourlyUpRampingCheck";
            sOut.Provider = "Series";
            return sOut;
        }

        // Check EMI daily outflow decrease ramping rate
        private static Series CheckEMIDailyDownRampingRate(Series emiQ)
        {
            Series sOut = new Series();
            Series dailyAvg = Reclamation.TimeSeries.Math.Average(emiQ, TimeInterval.Daily);

            for (int i = 0; i < dailyAvg.Count - 1; i++)
            {
                double ithQ = dailyAvg[i].Value;
                double jthQ = dailyAvg[i + 1].Value;

                if ((ithQ > 10.0) && (jthQ < 0.5 * ithQ))
                    sOut.Add(dailyAvg[i + 1].DateTime, -99.99);
                else
                    sOut.Add(dailyAvg[i + 1].DateTime, 0.0);
            }
            sOut.Name = "EMI_DailyDownRampingCheck";
            sOut.Provider = "Series";
            return sOut;
        }

        // Check EMI hourly outflow decrease ramping rate
        private static Series CheckEMIHourlyDownRampingRate(Series emiQ)
        {
            Series sOut = new Series();

            for (int i = 0; i < emiQ.Count - 1; i++)
            {
                double ithQ = emiQ[i].Value;
                double jthQ = emiQ[i + 1].Value;
                double rampQ = jthQ - ithQ;

                if ((ithQ <= 10.0) && (rampQ < -5.0))
                    sOut.Add(emiQ[i + 1].DateTime, -99.99);
                else
                    sOut.Add(emiQ[i + 1].DateTime, 0.0);
            }
            sOut.Name = "EMI_HourlyDownRampingCheck";
            sOut.Provider = "Series";
            return sOut;
        }

        // Interpolates a middle point between two points
        private static double rtfInterpolate(double x1, double x2, double xMid, double y1, double y2)
        {
            // Simple triangular interpolation
            double slope = (y2 - y1) / (x2 - x1);
            double xInc = xMid - x1;
            double ymid = slope * xInc + y1;

            return ymid;
        }

        // Checks hourly ANTO flow against hourly ANTO canal flows
        private static void CheckANTOFlowRampingRate(Series antoQ, Series antoQC, out Series sOut_Down,
            out Series sOut_Up)
        {
            sOut_Down = new Series();
            sOut_Up = new Series();
            sOut_Down.Name = "ANTO_DailyDownRampingCheck";
            sOut_Down.Provider = "Series";
            sOut_Up.Name = "ANTO_DailyUpRampingCheck";
            sOut_Up.Provider = "Series";

            for (int i = 0; i < antoQ.Count - 24; i++)
            {
                DateTime t1 = antoQ[i].DateTime;
                DateTime t2 = t1.AddHours(23);

                // Calculate GILO change within a 24 hour period
                double ithQ = antoQ[t1].Value;
                double jthQ = antoQ[t2].Value;
                double rampQ = jthQ - ithQ;

                // Calculate total change in SLBO and DICO flows within the same period
                double ithQ1 = 0.0;
                double jthQ1 = 0.0;
                if (antoQC.IndexOf(t1) > 0)
                    double.TryParse(antoQC[t1].Value.ToString(), out ithQ1);
                if (antoQC.IndexOf(t2) > 0)
                    double.TryParse(antoQC[t2].Value.ToString(), out jthQ1);
                double rampImpact = jthQ1 - ithQ1;

                // Check if ANTO Q increase is more than 100%
                if (jthQ > 2.0 * ithQ && rampImpact < 0.0 && ithQ > 1.0 && jthQ > 1.0)
                {
                    if (System.Math.Abs(rampImpact) < rampQ)
                        sOut_Up.Add(t2, -50.0); // Assigns -50 if GILO increase is not due to decrease in DICO and SLBO operations
                    else
                        sOut_Up.Add(t2, -99.99); // Assigns a -99 if increase is due to change in DICO and SLBO operations
                }
                else
                {
                    // Assigns a 0 if GILO flow did not increase in the last 24 hours
                    sOut_Up.Add(t2, 0.0); 
                } 

                // Check if ANTO Q decrease is more than 50%; same comments as above if-then conditionals
                if (jthQ < 0.5 * ithQ && rampImpact > 0.0 && ithQ > 1.0 && jthQ > 1.0)
                {
                    if (System.Math.Abs(rampQ) > rampImpact)
                        sOut_Down.Add(t2, -50.0);
                    else
                        sOut_Down.Add(t2, -99.99);
                }
                else
                { 
                    sOut_Down.Add(t2, 0.0); 
                }
            }
        }

        // Check ANTO GH ramping rate against ANTO canal flows
        private static Series CheckANTOGageRampingRate(Series antoGH, Series antoQC)
        {
            Series sOut = new Series();
            sOut.Name = "ANTO_HourlyGageHeightRampingCheck";
            sOut.Provider = "Series";

            for (int i = 0; i < antoGH.Count - 1; i++)
            {
                DateTime t1 = antoGH[i].DateTime;
                DateTime t2 = antoGH[i + 1].DateTime;

                // Get GH average and difference for processing
                double ghDiff = (antoGH[t2].Value - antoGH[t1].Value) * 12.0;//Convert to inches
                double ghAvg = ((antoGH[t2].Value + antoGH[t1].Value) * 12.0) / 2.0;
                
                // Get change in DICO and SLBO flows
                double ithQ = 0;
                double jthQ = 0;
                if (antoQC.IndexOf(t1) > 0)
                    double.TryParse(antoQC[t1].Value.ToString(), out ithQ);
                if (antoQC.IndexOf(t2) > 0)
                    double.TryParse(antoQC[t2].Value.ToString(), out jthQ);
                double projQDiff = jthQ - ithQ;

                // See comments on the GILO Gage Height Ramping Check method for explanation.
                //FEET		CFS	 	    INCH		CFS / INCH	 	   	
                //3.47	    0	        41.64	
                //3.50	    0	        42       	0
                //4.00      0.548	    48	        0.175
                //4.25	    3.67	    51	        1.040666667
                //4.50	    15.3	    54	        3.876666667
                //4.75	    42.2	    57	        8.966666667
                //5.00	    84.3	    60	        14.03333333
                //5.25	    141	        63	        18.9
                //5.50	    211	        66	        23.33333333
                //5.75	    297	        69	        28.66666667
                //5.90  	355	        70.8	    32.22222222
                //
                // Determine the 'CFS / INCH' value that would cause a 1" change in GH
                //var a = rtfInterpolate(3.0, 2.5, 2.75, 481.0, 351.0);
                double maxQDiff = 0.0;
                if (ghAvg <= 48.0)
                    maxQDiff = 0.175;
                else if (ghAvg > 48.0 && ghAvg <= 51.0)
                    maxQDiff = rtfInterpolate(51.0, 48.0, ghAvg, 1.04, 0.175);
                else if (ghAvg > 51.0 && ghAvg <= 54.0)
                    maxQDiff = rtfInterpolate(54.0, 51.0, ghAvg, 3.88, 1.04);
                else if (ghAvg > 54.0 && ghAvg <= 57.0)
                    maxQDiff = rtfInterpolate(57.0, 54.0, ghAvg, 8.97, 3.88);
                else if (ghAvg > 57.0 && ghAvg <= 60.0)
                    maxQDiff = rtfInterpolate(60.0, 57.0, ghAvg, 14.03, 8.97);
                else if (ghAvg > 60.0 && ghAvg <= 63.0)
                    maxQDiff = rtfInterpolate(63.0, 60.0, ghAvg, 18.90, 14.03);
                else if (ghAvg > 63.0 && ghAvg <= 66.0)
                    maxQDiff = rtfInterpolate(66.0, 63.0, ghAvg, 23.33, 18.90);
                else if (ghAvg > 66.0 && ghAvg <= 69.0)
                    maxQDiff = rtfInterpolate(69.0, 66.0, ghAvg, 28.67, 23.33);
                else if (ghAvg > 69.0 && ghAvg <= 70.8)
                    maxQDiff = rtfInterpolate(70.8, 69.0, ghAvg, 32.22, 28.67);
                else if (ghAvg > 70.8)
                    maxQDiff = 32.22;

                // If month is March or April, GH threshold is 1"
                if (t1.Month == 3 || t1.Month == 4)
                {
                    if (ghDiff < -1.0) // Check if GH dropped by more than 1"
                    {
                        if (projQDiff > maxQDiff) // GH dropped by >1", check if change in project flows caused it.
                            sOut.Add(t2, -99.99); // Change in flow > allowable, assign -99.
                        else
                            sOut.Add(t2, -50.00); // Change in flow < allowable, assign -50.
                    }
                    else 
                    {
                        // GH didn't drop by more than 1". Assign 0.0
                        sOut.Add(t2, 0.00); 
                    }
                }
                // All other months have a threshold of 2"
                else
                {
                    if (ghDiff < -2.0) // Check if GH dropped by more than 2"
                    {
                        if (projQDiff > 2.0 * maxQDiff)
                            sOut.Add(t2, -99.99);
                        else
                            sOut.Add(t2, -50.00);
                    }
                    else
                    { 
                        sOut.Add(t2, 0.00); 
                    }
                }

            }
            return sOut;
        }
    }
}
