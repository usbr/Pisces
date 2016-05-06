﻿using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;
using Reclamation.TimeSeries.NOAA;
using Reclamation.TimeSeries.Hydromet;

namespace Reclamation.TimeSeries.SHEF
{

    /*
     * TO DO: 
     * 1. Build a ReadCore() method using the connection string to read from the source text file
     * 2. Move some of the functions that gets Stations and P-Codes from ImportShef.cs in Forms/ImportForms
     * 
     */

    public class ShefSeries : Series
    {
        DataTable shefDataTable = new DataTable();
        string location, pecode, filename;

        public ShefSeries()
        {
            new ShefSeries("", "", "");
        }

        public ShefSeries(string location, string pecode, string filename)
        {
            ExternalDataSource = false;
            this.Name = location + "_" + pecode;
            this.SiteID = location;
            this.Parameter = pecode;
            this.Source = "SHEF";
            this.Provider = "ShefSeries";
            getShefTimeInterval(pecode);
            this.ConnectionString = "File=" + filename + ";ShefLocation=" + location + ";ShefCode=" + pecode + ";";
            this.Table.TableName = this.Name;
        }

        public ShefSeries(TimeSeriesDatabase db, TimeSeriesDatabaseDataSet.SeriesCatalogRow sr):base(db, sr)
        {
            location = ConnectionStringUtility.GetToken(ConnectionString, "ShefLocation", "");
            pecode = ConnectionStringUtility.GetToken(ConnectionString, "ShefCode", "");
            filename = ConnectionStringUtility.GetToken(ConnectionString, "File", "");
            InitTimeSeries(null, "", this.TimeInterval, true);
        }

        private void getShefTimeInterval(string pecode)
        {
            try
            {
                var tChar = pecode.ToCharArray()[pecode.Length - 1];
                if (tChar == 'H')
                { this.TimeInterval = TimeSeries.TimeInterval.Hourly; }
                else if (tChar == 'D')
                { this.TimeInterval = TimeSeries.TimeInterval.Daily; }
                else if (tChar == 'M')
                { this.TimeInterval = TimeSeries.TimeInterval.Monthly; }
                else
                { this.TimeInterval = TimeSeries.TimeInterval.Irregular; }
            }
            catch
            { this.TimeInterval = TimeSeries.TimeInterval.Irregular; }
        }

        public DataTable ReadShefFile(string fileName)
        {
            shefDataTable = new DataTable();
            shefDataTable.Columns.Add(new DataColumn("location", typeof(string)));
            shefDataTable.Columns.Add(new DataColumn("datetime", typeof(DateTime)));
            shefDataTable.Columns.Add(new DataColumn("shefcode", typeof(string)));
            shefDataTable.Columns.Add(new DataColumn("value", typeof(double)));

            List<string> shefInventory = new List<string>();
            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, 128))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var lineItems = System.Text.RegularExpressions.Regex.Split(line, @"\s+");
                    string location = lineItems[1];
                    DateTime t = DateTime.ParseExact(lineItems[2] + lineItems[4].Replace("DH", ""), "yyyyMMddHHmm", System.Globalization.CultureInfo.InvariantCulture);
                    var lineShefCodes = System.Text.RegularExpressions.Regex.Split(line, @"\/+");
                    for (int i = 1; i < lineShefCodes.Count(); i++)
                    {
                        var shefItems = System.Text.RegularExpressions.Regex.Split(lineShefCodes[i], @"\s+");
                        var shefcode = shefItems[0];
                        var shefValue = Convert.ToDouble(shefItems[1]);
                        shefDataTable.Rows.Add(location, t, shefcode, shefValue);
                    }
                }
            }
            return shefDataTable;
        }

        //protected override void ReadCore()
        //{
        //    Add(GetShefSeries());
        //}

        //private Series GetShefSeries()
        //{
        //    var dTab = ReadShefFile(filename);
        //    var valTable = dTab.Select(string.Format("location = '{0}' AND shefcode = '{1}'", location, pecode));
        //    foreach (DataRow item in valTable)
        //    {
        //        this.Add(DateTime.Parse(item["datetime"].ToString()), Convert.ToDouble(item["value"]));
        //    }
        //    return this;
        //}

        //////////////////////////////////////////////////////////////////////////////////////
        // HYDROMET PROGRAM FROM K.TARBET
        //////////////////////////////////////////////////////////////////////////////////////
        #region
        //static int Main(string[] args)
        //{
        //    if (args.Length != 2)
        //    {
        //        Console.WriteLine(" ImportKlamathShef.exe " + Application.ProductVersion + " " + AssemblyUtility.CreationDate());
        //        Console.WriteLine("Usage: ImportKlamathShef.exe config.csv output.txt");
        //        return -1;
        //    }
        //    CsvFile csv = new CsvFile(args[0]);
        //    string outputFilename = args[1];
        //    var tokens = File.ReadAllLines(@"ftp.usbr.gov.txt");
        //    SimpleFtp ftpClient = new SimpleFtp(@"ftp://ftp.usbr.gov", tokens[0], tokens[1]);


        //    string[] dir = ftpClient.directoryListSimple("KBAO_data/");
        //    for (int i = 0; i < dir.Count(); i++)
        //    {
        //        Console.Write("download " + dir[i]);
        //        ftpClient.download("/KBAO_data/" + dir[i], dir[i]);

        //        ProcessFile(dir[i], outputFilename, csv);

        //        Console.Write("ok.  Delete remote ");
        //        ftpClient.delete("/KBAO_data/" + dir[i]);
        //        Console.WriteLine("done.");
        //        // move to attic.
        //        FileUtility.MoveToSubDirectory(Path.GetDirectoryName(dir[i]), "attic", dir[i]);
        //        if (i == 24) // limit number of files processed at once.
        //        {
        //            Console.WriteLine("Stopped.  Limit of 24 files per call");
        //            break;
        //        }
        //    }

        //    Console.WriteLine("found " + dir.Length + " files ");
        //    return 0;
        //}

        //private static void ProcessFile(string filename, string outputFilename, CsvFile csv)
        //{
        //    for (int i = 0; i < csv.Rows.Count; i++)
        //    {
        //        var r = csv.Rows[i];
        //        var s = SimpleShef.ReadSimpleShefA(filename, r["shefloc"].ToString(), r["shefcode"].ToString());
        //    }
        //}
        #endregion
    }
}
