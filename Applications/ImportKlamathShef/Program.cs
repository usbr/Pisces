using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;
using Reclamation.TimeSeries.NOAA;
using Reclamation.TimeSeries.Hydromet;
namespace ImportKlamathShef
{

    class Program
    {
        
        static int Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine(" ImportKlamathShef.exe "+ Application.ProductVersion+ " "+AssemblyUtility.CreationDate());
                Console.WriteLine("Usage: ImportKlamathShef.exe config.csv output.txt input_filepath");
                return -1;
            }
            CsvFile csv = new CsvFile(args[0]);
            string outputFilename = args[1];
       
            if( args.Length == 3)
            {// read files from local directory instead of ftp.
                ProcessFromDirectory(args[2],csv,outputFilename);
            }
            return 0;
        }

        private static void ProcessFromDirectory(string dir, CsvFile csv, string outputFilename)
        {
            string[] fileEntries = Directory.GetFiles(dir);

            if( fileEntries.Length == 0)
            {
                Console.WriteLine("no files found");
                return;
            }

            Console.WriteLine("saving to '" + outputFilename + "'");

            for (int i = 0; i < fileEntries.Length; i++)
            {
                ProcessFile(fileEntries[i],outputFilename, csv);
                FileUtility.MoveToSubDirectory(Path.GetDirectoryName(fileEntries[i]), "attic", fileEntries[i]);
            }

        }



        private static void ProcessFile(string filename,string outputFilename,CsvFile csv)
        {
            for (int i = 0; i < csv.Rows.Count; i++)
            {
                var r = csv.Rows[i];
                var s = SimpleShef.ReadSimpleShefA(filename, r["shefloc"].ToString(), r["shefcode"].ToString());
                Console.WriteLine(r["cbtt"].ToString()+"/"+r["pcode"].ToString()+" "+s.Count +" records");

                HydrometInstantSeries.WriteToHydrometFile(s, r["cbtt"].ToString(),
                    r["pcode"].ToString(), WindowsUtility.GetUserName(), outputFilename, true);

            }
            
        }
    }
}
