using System;
using System.Text.RegularExpressions;
using System.Data;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Excel
{
    [Obsolete("try ExcelDataReaderSeries instead of ExcelReaderSeries1.")]
    public class ExcelReaderSeries1 : Series
    {
        string filename;
        string sheetName;
        string dateColumn;
        string valueColumn;


        /// <summary>
        /// Create Series that references data in Excel
        /// </summary>
        /// <param name="excelFilename"></param>
        /// <param name="sheetName"></param>
        /// <param name="dateColumn"></param>
        /// <param name="valueColumn"></param>
        public ExcelReaderSeries1(string filename, string sheetName, 
            string dateColumn, string valueColumn)
        {
            this.filename = filename;
            this.sheetName = sheetName;
            this.dateColumn = dateColumn;
            this.valueColumn = valueColumn;
            this.Name = valueColumn;
        }

        public ExcelReaderSeries1(string connectionString)
        {
            this.filename = ConnectionStringUtility.GetToken(connectionString, "file","");
            this.sheetName = ConnectionStringUtility.GetToken(connectionString, "sheetName","");
            this.dateColumn = ConnectionStringUtility.GetToken(connectionString, "dateColumn","");
            this.valueColumn = ConnectionStringUtility.GetToken(connectionString, "valueColumn","");
            this.Name = valueColumn;
        }
        

        protected override void ReadCore(DateTime t1, DateTime t2)
        {

           DataTable tbl = Reclamation.Core.ExcelUtility.Read(filename, sheetName,true);
           tbl = RemoveDuplicates(tbl,t1,t2);

           TimeInterval ti = TimeInterval.Daily;
           if (tbl.Rows.Count > 1)
           {
               DateTime tr0 = Convert.ToDateTime(tbl.Rows[0][0]);
               DateTime tr1 = Convert.ToDateTime(tbl.Rows[1][0]);

               if (tr0.AddDays(1) == tr1)
                  ti = Reclamation.TimeSeries.TimeInterval.Daily;
               else
                   if (tr0.AddMonths(1) == tr1)
                       this.TimeInterval = Reclamation.TimeSeries.TimeInterval.Monthly;
                   else
                       this.TimeInterval = Reclamation.TimeSeries.TimeInterval.Irregular;

           }
           base.InitTimeSeries(tbl, "", ti, true);
        }

        private DataTable RemoveDuplicates(DataTable tbl,DateTime t1, DateTime t2)
        {
            DataTable rval = new DataTable("excel");
            rval.Columns.Add("DateTime", typeof(DateTime));
            rval.Columns.Add("Value", typeof(double));
            DateTime prevDate = DateTime.MinValue;
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                DataRow r = tbl.Rows[i];
                if (r[0] == DBNull.Value || r[1] == DBNull.Value)
                    continue;
                DateTime dout;
//                string date1 = DateTime.FromOADate(Convert.ToDouble(dr.ItemArray[0]ToString())).ToString();


                if (!DateTime.TryParse(r[0].ToString(), out dout))
                {
                    continue;
                }

                DateTime d = Convert.ToDateTime(r[0]);

                if (d < t1 || d > t2)
                {
                    continue;
                }

                if (d == prevDate)
                {
                    Logger.WriteLine("Skipped duplicate date " + d.ToString());
                    continue;
                }
                prevDate = d;


                rval.Rows.Add(d, r[1]);

            }
            return rval;
        }

        

    }
}
