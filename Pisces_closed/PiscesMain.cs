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

                HydrometInfoUtility.SetDefaultHydrometServer();


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