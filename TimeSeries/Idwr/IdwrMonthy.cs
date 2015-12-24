using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Idwr
{
    public class IdwrMonthy
    {
        string m_dataFile;
        string m_indexFile;
        DataTable IDWRdb;
        DataTable IDWRindex;
        static TimeSeriesDatabase db;
        //SeriesCatalog sc;
        string databasename;
        int[] sid;
        public static void Import(Reclamation.Core.Arguments args, TimeSeriesDatabase dbase)
        {
            //// Pisces /ImportIdwr /Data:data.dat /Index:index.dat /Filename:SnakeMonthly.sdf
            if (!args.Contains("Data") || !args.Contains("Filename"))
            {
                Console.WriteLine("Usage: Pisces /ImportIdwr /Data:datafile /Filename:databasename");
                Console.WriteLine("   or: Pisces /ImportIdwr /Data:datafile /Index:indexfile /Filename:databasename");
                return;
            }
            string dbname = args["Filename"];
            IdwrMonthy id;
            db = dbase;
            if (args.Count == 3)
            {
                    id = new IdwrMonthy(args["Data"], "noIndex" , dbname);
            }
            else
            {
                if (args.Contains("Index"))
                {
                    if (args[2] == "ValidityCheck")
                        id = new IdwrMonthy(args["Data"], "ValidityCheck", dbname);
                    else
                        id = new IdwrMonthy(args["Data"], args["Index"], dbname);
                }
                else
                    id = new IdwrMonthy(args["Data"], "noIndex", dbname);
            }
            id.Import();
        }
        IdwrMonthy(string dataFile, string indexFile, string dbname)
        {
           // sc = new SeriesCatalog(db.Server);
            //sc.DatabaseList.Add(db);
            //if (!sc.GetTree().Columns.Contains("SiteName"))
            //    sc.GetTree().Columns.Add("SiteName");
            //if (!sc.GetTree().Columns.Contains("TimeInterval"))
            //    sc.GetTree().Columns.Add("TimeInterval");
            //if (!sc.GetTree().Columns.Contains("Units"))
            //    sc.GetTree().Columns.Add("Units");
            //if (!sc.GetTree().Columns.Contains("TableName"))
            //    sc.GetTree().Columns.Add("TableName");
            //if (!sc.GetTree().Columns.Contains("Source"))
            //    sc.GetTree().Columns.Add("Source");
            IDWRindex = null;
            m_indexFile = indexFile;
            m_dataFile = dataFile;
            databasename = dbname;
            if (m_indexFile != "noIndex" && m_indexFile != "ValidityCheck")
                IDWRindex = ReadIndexFile();
            IDWRdb = ReadIDWRDataFile();
        }
        DataTable ReadIndexFile()
        {
            DataTable rval = new DataTable();
            if(!File.Exists(m_indexFile))
                throw new Exception("Index File " + m_indexFile + " is missing");
            string[] lines = File.ReadAllLines(m_indexFile);
            rval.Columns.Add("IDWRid", typeof(int));
            rval.Columns.Add("SiteName", typeof(string));
            rval.Constraints.Add("pk", new DataColumn[] {rval.Columns[0]}, true);
            DataRow row;
            int id;
            string name;
            for (int i = 0; i < lines.Length; i++)
            {
                row = rval.NewRow();
                id = Convert.ToInt32(lines[i].Substring(0, 9));
                name = lines[i].Substring(9);
                row[0] = id;
                row[1] = name;
                rval.Rows.Add(row);
            }
            return rval;
        }
        DataTable ReadIDWRDataFile()
        {
            if (!File.Exists(m_dataFile))
                throw new Exception(m_dataFile + " not found");
            bool dataIsOK = true;
            bool goodrow;
            int id, year, i, j, ib;
            string[] data = File.ReadAllLines(m_dataFile);
            DataTable rval = new DataTable();
            DataRow row;
            float val;
            List<int> lid = new List<int>();
            rval.Columns.Add("IDWRid", typeof(int));
            rval.Columns.Add("wy", typeof(int));
            rval.Constraints.Add("pk",new DataColumn[] {rval.Columns[0],rval.Columns[1]},true);
            for (i = 0; i < 12; i++)
            {
                rval.Columns.Add("mon" + i);
            }
            for (i = 0; i < data.Length; i++)
            {
                if (m_indexFile == "ValidityCheck")
                    goodrow = ValidYearOfData(rval, i, data[i]);
                else
                    goodrow = true;
                if (goodrow)
                {
                    id = Convert.ToInt32(data[i].Substring(0, 9));
                    year = Convert.ToInt32(data[i].Substring(9, 5));
                    if (i == 0 || id != lid[lid.Count - 1])
                    {
                        lid.Add(id);
                    }
                    row = rval.NewRow();
                    row[0] = id;
                    row[1] = year;
                    for (j = 0; j < 12; j++)
                    {
                        ib = 14 + j * 7;
                        val = Convert.ToSingle(data[i].Substring(ib, 7)) * 1000;
                        row[j + 2] = Convert.ToInt32(val + 0.5);
                    }
                    rval.Rows.Add(row);
                }
                else
                    dataIsOK = false;
            }
            if (dataIsOK == false)
                throw new Exception(" baddata");
            sid = new int[lid.Count];
            lid.CopyTo(sid);
            return rval;
        }
        bool ValidYearOfData(DataTable rval, int i, string data)
        {
            int ib, ie, j;
            string[] field = new string[12];
            if (data.Length != 98)
            {
                Console.WriteLine("line " + i + " not expected format");
                return false;
            }
            int id = Convert.ToInt32(data.Substring(0, 9));
            if (id < 13000000 || id > 14000000)
            {
                Console.WriteLine("line " + i + " id not within range");
                return false;
            }
            int year = Convert.ToInt32(data.Substring(9, 5));
            int yearnow = DateTime.Today.Year;
            if (year < 1800 || year > yearnow)
            {
                Console.WriteLine("line " + i + " year " + year);
                return false;
            }
            if (i > 0)
            {
                DataRow[] check = rval.Select("IDWRid = " + id);
                if (check.Length > 0)
                {
                    for (int r = 0; r < check.Length; r++)
                    {
                        int checkyr = Convert.ToInt32(check[r][1]);
                        int checkid = Convert.ToInt32(check[r][0]);
                        if (checkid == id && checkyr == year)
                        {
                            Console.WriteLine("line " + i + " duplicate record");
                            return false;
                        }
                    }
                }
            }
            for (j = 0; j < 12; j++)
            {
                ib = 14 + j * 7;
                ie = ib + 6;
                field[j] = data.Substring(ib, 7);
                if (field[j].Substring(5, 1) != ".")
                {
                    Console.WriteLine("line " + i + " not expected format");
                    return false;
                }
            }
            return true;
        }
        void Import()
        {
            DataRow row;
            for (int i = 0; i < sid.Length; i++)
            {
                Series s = new Series();
                s.TimeInterval = TimeInterval.Monthly;
                s.Units = "acre-feet";
                s.Table = SetSeriesTable(sid[i]);
               
                s.SiteID = "Idwr:" + sid[i].ToString();
                if (m_indexFile == "noIndex" || m_indexFile == "ValidityCheck")
                    s.Name = Convert.ToString(sid[i]);
                else
                {
                    row = IDWRindex.Select("IDWRid = " + sid[i])[0];
                    s.Name = Convert.ToString(row["SiteName"]);
                }
                s.Table.TableName = s.Name;
                Import(s);
            }
        }
        DataTable SetSeriesTable(int id)
        {
            DataRow row;
            DateTime date;
            DateTime now = DateTime.Today;
            DataTable rval = new DataTable();
            rval.Columns.Add("Date", typeof(DateTime));
            rval.Columns.Add("AF", typeof(int));
            string sql = "IDWRid = " + id;
            DataRow[] rows = IDWRdb.Select(sql);
            for (int r = 0; r < rows.Length; r++)
            {
                date = new DateTime(Convert.ToInt32(rows[r][1]) - 1, 10, 1);
                for (int j = 0; j < 12; j++)
                {
                    row = rval.NewRow();
                    row[0] = date;
                    row[1] = rows[r][j + 2];
                    rval.Rows.Add(row);
                    date = date.AddMonths(1);
                }
            }
            return rval;
        }
        private void Import(Series s)
        {
            //SeriesInfo si = db.SeriesCatalog.NewSeriesInfo();
            //si.ParentID = 0;
            s.Source = "Idwr:" + s.SiteID;
            s.Provider = databasename;
            s.ConnectionString = "IDWR;file=" + m_dataFile + ";index=" + m_indexFile;
            //si.ID = si.id;

            db.AddSeries(s);
            //db.SeriesCatalog.Add(si);
            //db.ImportTimeSeriesTable(s.Table, si,false);
        }
        //public static void UpdateIdwr(Arguments args, TimeSeriesDatabase dbase)
        //{
        //    DateTime t1 = DateTime.MinValue;
        //    DateTime t2 = DateTime.MaxValue;
        //    if (!args.Contains("Data"))
        //    {
        //        Console.WriteLine("Usage: Pisces /UpdateIdwr /Data:datafile /Filename:databasename");
        //        Console.WriteLine("Usage: Pisces /UpdateIdwr /Data:datafile /T1:beginningDate /T2:endingDate /Filename:databasename");
        //        return;
        //    }
        //    db = dbase;
        //    IdwrFile dt = new IdwrFile(args["Data"]);
        //    if (args.Contains("T1"))
        //    {
        //        t1 = Convert.ToDateTime(args["T1"]);
        //        t2 = Convert.ToDateTime(args["T2"]);
        //    }
        //    int yr, year, sdi = -1;
        //    string siteNum, lastNum = "0";
        //    int mon;
        //    DataRow row;
        //    for (int i = 0; i < dt.Table.Rows.Count; i++)
        //    {
        //        year = Convert.ToInt32(dt.Table.Rows[i][1]);
        //        if (t1.Year > year) continue;
        //        if (t2.Year < year) continue;
        //        siteNum = Convert.ToString(dt.Table.Rows[i][0]);
        //         Series s = null;
        //        if (siteNum != lastNum)
        //        {
        //            s = db.GetSeries("SiteName like '%" + siteNum + "%'");
        //            //sdi = FindSiteID(siteNum);
        //            if (s == null)
        //            {
        //                Console.WriteLine("Site ID " + siteNum + " not in database");
        //                lastNum = siteNum;
        //                continue;
        //            }
        //            lastNum = siteNum;
        //        }
        //        if (sdi < 0) continue;
        //        //SeriesInfo si = db.SeriesCatalog.GetSeriesInfo(sdi);
        //        DataTable ts = new DataTable();
        //        ts.Columns.Add("DateTime", typeof(DateTime));
        //        ts.Columns.Add("value", typeof(int));
        //        for (int j = 0; j < 12; j++)
        //        {
        //            mon = (j < 3 ? j + 10 : j - 2);
        //            yr = (j < 3 ? year - 1 : year);
        //            row = ts.NewRow();
        //            row["DateTime"] = new DateTime(yr, mon, 1);
        //            row["value"] = Convert.ToInt32(dt.Table.Rows[i][j+2]);
        //            ts.Rows.Add(row);
        //        }
        //        //db.ImportTimeSeriesTable(ts, si, true);
        //        db.SaveTimeSeriesTable(s.ID, s, true);
        //    }
        //}
        //static int FindSiteID(string id)
        //{
        //    int rval = -1;
        //    string siteNum, siteName;
        //    //DataTable dt = db.SeriesCatalog.GetTree();
            
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        siteName = Convert.ToString(dt.Rows[i]["SiteName"]);
        //        if (siteName.Contains(":"))
        //        {
        //            siteNum = siteName.Split(':')[1];
        //            if (siteNum == id) return Convert.ToInt32(dt.Rows[i]["id"]);
        //        }
        //    }
        //    return rval;
        //}
    }

    public class IdwrFile : DataTable
    {
        public DataTable Table
        {
            get { return table; }
        }
        public string Filename
        {
            get { return filename; }
        }
        DataTable table;
        string filename;
        string[] lines;
        public IdwrFile(string file)
        {
            filename = file;
            ReadFile(file);
        }
        void ReadFile(string file)
        {
            DataRow row;
            string[] field;
            table = new DataTable();
            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("Year", typeof(int));
            for (int i = 0; i < 12; i++)
			{
			 table.Columns.Add("val"+i, typeof(int));
			}
            lines = File.ReadAllLines(file);
            for (int i = 0; i < lines.Length; i++)
            {
                if (!lines[i].Contains(","))
                {
                    Console.WriteLine("bad IdwrFile " + file + " line " + i);
                    continue;
                }
                field = lines[i].Split(',');
                if (field.Length != 14)
                {
                    Console.WriteLine("bad IdwrFile " + file + " line " + i);
                    continue;
                }
                row = table.NewRow();
                row[0] = field[0];
                int kyr = Convert.ToInt32(field[1]);
                if (kyr < 1800 || kyr > DateTime.Now.Year)
                {
                    Console.WriteLine("bad IdwrFile " + file + " line " + i);
                    continue;
                }
                row[1] = kyr;
                for (int j = 0; j < 12; j++)
                {
                    try
                    {
                        row[j + 2] = Convert.ToInt32(field[j + 2]) * 100 + .5;
                    }
                    catch
                    {
                        Console.WriteLine("bad IdwrFile " + file + " line " + i);
                        continue;
                    }
                }
                table.Rows.Add(row);
            }
        }
    }
}
