using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrometForecast
{
    public class CommandLine
    {

        [STAThread]
        static void Main(string[] args)
        {

            if( args.Length != 5)
            {
                Console.WriteLine("HydrometForecast.exe basin.csv date level look-ahead output");
                Console.WriteLine("Where: ");
                Console.WriteLine("        basin.csv      -- name of csv input file");
                Console.WriteLine("        date           -- date to run forecaset 2017-01-01");
                Console.WriteLine("        level          -- subsequent conditions  1.0 normal  0.8 for 80%, etc... ");
                Console.WriteLine("        look-ahead     -- perfect forecast 0 or 1");
                Console.WriteLine("        output         -- filename for output");

                Console.WriteLine("Example:  HydrometForecast  heise.csv 2016-1-1 1.0 0 output.txt");
                return;
            }

            var filename = args[0];
            DateTime t = DateTime.Parse(args[1]);
            double level = double.Parse(args[2]);
            int lookAhead = int.Parse(args[3]);
            string outputFileName = args[4];


            ForecastEquation eq = new ForecastEquation(filename);
            ForecastResult result = eq.Evaluate(t,lookAhead == 1 ,level);

            File.WriteAllLines(outputFileName, result.Details.ToArray());

        }

    }
}
