using System;
using System.Data;
using System.IO;
using System.Diagnostics;


namespace HecDss
{
	/// <summary>
	/// Summary description for HecDssMath.
	/// </summary>
	public class HecDssMath
	{

    string dssFilename;

    public HecDssMath(string dssFilename)
    {
      this.dssFilename=dssFilename;
      if( !File.Exists(dssFilename))
      {
        
        throw new FileNotFoundException("File does not exist ",dssFilename);
      }
    }

    private string PathToFile
    {
      get { return Path.GetDirectoryName(dssFilename);}
    }

    /// <summary>
    /// Computes daily average 
    /// returns new path saved in hec dss file
    /// </summary>
    public HecDssPath DailyAverage( string dssPathname, DateTime t1, DateTime t2)
    {
      // create data input file.
      string filename = cbp.Utility.GetTempFileName(PathToFile);
      StreamWriter sw = new StreamWriter(PathToFile+"\\" + filename);

      sw.WriteLine("ti "+t1.ToString("ddMMMyyyy")+" "+t2.ToString("ddMMMyyyy"));
      sw.WriteLine("get aaa="+dssFilename+":"+dssPathname);
      sw.WriteLine("co  bbb=ts2(aaa,1DAY,0M)");
      string newPath = dssPathname.Trim().Substring(0,dssPathname.Length-1)+"dailyAvg/";
      newPath = newPath.Replace("IR-DAY","1day");
      sw.WriteLine("put bbb="+dssFilename+":"+newPath);
      sw.WriteLine("cl all");
      sw.Close();
      /* Example..
      ti 02feb1996 02feb1997
      get aaa=site28:/cbp/site28/flow/01Feb1996/IR-DAY/site28 flows/
      co  bbb=ts2(aaa,1DAY,0M)
      put bbb=site28:/cbp/site28/flow/01feb1996/1day/site28 daily flows/
      cl all
      */
		string[] rval = HecDssProgramRunner.Run("dssmath.exe","input="+filename,PathToFile);
		Console.WriteLine(String.Join("\n",rval));
		HecDssPath hp = new HecDssPath(newPath);
		File.Delete(PathToFile+"\\"+filename);
		return hp;
    }

	}
}
