using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Reclamation.Core;
using SeriesCatalogRow = Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow;
using Reclamation.TimeSeries.Parser;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// TimeSeriesFactory creates objects stored in the TimeSeriesDatabase
    /// </summary>
    public class PiscesFactory
    {
        TimeSeriesDatabase db;
        public PiscesFactory(TimeSeriesDatabase db)
        {
            this.db = db;
        }

        public IEnumerable<Series> GetSeries(TimeInterval interval, string filter = "",string propertyFilter="")
        {
            string sql = " timeinterval = '" + interval.ToString() + "'";
            if (filter != "")
                sql += " AND " + filter;

            var sc = db.GetSeriesCatalog(sql,propertyFilter);

            foreach (var sr in sc)
            {
                var s = GetSeries(sr);

                yield return s;
            }
        }

        /// <summary>
        ///  GetCalculationSeries returns a list of CalculationSeries
        ///  sorted by dependencies so calculations can procede in the 
        ///  proper order.
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="filter">supplement for the SQL where clause</param>
        /// <param name="propertyFilter">two part filter for the siteproperty table i.e.  'program:agrimet'</param>
        /// <returns></returns>
        public List<CalculationSeries> GetCalculationSeries(TimeInterval interval, string filter="", string propertyFilter="")
        {
            string sql = "provider = 'CalculationSeries' AND "
                + " timeinterval = '" + interval.ToString() + "'";
            if (filter != "")
                sql += " AND "+filter;

            var sc = db.GetSeriesCatalog(sql,propertyFilter);

            List<CalculationSeries> list1 = new List<CalculationSeries>();
            foreach (var sr in sc)
            {
                var s = GetSeries(sr) as CalculationSeries;
                if( s.Enabled )
                  list1.Add(s);
            }
            return list1;
        }


        public PiscesFolder GetFolder(int id)
        {
            TimeSeriesDatabaseDataSet.SeriesCatalogRow sr = db.GetSeriesRow(id);
           PiscesObject o = CreateObject(sr);

           if (!(o is PiscesFolder))
           {
               throw new ArgumentException("this object is not a PiscesFolder "+id);
           }
           return o as PiscesFolder;
        }


        private static List<Type> seriesTypeList = null;

        public Series GetSeries(TimeSeriesDatabaseDataSet.SeriesCatalogRow sr)
        {
            if (db.Settings.ReadBoolean("VerboseLogging", false))
                Logger.EnableLogger();

            Series s = null;// = new Series(sr, db);
            int sdi = sr.id;
            try
            {
                
                
                if (sr.Provider.Trim() == "")
                    sr.Provider = "Series";

                // most common cases -- avoid reflection
                if (sr.Provider == "Series")
                { 
                    s = new Series(db, sr);
                    //s.Table.TableName = 
                    s.Icon = AssignIcon(sr.iconname);
                    s.TimeSeriesDatabase = this.db;
                    return s;
                }
                // most common cases -- avoid reflection
                if (sr.Provider == "CalculationSeries")
                {
                    s = new CalculationSeries(db, sr);
                    s.Icon = AssignIcon(sr.iconname);
                    s.TimeSeriesDatabase = this.db;
                    return s;
                }


                if (seriesTypeList == null)
                {
                    seriesTypeList = new List<Type>();
                    var asmList = AppDomain.CurrentDomain.GetAssemblies();

                    foreach (Assembly item in asmList)
                    {
                        if (item.FullName.IndexOf("Reclamation.") <0
                            && item.FullName.IndexOf("Pisces") <0
                            && item.FullName.IndexOf("HDB") <0 )
                            continue;

                        var types = item.GetTypes().Where(x => x.BaseType == typeof(Series));
                        seriesTypeList.AddRange(types);
                    }
                }

                for (int i = 0; i < seriesTypeList.Count; i++)
                {
                    Type t = seriesTypeList[i];
                    if (t.Name == sr.Provider)
                    {
                        Type[] parmFaster = new Type[] {  typeof(TimeSeriesDatabase), typeof(SeriesCatalogRow) };
                        var cInfoFaster = t.GetConstructor(parmFaster);

                        if (cInfoFaster != null)
                        {
                            object o = cInfoFaster.Invoke(new object[] {  db, sr });
                            if (o is Series)
                                s = o as Series;
                            else
                                throw new InvalidOperationException("Provider '" + sr.Provider + "' is not a Series");
                        }
                       
                            else
                            {
                                throw new InvalidOperationException("Can't find constructor for '" + sr.Provider + "'");
                            }

                        break;
                    }

                }
            }
            catch(Exception excep)
            {
                if (excep.InnerException != null)
                {
                    Logger.WriteLine(excep.InnerException.Message);
                    throw excep.InnerException;
                }
                var msg = excep.Message + "\n" + sr.Provider;
                Logger.WriteLine(msg);
                throw new Exception(msg);
            }

            if (s == null)
            {
//                Logger.WriteLine("No Class found for '"+sr.Provider +"'  ID= "+sr.id+" Name = "+sr.Name);
                s = new Series( db, sr);
            }
            s.Icon = AssignIcon(sr.iconname);
            s.TimeSeriesDatabase = this.db;
            return s;
        }


        public Series GetSeries(int id)
        {
            SeriesCatalogRow si = db.GetSeriesRow(id);
            return GetSeries(si);
        }

        public PiscesObject CreateObject(SeriesCatalogRow sr)
        {
          
            if (sr.IsFolder)
            {
                var v = new PiscesFolder(db, sr);
                v.Icon = AssignIcon(sr.iconname);
                return v;
            }
            else if( sr.IsMeasurement)
            {
                var m = GetMeasurement(sr);
                m.Icon = AssignIcon(sr.iconname);
                return m;
            }
            else
            {
                return GetSeries(sr); //11.53125 seconds elapsed.
                //return GetSeries(sdi, db); //31.96875
            }

            //return new PiscesObject();

        }

        private PiscesObject GetMeasurement(SeriesCatalogRow sr)
        {
            BasicMeasurement bm = new BasicMeasurement(db, sr);
            return bm;
        }



        static private Image AssignIcon(string source)
        {
            if (source == "" )
                return null;
            for (int i = 0; i < IconNames.Length; i++)
            {
                string s = IconNames[i];
                int idx = source.IndexOf(s, StringComparison.CurrentCultureIgnoreCase);
                if (idx >= 0)
                {
                    return m_images[i];
                }
            }
            return null;
        }

        static string[] m_iconNames = null;
        static Image[] m_images = null;

        /// <summary>
        /// A list of image names without an extension.
        /// </summary>
        private static string[] IconNames
        {
            get
            {
                if (m_iconNames == null)
                {
                    string dir = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                    dir = Path.Combine(dir, "images");

                    if (!Directory.Exists(dir))
                        return new string[] { };

                    var lst = new List<string>();
                    var imgList = new List<Image>();
                    DirectoryInfo di = new DirectoryInfo(dir);
                    FileInfo[] files = di.GetFiles();

                    foreach (var f in files)
                    {
                        var ext = f.Extension.ToLower();
                        if (ext == ".ico"
                            || ext == ".bmp"
                            || ext == ".gif")
                        {
                            Bitmap b = null;
                            try
                            {
                                Logger.WriteLine("reading " + f.FullName);
                                Bitmap b1;
                                if (ext == ".gif")
                                {
                                    byte[] ir = File.ReadAllBytes(f.FullName);
                                    Image i = Image.FromStream(new MemoryStream(ir));
                                    b1 = new Bitmap(new Bitmap(i));
                                }
                                else
                                {
                                    b1 = new Bitmap(f.FullName);
                                }
                                b = new Bitmap(b1, new Size(16, 16));

                            }
                            catch (Exception ex)
                            {
                                //System.Windows.Forms.MessageBox.Show(ex.Message);
                                Logger.WriteLine(ex.Message);
                                //m_iconNames = new string[]{};
                                //return m_iconNames;
                                continue;
                            }
                            lst.Add(Path.GetFileNameWithoutExtension(f.FullName));
                            imgList.Add(b);
                        }
                    }
                    m_iconNames = lst.ToArray();
                    m_images = imgList.ToArray();

                }
                return m_iconNames;

            }
        }


    }
}
