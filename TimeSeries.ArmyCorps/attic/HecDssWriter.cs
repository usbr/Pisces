using System;
using System.Data;
using System.IO;
using TimeSeries;
using System.Diagnostics;

namespace HecDss
{
	/// <summary>
	///  HecDssWriter.
	/// </summary>
	public class HecDssWriter
	{
    private string dssFilename;

    public HecDssWriter(string dssFilename, bool overwrite)
    {
      this.dssFilename = dssFilename;
		if( overwrite)
		{
			File.Delete(dssFilename);
			File.Delete(Path.ChangeExtension(dssFilename,".dsc"));
		}
    }
    
    private string PathToFile
    {
      get { return Path.GetDirectoryName(dssFilename);}
    }
    /// <summary>
    /// Writes time series to dss file.
    /// </summary>
    public void WriteTimeSeries(Series series, HecDssPath path)
    {
      int sz = series.Count;
      string DssTimeFormat = "ddMMMyyyy, HHmm"; // seconds are ignored.
      string fn = cbp.Utility.GetTempFileName(PathToFile);
      StreamWriter sw = new StreamWriter(PathToFile+"\\"+fn);
      sw.WriteLine(Path.GetFileName(dssFilename));
      sw.WriteLine(path.CondensedName);
      sw.WriteLine(series.Units);
      sw.WriteLine("inst-val");

      string prevDate = "xxxxx";
      for(int i=0; i<sz; i++)
      {
        string date =   series[i].DateTime.ToString(DssTimeFormat);
        double val =    series[i].Value;
        if( prevDate == date)
        {// delete duplicates withing same minute.
          Console.WriteLine("skipping duplicate date "+date);
          continue;
        }
        sw.WriteLine(date+", "+val);
        prevDate = date;
      }
      sw.WriteLine("end");
      sw.Close();

      string workingDir = Path.GetDirectoryName(PathToFile);
      Console.WriteLine("about to import using "+fn);
      string[] stdout = HecDssProgramRunner.Run("dssits.exe","input="+fn,PathToFile);
			//Console.WriteLine(String.Join("\n",stdout));
      File.Delete(PathToFile+"\\"+fn); // remove temporary script file.

    }




/* sample input file for dssits
ITS2.DSS
/LW/LW/ELEV//IR-YEAR/RULE CURVE/
FEET
INST-VAL
01JAN2002, 2400, 20.0
15FEB92, 2400, 20.0
01MAY92, 2400, 21.85
31MAY92, 2400, 21.85
01JUN92, 2400, 22.00
18JUL92, 2400, 22.00
01DEC92, 2400, 20.00
31DEC92, 2400, 20.00
END		 
     */

    private void pr_WriteLine(object sender, cbp.ProgramRunner.ProgramEventArgs e)
    {
      Console.WriteLine(e.Message);
    }
  }
}
