using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;

namespace Reclamation.Riverware
{
    /// <summary>
    /// Imports daily excel time-series data
    /// into riverware as a dmi
    /// Either imports all data in your spreadsheet
    /// or imports a single year for multiple year trace type run 
    /// Karl Tarbet October 2006
    /// </summary>
    public class HydrometDMI
    {
        ControlFile controlFile1;
        string[] pcode;
        string[] cbtt;
        string[] filename;
        int[] daysOffset;
        int[] dayCount;
        int[] slot_offset;
        bool[] hasCount;
        DateTime startDate;
        DateTime endDate;
        HydrometHost server;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="server">either pnhyd0 or yakhyd</param>
        /// <param name="controlFilename">riverware control file</param>
        /// <param name="startDate">riverware simulation start date</param>
        public HydrometDMI(HydrometHost server, string controlFilename, 
            DateTime startDate, DateTime endDate)
        {
            controlFile1 = new ControlFile(controlFilename);
            this.startDate = startDate;
            this.endDate = endDate;
            this.server = server;
        }

        public void ExportTextFiles()
        {
            ParseControlFile();
            ReadFromHydromet();
        }

        private void ReadFromHydromet()
        {
            //DateTime t1 = startDate.AddDays(minOffset); // usually negative..

            SeriesList list = new SeriesList();
            for (int i = 0; i < cbtt.Length; i++)
            {
                HydrometDailySeries s =
                    new HydrometDailySeries(cbtt[i], pcode[i], this.server);
                DateTime t1 = startDate.AddDays(daysOffset[i]);
                DateTime t2 = t1.AddDays(dayCount[i] - 1);

                if (!hasCount[i])
                {
                    t2 = endDate;
                }
                
                if (dayCount[i] < 1 && hasCount[i])
                {
                    Console.WriteLine("Warning: The number of days requested was " + dayCount[i] + "from hydromet");
                }

                s.Read(t1, t2);
                if (s.Count < dayCount[i] && hasCount[0])
                {
                    Console.WriteLine("Warning: the requested hydromet data is missing.");
                }

                list.Add(s);
            }
            

            //Reclamation.PNHydromet.HydrometDaily.BulkRead(cbtt, pcode, t1, startDate, false,server);

            //FormPrevew p = new FormPrevew(list);
            //p.ShowDialog();

            WriteToRiverwareFiles(list);


        }

        private void WriteToRiverwareFiles(SeriesList list)
        {
            for(int i=0; i<list.Count; i++)
            {
                Series s = list[i];
                File.Delete(filename[i]);
                if (s.Count <= 0)
                    continue;
                StreamWriter sw = new StreamWriter(filename[i]);
                sw.WriteLine("# this data was imported from Hydromet "+DateTime.Now.ToString() );
                sw.WriteLine("# " + cbtt[i] + " " + pcode[i]);

                sw.WriteLine("start_date: " + s[0].DateTime.AddDays(slot_offset[i]).ToString("yyyy-MM-dd") + " 24:00");

                for (int j = 0; j < s.Count; j++)
                {
                    if (s[j].IsMissing)
                    {
                        Console.WriteLine(cbtt[i]+" "+pcode[i] + " Error: missing data " + s[j].ToString());
                        sw.WriteLine("NaN");
                    }
                    else
                    {
                        sw.WriteLine(s[j].Value);
                    }
                }
                sw.Close();

            }
        }

        private void ParseControlFile()
        {
            pcode = new string[controlFile1.Length];
            cbtt = new string[controlFile1.Length];
            filename = new string[controlFile1.Length];
            daysOffset = new int[controlFile1.Length];
            dayCount = new int[controlFile1.Length];
            slot_offset = new int[controlFile1.Length];
            hasCount = new bool[controlFile1.Length];
            for (int i = 0; i < controlFile1.Length; i++)
            {
                controlFile1.TryParse(i, "file", out filename[i]);
                File.Delete(filename[i]);
                controlFile1.TryParse(i, "cbtt", out cbtt[i]);
                controlFile1.TryParse(i, "pcode", out pcode[i]);
                controlFile1.TryParse(i, "days_offset", out daysOffset[i], 0);
                controlFile1.TryParse(i, "slot_offset", out slot_offset[i], 0);

                hasCount[i] = controlFile1.TryParse(i, "count", out dayCount[i], -1, true);
                
            }
        }
    }
}
