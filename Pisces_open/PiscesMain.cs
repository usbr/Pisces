using System;
using System.IO;
using System.Windows.Forms;
using Reclamation.TimeSeries;
using Reclamation.Core;
using Reclamation.TimeSeries.Forms;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries.Excel;
namespace Pisces
{
    public class PiscesMain
    {

        static PiscesForm piscesForm1;
        static PiscesSettings explorer;
        static TimeSeriesDatabase db;

        /// <summary>
        /// Try to open database in the following order:
        ///  1) command line argument
        ///  2) config file 'fileName'
        ///  2) previous file opened
        ///  3) create empty database
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {


            Logger.EnableLogger(true);

             try
            {
                string fileName = "";
                if (args.Length == 1)
                {
                    fileName = args[0];
                    if (!File.Exists(fileName))
                    {
                        MessageBox.Show("Could not open file '" + fileName + "'");
                        return;
                    }
                }
                
                else if (UserPreference.Lookup("fileName") != ""
                    && File.Exists(UserPreference.Lookup("fileName")) 
                    && Path.GetExtension(UserPreference.Lookup("fileName")) != ".sdf")
                {
                    fileName = UserPreference.Lookup("fileName");
                }
                else
                {// open default database
                    fileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\tsdatabase.pdb";
                }

                if (!File.Exists(fileName))
                {
                    SQLiteServer.CreateNewDatabase(fileName);
                }


                UserPreference.SetDefault("HydrometServer", HydrometHost.PN.ToString(), false);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.ThreadExit += new EventHandler(Application_ThreadExit);
                Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
                //Application.Idle += new EventHandler(Application_Idle);
                //Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

                
                explorer = new PiscesSettings(new ExplorerView());
                // explorer.Database
                
                explorer.Open(fileName);
                 db = explorer.Database;
                db.OnReadSettingsFromDatabase += new EventHandler<TimeSeriesDatabaseSettingsEventArgs>(db_OnReadSettingsFromDatabase);
                db.OnSaveSettingsToDatabase += new EventHandler<TimeSeriesDatabaseSettingsEventArgs>(db_OnSaveSettingsToDatabase);

                db_OnReadSettingsFromDatabase(null, new TimeSeriesDatabaseSettingsEventArgs(db.Settings, explorer.TimeWindow));

                piscesForm1 = new PiscesForm(explorer);


                

                piscesForm1.FormClosed += new FormClosedEventHandler(explorerForm1_FormClosed);
                //Pisces2 p2 = new Pisces2(explorer);
                //p2.FormClosed += new FormClosedEventHandler(explorerForm1_FormClosed);

                Application.Run(piscesForm1);
                explorer.Database.SaveSettingsToDatabase(explorer.TimeWindow);
                //db.SaveSettingsToDatabase(explorer.TimeWindow);

                PostgreSQL.ClearAllPools();

                FileUtility.CleanTempPath();
            }
             catch (Exception exc)
             {
                 MessageBox.Show(exc.ToString());
             }
        }


        static void db_OnReadSettingsFromDatabase(object sender, TimeSeriesDatabaseSettingsEventArgs e)
        {
            var m_settings = e.Settings;
            HydrometInfoUtility.WebCaching = m_settings.ReadBoolean("HydrometWebCaching", false);
            HydrometInfoUtility.AutoUpdate = m_settings.ReadBoolean("HydrometAutoUpdate", false);
            HydrometInstantSeries.KeepFlaggedData = m_settings.ReadBoolean("HydrometIncludeFlaggedData", false);
            HydrometInfoUtility.WebOnly = m_settings.ReadBoolean("HydrometWebOnly", false);
            Reclamation.TimeSeries.Usgs.Utility.AutoUpdate = m_settings.ReadBoolean("UsgsAutoUpdate", false);
            //  db.se
            
            Reclamation.TimeSeries.Modsim.ModsimSeries.DisplayFlowInCfs = m_settings.ReadBoolean("ModsimDisplayFlowInCfs", false);
            //SpreadsheetGearSeries.AutoUpdate = m_settings.ReadBoolean("ExcelAutoUpdate", true);

            var w = e.Window;
            w.FromToDatesT1 = m_settings.ReadDateTime("FromToDatesT1", w.FromToDatesT1);
            w.FromToDatesT2 = m_settings.ReadDateTime("FromToDatesT2", w.FromToDatesT2);
            w.FromDateToTodayT1 = m_settings.ReadDateTime("FromDateToTodayT1", w.FromDateToTodayT1);
            w.NumDaysFromToday = m_settings.ReadDecimal("NumDaysFromToday", w.NumDaysFromToday);

            string s = m_settings.ReadString("TimeWindowType", "FromToDates");
            w.WindowType = (TimeWindowType)System.Enum.Parse(typeof(TimeWindowType), s);
            db.AutoRefresh = m_settings.ReadBoolean("AutoRefresh", true);
        }

        static void db_OnSaveSettingsToDatabase(object sender, TimeSeriesDatabaseSettingsEventArgs e)
        {
            var m_settings = e.Settings;
            m_settings.Set("HydrometWebCaching", HydrometInfoUtility.WebCaching);
            m_settings.Set("HydrometAutoUpdate", HydrometInfoUtility.AutoUpdate);
            m_settings.Set("HydrometIncludeFlaggedData", HydrometInstantSeries.KeepFlaggedData);
            m_settings.Set("HydrometWebOnly", HydrometInfoUtility.WebOnly);

            m_settings.Set("UsgsAutoUpdate", Reclamation.TimeSeries.Usgs.Utility.AutoUpdate);
            m_settings.Set("ModsimDisplayFlowInCfs", Reclamation.TimeSeries.Modsim.ModsimSeries.DisplayFlowInCfs);

            var w = e.Window;
            m_settings.Set("FromToDatesT1", w.FromToDatesT1);
            m_settings.Set("FromToDatesT2", w.FromToDatesT2);
            m_settings.Set("FromDateToTodayT1", w.FromDateToTodayT1);
            m_settings.Set("NumDaysFromToday", w.NumDaysFromToday);
            m_settings.Set("TimeWindowType", w.WindowType.ToString());

            m_settings.Set("AutoRefresh", db.AutoRefresh);

          //  m_settings.Set("ExcelAutoUpdate", SpreadsheetGearSeries.AutoUpdate);
            m_settings.Save();
        }

        static void explorerForm1_FormClosed(object sender, FormClosedEventArgs e)
        {

            Pisces.Properties.Settings.Default.Save();
            
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            //MessageBox.Show(e.Exception.Message);
        }

        static void Application_ThreadExit(object sender, EventArgs e)
        {
            //MessageBox.Show("Thread exit");
            Logger.WriteLine("Application_ThreadExit");
        }

    }
}