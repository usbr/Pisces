using System;
using Reclamation.Core;
using System.Configuration;
using System.IO;

namespace Reclamation.TimeSeries.AgriMet
{
    
    
    public partial class CropDatesDataSet 
    {
        partial class CropDatesDataTable
        {
        }


        /// <summary>
        /// Initiate running new crop charts on the server
        /// </summary>
        public static void RunCropCharts()
        {
            int id = DB.RunSqlCommand("select run_cropcharts()");

        }


        public static string GetCropOutputDirectory(int year)
        {
            var agrimetDir = ConfigurationManager.AppSettings["AgriMetCropOutputDirectory"];

            string cropDir = Path.Combine(Path.Combine(agrimetDir, "chart") , year.ToString());

            if (!Directory.Exists(cropDir))
            {
                Logger.WriteLine("Creating Directory: " + cropDir);
                Directory.CreateDirectory(cropDir);
            }

            return cropDir;
        }

       

        private static BasicDBServer DB
        {
            get
            {
                return PostgreSQL.GetPostgresServer("agrimet");
            }
        }

       

        /// <summary>
        /// Gets Crop Dates given a station and a year - Used for Crop Chart generation
        /// </summary>
        /// <param name="cbtt"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static CropDatesDataSet.CropDatesDataTable GetCropDataTable(int year, string[] cbttList)
        {
            var table = new CropDatesDataSet.CropDatesDataTable();
            if (cbttList.Length == 0)
                return table;// nothing to do.

            var sql = "select * from CropDates WHERE year = " + year
                + " and cbtt IN ('" + String.Join("','", cbttList) + "') order by CBTT,SortIndex "; 

            DB.FillTable(table,sql );
            SetupIndexColumn(table);
            return table;
        }
        /// <summary>
        /// Gets Crop Dates given a year , used by user interface to edit crops
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static CropDatesDataSet.CropDatesDataTable GetCropDataTable(int year, bool includeEmptyDates=true)
        {
            CropDatesDataSet.CropDatesDataTable table = new CropDatesDataSet.CropDatesDataTable();
            var sql = "select * from CropDates where year = " + year;

            if (!includeEmptyDates)
            {
                sql += " and StartDate is not null and FullCoverDate is not null and TerminateDate is not null";
            }
            
            sql +=" ORDER BY cbtt,SortIndex";

            DB.FillTable(table, sql);
            //SQL search for Year and CBTT from the Access DB
            SetupIndexColumn(table);
            return table;
        }

        private static void SetupIndexColumn(CropDatesDataSet.CropDatesDataTable table)
        {
            table.indexColumn.AutoIncrement = true;
            table.indexColumn.AutoIncrementSeed = GetNextIndex();
        }

        /// <summary>
        /// Gets Crop Dates given a year and group number
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static CropDatesDataSet.CropDatesDataTable GetCropDataTable(int? year, string cbtt , int? group)
        {
            CropDatesDataSet.CropDatesDataTable table = new CropDatesDataSet.CropDatesDataTable();
            var sql = "select * from cropdates where 1=1 ";
            
            if (year.HasValue)
                sql += " and year = " + year;

            if (cbtt != "")
            {
                sql += " and cbtt = '" + cbtt + "' ";
            }

            if (group.HasValue)
                sql += " and \"group\" = " + group;

            sql += " ORDER BY year,cbtt,SortIndex";

            DB.FillTable(table, sql);
            SetupIndexColumn(table);
            return table;
        }

     
        /// <summary>
        /// Copy a new year's worth of Crop Dates from the previous years data
        /// Designed to run for the current year so run on 1/1/(CurrentYear) or later
        /// </summary>
        /// <param name="year"></param>
        public static void InitializeYear(int year)
        {
            if (DateTime.Now.Year != year)
            { 
                throw new ArgumentException("You can't initialize years other than the current one!");
            }
            else
            {
                var prevYearTable = new CropDatesDataSet.CropDatesDataTable();
                DB.FillTable(prevYearTable, "SELECT * FROM CropDates WHERE year = " + (year - 1).ToString());

                var newYearTable = new CropDatesDataSet.CropDatesDataTable();
                DB.FillTable(newYearTable, "SELECT * FROM CropDates WHERE year = " + year.ToString());

                if (newYearTable.Rows.Count != 0)
                {
                    throw new Exception("Error:  there is existing data for year " + year);
                }

                int nextIndex = CropDatesDataSet.GetNextIndex();
                var rowCount = prevYearTable.Count;
                for (int i = 0; i < rowCount; i++)
                {
                    var row = prevYearTable[i];
                    var newRow = newYearTable.NewCropDatesRow();
                    
                    newRow.index = nextIndex;
                    newRow.year = year;
                    newRow.sortindex = row.sortindex;
                    newRow.group = row.group;
                    newRow.cbtt = row.cbtt;
                    newRow.cropname = row.cropname;
                    //newRow.UIDX = row.UIDX;
                    newRow.cropcurvenumber =row.cropcurvenumber;

                    newYearTable.Rows.Add(newRow);

                    nextIndex++;
                }

                DB.SaveTable(newYearTable);
            }
        }

        private static int GetNextIndex()
        {
            var tbl = DB.Table("CropDates","Select max(Index) from CropDates");
            object o = tbl.Rows[0][0];

            return Convert.ToInt32(o) + 1;
        }



        public static int SaveTable(CropDatesDataTable tbl)
        {
           return DB.SaveTable(tbl);
        }
    }
}
