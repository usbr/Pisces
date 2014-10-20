using System.Collections.Generic;
using System.Drawing;
using Reclamation.Core;
using System.IO;
using System.Data;
using System;
using System.Text.RegularExpressions;
namespace Reclamation.TimeSeries {
    
    


    public partial class TimeSeriesDatabaseDataSet {

        partial class alarmDataTable
        {
        }

        public partial class seriespropertiesDataTable
        {
            int m_seriesid;

            public int Seriesid
            {
                get { return m_seriesid; }
                set { m_seriesid = value; }
            }
            TimeSeriesDatabase m_db;
            public seriespropertiesDataTable (TimeSeriesDatabase db , int seriesid):base()
	    {
            db.Server.FillTable(this, "Select * from seriesproperties");
            this.TableName = "seriesproperties";
                m_db = db;
                m_seriesid = seriesid;

        }

            public void Save()
            {
                m_db.Server.SaveTable(this);
            }
            public int NextID()
            {
                if (this.Rows.Count > 0)
                {
                    return ((int)this.Compute("Max(id)", "") + 1);
                }
                return 1;

            }

            public bool Contains(string name)
            {
                return Select("name='" + name + "' and seriesid = " + m_seriesid).Length == 1;
            }

            public string Get(string name, string defaultValue)
            {
                var rows = Select("name='"+name+"' and seriesid = "+m_seriesid);
                if (rows.Length != 1)
                    return defaultValue;

                return rows[0]["value"].ToString();
            }

            public void Set(string name, string value)
            {
                var rows = Select("name='" + name + "' and seriesid = " + m_seriesid);
                if (rows.Length == 0)
                {
                    AddseriespropertiesRow(NextID(), m_seriesid, name, value);
                }
                else
                {
                    rows[0]["value"] = value;
                }

            }
        }
        public partial class SeriesCatalogDataTable
        {
            public void AddSeriesCatalogRow(Series s, int id, int parentID, string tableName="")
            {
                AddSeriesCatalogRow(id, parentID, false, 0, s.Source, s.Name, s.SiteName, s.Units,
                    s.TimeInterval.ToString(), s.Parameter, tableName, s.Provider, s.ConnectionString, s.Expression, s.Notes,true);
            }

            public int AddFolder(string folderName, int id, int parentID)
            {
                AddSeriesCatalogRow(id, parentID, true, 0, "", folderName,"", "","","","","","","","",false);
                return id;
            }
            public int AddFolder(string folderName, int parentID)
            {
                int id = NextID();
                AddSeriesCatalogRow(id, parentID, true, 0, "", folderName, "", "", "", "", "", "", "", "", "",false);
                return id;

            }


            public bool FolderExists(string folderName, int parentID)
            {
                DataRow[] foundFolder = this.Select(string.Format("Name = '{0}' and IsFolder = True and parentid = {1}", folderName, parentID));
                if (foundFolder.Length == 1)
                    return true;
                return false;
            }

            public int NextID()
            {
                if ( this.Rows.Count >0)
                {
                    return ((int)this.Compute("Max(id)", "") + 1);
                }
                return 1;

                //return (this.Rows.Count + 1);
            }


           
        }

        public partial class ScenarioRow
        {
             public string GetConnectionStringParameter(string name,string defaultValue="")
            {
             return   ConnectionStringUtility.GetToken(this.Path,name, defaultValue);
            }
        }

        

        partial class ScenarioDataTable
        {

            public string[] GetNames()
            {
                var rval = new List<string>();
                foreach (var item in this)
                {
                    rval.Add(item.Name);
                }
                return rval.ToArray();
            }

            public void WriteLocalXml()
            {
                string xmlPath = FileUtility.GetLocalApplicationPath();
                string xmlFile = "LocalScenarioTable.xml";
                this.WriteXml(Path.Combine(xmlPath, xmlFile));
            }

            public void ReadLocalXml()
            {
                string xmlPath = FileUtility.GetLocalApplicationPath();
                string xmlFile = "LocalScenarioTable.xml";
                this.ReadXml(Path.Combine(xmlPath, xmlFile));
            }
        }

        //static TimeSeriesDatabaseDataSet.SeriesCatalogDataTable s_catalog = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();
        public partial class SeriesCatalogRow : global::System.Data.DataRow
        {
           // public TimeSeriesDatabase TimeSeriesDatabase;

           // internal int FileIndex=0;
            private Image _icon;

        public Image Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        }
    }
}

namespace Reclamation.TimeSeries.TimeSeriesDatabaseDataSetTableAdapters {
    
    
    public partial class SeriesCatalogTableAdapter {


    }
}
