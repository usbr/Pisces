using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Reclamation.Riverware
{
    /// <summary>
    /// Used for control files that have data 'embeded' in the
    /// control file instead of an external data source
    /// </summary>
    class ControlFileWithData
    {


        public static void ProcessFile(string filename)
        {

            ControlFile cf = new ControlFile(filename);

            for (int i = 0; i < cf.Length; i++)
            {
                double val=0.0;
                if (cf.OptionExists(i, "static_file"))
                {
                    // nothing to do for static files
                }
                else
                if (cf.TryParse(i, "data", out val, 0))
                {
                    string outFilename="";
                    cf.TryParse(i,"file",out outFilename);
                    File.WriteAllText(outFilename, val.ToString()+"\n");
                }
                else
                {
                    Console.WriteLine("Error parsing '"+cf[i]);
                }
            }
        }
/*        units: decimal
scale: 1.000000
# Periodic Slot: BoiseCanals:FarmersUnion.Periodic Fraction
# Row: [0] 0/0/1/1/1
# Column: [0] 
0.25 
        */

    }
}
