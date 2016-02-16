using System;
using System.Data;
using System.IO;
using Reclamation.Core;

namespace Reclamation.Riverware
{
    /// <summary>
    /// Exports columns from an excel file to text files
    /// ready to be imported by riverware.
    /// </summary>
    public class ExcelDMI
    {
        string excelName;
        ControlFile controlFile1;
        DateTime t1, t2;
        int maxCount;
        bool usingTraces;

        public ExcelDMI(string excelName, string controlFilename)
        {
            controlFile1 = new ControlFile(controlFilename);
            this.excelName = excelName;
            TimeSpan ts = new TimeSpan(t2.Ticks - t1.Ticks);
        }


        private int traceNumber = -1;
        private DateTime startDate;
        private DateTime _importDate;


        public void ExportTextFiles(DateTime t1, DateTime t2, int traceNumber)
        {
            this.traceNumber = traceNumber;
            usingTraces = traceNumber > 0;
            this.t1 = t1;
            this.t2 = t2;
            TimeSpan ts = new TimeSpan(t2.Ticks - t1.Ticks);
            maxCount = ts.Days + 1 + 60; // get two extra months

            if (!File.Exists(excelName))
            {
                Console.WriteLine("Could not find file '" + excelName + "'");
                return;
            }

            //Console.WriteLine("Found "+tbl.Rows.Count + " rows in "+excelName);

           // cbp.TextFile controlFile = new cbp.TextFile(controlFilename);


            for (int i = 0; i < controlFile1.Length; i++)
            {
                string filename="";
                controlFile1.TryParse(i, "file", out filename);
                
                File.Delete(filename);

                if (controlFile1.OptionExists(i,"WaterYear"))
                { // echo water year to file
                    if (usingTraces)
                    {
                        int wy = _importDate.Year;
                        if (t1.Month >= 10)
                            wy++;
                        //Console.WriteLine("saving " + wy + " to file " + filename);
                        File.WriteAllText(filename, wy.ToString()+"\n");
                    }
                    continue;

                }

                string sheetName = "";
                controlFile1.TryParse(i, "excel_sheet", out sheetName);

                string excelColumn = "";
                controlFile1.TryParse(i, "excel_data_column", out excelColumn);

                string dateColumn = "";
                controlFile1.TryParse(i, "excel_date_column", out dateColumn,"Date");

                if (sheetName == "" || excelColumn == "")
                {
                    continue;
                }


                DataTable tbl = GetTable(excelName, sheetName);

                if (IndexToColumn(tbl, dateColumn) < 0)
                {
                    Console.WriteLine("Error: the date column '" + dateColumn + " could not be found\n please enter !excel_date_column in the control file\n " + controlFile1[i]);
                    continue;
                }

                startDate = Convert.ToDateTime(tbl.Rows[0][dateColumn]);
                _importDate = new DateTime(traceNumber - 1 + startDate.Year, startDate.Month, startDate.Day);
                

                //Reclamation.TimeSeries.Series s = new Reclamation.TimeSeries.Series(tbl, "unknown", Reclamation.TimeSeries.TimeInterval.Daily, true);

                int count = SaveTimeSeries(tbl, filename, excelColumn, dateColumn);

                if (count ==0)
                {
                    Console.WriteLine("Warning: {0} there are {1} days exported from excel but Riverware requested {2} days",excelColumn, count, maxCount);
                }

            }
        }


        private DataSet excelDataSet;
        private DataTable GetTable(string excelName, string sheetName)
        {

           // Performance p = new Performance();
            DataTable rval = new DataTable();
            if (excelDataSet == null)
            {
                excelDataSet = new DataSet();
            }
            if (!excelDataSet.Tables.Contains(sheetName))
            {
                rval = ExcelDB.Read(excelName, sheetName);
                excelDataSet.Tables.Add(rval);
            }

         //  p.Report("read " + sheetName);
            return excelDataSet.Tables[sheetName];
        }

        private int SaveTimeSeries(DataTable tbl, string filename,
           string excelColumn, string dateColumn)
        {
           

            int idx = IndexToColumn(tbl, excelColumn);

            if (idx < 0)
            {
                Console.WriteLine("could not find column '" + excelColumn + "'");
                return 0;
            }

            if (tbl.Rows[0][dateColumn] == DBNull.Value)
            {
                Console.WriteLine("Date is missing " );
                return 0;
            }


            return WriteToFile(tbl, filename, dateColumn, excelColumn);
        }

