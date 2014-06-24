using System;
using System.Data;
using System.Configuration;
using cbp;


namespace TimeSeries
{
	/// <summary>
	/// RecordsDatabase manages sybase connection to Records database
	/// </summary>
	public class Database
	{
    private static Sybase _cbpscada;
    private static Sybase _hydrodb;

    public static Sybase CbpScada
    {
      get { return _cbpscada;}
    }
    public static Sybase HydroDB
    {
      get { return _hydrodb;}
    }

    public static void Connect()
    {
      Connect("ktarbet","sqlexpress"); // development-testing
    }
    /// <summary>
    /// connect to scada and records database.
    /// </summary>
    public static void Connect(string user, string password)
    {
      Console.WriteLine("Connect");
      SybaseLogin sl = new SybaseLogin();
      sl.ServerIPNumber= Config("ScadaServerIPNumber");
      sl.User =user;
      sl.Password = password;
      sl.ServerName = Config("ScadaServerName");
      sl.DatabaseName=Config("ScadaDatabaseName");
      _cbpscada = new Sybase(sl);

      sl = new SybaseLogin();
      sl.ServerIPNumber= Config("RecordsServerIPNumber");
      sl.User =user;
      sl.Password = password;
      sl.ServerName = Config("RecordsServerName");
      sl.DatabaseName=Config("RecordsDatabaseName");
      _hydrodb =new Sybase(sl);
    
    }

    private static string Config(string key)
    {
      string rval = ConfigurationSettings.AppSettings[key];
      if (rval != null)
      {
        return rval;
      }

      Console.WriteLine("could not find key '"+key+"'");
        return "";
    }
    
	}
}
