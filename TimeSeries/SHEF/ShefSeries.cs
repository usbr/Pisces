﻿using System;
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
    public class ShefSeries : Series
    {
        List<DateTime> dates;
        string m_filename;
        int m_scenarioNumber = -1;
        TextFile m_textFile;
        static Dictionary<string, TextFile> s_cache = new Dictionary<string, TextFile>();
        string m_slotName = "";
        string m_objectName = "";
        bool m_isSnapShot;

        public ShefSeries(string fileName)
        {
            ExternalDataSource = true;
            m_filename = fileName;
        }

        static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine(" ImportKlamathShef.exe " + Application.ProductVersion + " " + AssemblyUtility.CreationDate());
                Console.WriteLine("Usage: ImportKlamathShef.exe config.csv output.txt");
                return -1;
            }
            CsvFile csv = new CsvFile(args[0]);
            string outputFilename = args[1];
            var tokens = File.ReadAllLines(@"ftp.usbr.gov.txt");
            SimpleFtp ftpClient = new SimpleFtp(@"ftp://ftp.usbr.gov", tokens[0], tokens[1]);


            string[] dir = ftpClient.directoryListSimple("KBAO_data/");
            for (int i = 0; i < dir.Count(); i++)
            {
                Console.Write("download " + dir[i]);
                ftpClient.download("/KBAO_data/" + dir[i], dir[i]);

                ProcessFile(dir[i], outputFilename, csv);

                Console.Write("ok.  Delete remote ");
                ftpClient.delete("/KBAO_data/" + dir[i]);
                Console.WriteLine("done.");
                // move to attic.
                FileUtility.MoveToSubDirectory(Path.GetDirectoryName(dir[i]), "attic", dir[i]);
                if (i == 24) // limit number of files processed at once.
                {
                    Console.WriteLine("Stopped.  Limit of 24 files per call");
                    break;
                }
            }

            Console.WriteLine("found " + dir.Length + " files ");
            return 0;
        }

        private static void ProcessFile(string filename, string outputFilename, CsvFile csv)
        {
            for (int i = 0; i < csv.Rows.Count; i++)
            {
                var r = csv.Rows[i];
                var s = SimpleShef.ReadSimpleShefA(filename, r["shefloc"].ToString(), r["shefcode"].ToString());
            }
        }

    }
}
