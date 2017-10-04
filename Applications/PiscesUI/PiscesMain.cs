using Reclamation.Core;
using Reclamation.TimeSeries.Forms;
using Reclamation.TimeSeries.Hydromet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Windows.Forms;
namespace Pisces
{
    public class PiscesMain
    {

        static PiscesForm piscesForm1;

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
                string fileName = GetFileName(args);

                HydrometInfoUtility.SetDefaultHydrometServer();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.ThreadExit += new EventHandler(Application_ThreadExit);
                Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
                
               
                piscesForm1 = new PiscesForm(fileName);

                piscesForm1.FormClosed += new FormClosedEventHandler(explorerForm1_FormClosed);

                Application.Run(piscesForm1);

                PostgreSQL.ClearAllPools();
                FileUtility.CleanTempPath();
            }
             catch (Exception exc)
             {
                 MessageBox.Show(exc.ToString());
             }
        }

        private static string GetFileName(string[] args)
        {
            string fileName = "";
            if (args.Length == 1)
            {
                fileName = args[0];
            }

            else if (UserPreference.Lookup("fileName") != ""
                && File.Exists(UserPreference.Lookup("fileName"))
                && Path.GetExtension(UserPreference.Lookup("fileName")) != ".sdf")
            {
                fileName = UserPreference.Lookup("fileName");
            }
            else
            {// open default database

                var paths = new List<string>() 
                    {
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        FileUtility.GetLocalApplicationPath(),
                        FileUtility.GetTempPath()
                    };

                foreach (var item in paths)
                {
                    if (HasWriteAccessToFolder(item))
                    {
                        fileName = Path.Combine(item, "tsdatabase.pdb");
                        break;
                    }
                }
            }

            if (!File.Exists(fileName))
            {
                SQLiteServer.CreateNewDatabase(fileName);
            }
            return fileName;
        }

        /* taken from here: http://stackoverflow.com/q/1410127/2333687 */
        private static bool HasWriteAccessToFolder(string folderPath)
        {
            try
            {
                // Attempt to get a list of security permissions from the folder. 
                // This will raise an exception if the path is read only or do 
                // not have access to view the permissions or path is null.
                DirectorySecurity ds = Directory.GetAccessControl(folderPath);
                return true;
            }
            catch (Exception)
            {
                return false;
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