        private int WriteToFile(DataTable tbl, string filename, string dateColumn, 
            string excelColumn)
        {
            int idx = IndexToColumn(tbl, excelColumn);
            int firstDateIndex = LookupDateIndex(tbl, dateColumn);
            DateTime d = DateTime.MinValue;
            string dateStr = tbl.Rows[firstDateIndex][dateColumn].ToString();
            if (!DateTime.TryParse(dateStr, out d))
            {
                Console.WriteLine("Could not parse date '" + dateStr + " on first row");
                return 0;
            }

            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine("# data was imported from " + this.excelName);
            sw.WriteLine("# sheet " + tbl.TableName);
            sw.WriteLine("# column " + excelColumn);
            sw.WriteLine("# import began on line index: " + firstDateIndex);
            //import=<fixed>|<resize>
            if (usingTraces)
            {
                sw.WriteLine("# This file represents data from the water year beginning in " + _importDate.ToString());
                sw.WriteLine("start_date: " + this.t1.ToString("yyyy-MM-dd") + " 24:00");
            }
            else
            {
                sw.WriteLine("start_date: " + d.ToString("yyyy-MM-dd") + " 24:00");
            }
            int count = 0;

            for (int i = firstDateIndex; i < tbl.Rows.Count; i++)
            {

                if (tbl.Rows[i][idx] == DBNull.Value)
                {
                    sw.WriteLine("NaN");
                }
                else
                {
                    double val = -999;
                    string strValue = tbl.Rows[i][idx].ToString();
                    if (Double.TryParse(strValue, out val))
                    {
                        if (Math.Abs(val + 999) < 0.01)
                        {
                            sw.WriteLine("NaN");
                        }
                        else
                        {
                            sw.WriteLine(val);
                            count++;
                        }
                    }
                    else
                    {
                        Console.WriteLine(strValue);
                        sw.WriteLine("NaN");
                    }
                }
                if (count >= maxCount && usingTraces)
                {
                    break;
                }

            }


            sw.Close();
            //            Console.WriteLine("wrote " + tbl.Rows.Count + " rows");
            return count;
        }

        private int LookupDateIndex(DataTable tbl,  string dateColumn)
        {
            if (!usingTraces)
                return 0; // this will cause everything in excel to be used.

            
            int idx = IndexToImportDate(tbl, dateColumn); // get correct water year
            // seek forward to month/day 
            int i = 0;
            for (i = idx; i < tbl.Rows.Count; i++ )
            {
                DateTime d = Convert.ToDateTime(tbl.Rows[i][dateColumn]);

                if ( (d.Month  == t1.Month) && // match month and day of historical trace
                    ( d.Day == t1.Day) // to this years month and day for simulation dates
                    )
                {
                    return i;
                }
            }

            throw new Exception("Error: The month/day in " + t1.ToString() + " was not found in" + this.excelName);

        }

        private int IndexToImportDate(DataTable tbl, string dateColumn)
        {
            int i = 0;
            for (; i < tbl.Rows.Count; i++)
            {
                DateTime d = Convert.ToDateTime(tbl.Rows[i][dateColumn]);

                if (d == _importDate)
                {
                    return i;
                }
            }
            throw new Exception("Error: The date " + _importDate.ToString() + " was not found in" + this.excelName);
        }

        private static int IndexToColumn(DataTable tbl, string excelColumn)
        {
            int idx = tbl.Columns.IndexOf(excelColumn);

            if (idx < 0)
            {
                idx = tbl.Columns.IndexOf(excelColumn.Replace("_", " "));
            }
            return idx;
        }

        /* sample metaControl read by this program 
Jackson.Outflow: obj=StorageReservoir file=c:\temp\Outflow.Jackson units=cfs scale=1 import=resize
Jackson.Inflow: obj=StorageReservoir file=c:\temp\Inflow.Jackson units=cfs scale=1 import=resize column=jck_inflow
JacksonToPalisades:FlatCreekLocal.Return Flow: obj=Reach file=c:\temp\Return_Flow.JacksonToPalisades_FlatCreekLocal units=cfs scale=1 import=resize


         * */


    }
}
