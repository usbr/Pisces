using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;
using System.IO;

namespace Reclamation.TimeSeries.Hec
{
    public class HecDssTree
    {
        public static void AddDssFileToDatabase(string dssFilename, PiscesFolder parent,
            TimeSeriesDatabase db)
        {
            PiscesFolder root = parent;
            try
            {
                string[] paths = GetCatalog(dssFilename);
                root = db.AddFolder(parent, Path.GetFileName(dssFilename));
                var sc = db.GetSeriesCatalog();
                int folderID = root.ID;
                string previousA = "";
                //db.SuspendTreeUpdates();
                for (int i = 0; i < paths.Length; i++)
                {
                    HecDssPath p = new HecDssPath(paths[i]);
                    if (i == 0 || p.A != previousA)
                    {
                        folderID  =sc.AddFolder(p.A, root.ID);
                        previousA = p.A;
                    }

                    HecDssSeries s = new HecDssSeries(dssFilename, paths[i]);
                    sc.AddSeriesCatalogRow(s, sc.NextID(), folderID);
                }
                db.Server.SaveTable(sc);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
            finally
            {
                //db.ResumeTreeUpdates();
                //db.RefreshFolder(parent);
            }
        }



        /// <summary>
        /// Returns a list of paths with dates (part D ) removed
        /// </summary>
        /// <param name="dssFilename"></param>
        /// <returns></returns>
        public static string[] GetCatalog(string dssFilename)
        {
            string dir = Path.GetDirectoryName(dssFilename);
            string fnScript = FileUtility.GetTempFileNameInDirectory(dir, ".txt");
            StreamWriter sw = new StreamWriter(fnScript);


            if (Path.GetFileName(dssFilename).IndexOf(" ")>=0)
            {
                System.Windows.Forms.MessageBox.Show("Warning: The dss filename has a space.  ");
            }

            sw.WriteLine(Path.GetFileName(dssFilename));

            sw.WriteLine("CA");// create (N)ew (C)ondenced catalog  (F)ull batch mode.
            sw.Close();

            string dssutl = HecDssSeries.GetPathToDssUtl();

            ProgramRunner pr = new ProgramRunner();
            pr.Run(dssutl, "input=" + Path.GetFileName(fnScript), dir);
            pr.WaitForExit();
            return ParseRawCatalog(pr.Output);


        }

        private static string[] ParseRawCatalog(string[] lines)
        {
            TextFile tf = new TextFile(lines);

            int idx = tf.IndexOf(" Number   Tag     Program   Date  Time  Type Vers Data        Record Pathname");
            int idx2 = tf.IndexOf("   -----DSS---ZCLOSE");
                                        
            if (idx < 0 || idx2 < 0)
            {
                Logger.WriteLine("Error creating catalog ");
                System.Windows.Forms.MessageBox.Show("Error creating DSS catalog \n\n"+String.Join("\n",lines));
                return new string[] { };
            }

            List<string> paths = new List<string>();
            for (int i = idx + 2; i < idx2; i++)
            {
                string s = tf[i].Substring(56).Trim();
                HecDssPath p = new HecDssPath(s);
                if (!paths.Contains(p.CondensedName)) // condensed name removes dates,period of record
                    paths.Add(p.CondensedName);

            }
            return paths.ToArray();
        }
    }
}
