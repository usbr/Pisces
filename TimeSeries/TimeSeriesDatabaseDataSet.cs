using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Reclamation.TimeSeries
{




    public partial class TimeSeriesDatabaseDataSet
    {

        partial class alarmDataTable
        {
        }

        public partial class sitepropertiesDataTable
        {

            TimeSeriesDatabase m_db;
            public sitepropertiesDataTable(TimeSeriesDatabase db)
                : base()
            {
                string sql = "Select * from siteproperties";

                db.Server.FillTable(this, sql);
                this.TableName = "siteproperties";
                m_db = db;
            }

            public string GetValue(string siteid, string key, string defaultValue = "")
            {
                var d = GetDictionary(siteid);
                if (d.ContainsKey(key))
                {
                    return d[key].ToString();
                }
                return defaultValue;
            }
            public Dictionary<string, string> GetDictionary(string siteid)
            {
                var rval = new Dictionary<string, string>();
                var rows = Select("siteid = '" + siteid + "'");
                foreach (var item in rows)
                {
                    rval.Add(item["name"].ToString(), item["value"].ToString());
                }
                return rval;
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

            public bool Contains(string name, string siteid)
            {
                return Select("name='" + name + "' and siteid = '" + siteid + "'").Length == 1;
            }

            public string Get(string name, string defaultValue, string siteid)
            {
                var rows = Select("name='" + name + "' and siteid = '" + siteid + "'");
                if (rows.Length != 1)
                    return defaultValue;

                return rows[0]["value"].ToString();
            }

            public void Set(string name, string value, string siteid)
            {
                var rows = Select("name='" + name + "' and siteid = '" + siteid + "'");
                if (rows.Length == 0)
                {
                    AddsitepropertiesRow(NextID(), siteid, name, value);
                }
                else
                {
                    rows[0]["value"] = value;
                }

            }

        }


        public partial class seriespropertiesDataTable
        {

            TimeSeriesDatabase m_db;
            public seriespropertiesDataTable(TimeSeriesDatabase db) : base()
            {
                string sql = "Select * from seriesproperties";

                db.Server.FillTable(this, sql);
                this.TableName = "seriesproperties";
                m_db = db;
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

            public bool Contains(string name, int seriesID)
            {
                return Select("name='" + name + "' and seriesid = " + seriesID).Length == 1;
            }

            public string Get(string name, string defaultValue, int seriesID)
            {
                var rows = Select("name='" + name + "' and seriesid = " + seriesID);
                if (rows.Length > 1)
                    Console.WriteLine("Warning: duplicate property " + name + " seriesid = " + seriesID);

                if (rows.Length == 0)
                    return defaultValue;

                return rows[0]["value"].ToString();
            }


            /// <summary>
            /// Delete specified item from the series properites
            /// </summary>
            /// <param name="name"></param>
            /// <param name="seriesID"></param>
            /// <returns></returns>
            public void Delete(string name, int seriesID)
            {
                var rows = Select("name='" + name + "' and seriesid = " + seriesID);
                if (rows.Length > 1)
                    Console.WriteLine("Warning: duplicate property " + name + " seriesid = " + seriesID);

                if (rows.Length == 0)
                    return;

                rows[0].Delete();
            }




            public static void Set(string name, string value, TimeSeriesName tn, BasicDBServer svr)
            {
                var tableName = tn.GetTableName();
                var sc = svr.Table("seriescatalog", "select * from seriescatalog where tablename = '" + tableName + "'");
                if (sc.Rows.Count == 1)
                {
                    int id = Convert.ToInt32(sc.Rows[0]["id"]);
                    Set(name, value, id, svr);
                }
                else
                {
                    var msg = "Error: tablename:" + tableName + "not found (or duplicated) in the seriescatalog";
                    Logger.WriteLine(msg);
                    throw new KeyNotFoundException(msg);
                }
            }
            /// <summary>
            /// Set property directly to database
            /// </summary>
            public static void Set(string name, string value, int seriesID, BasicDBServer svr)
            {
                var tbl = new TimeSeriesDatabaseDataSet.seriespropertiesDataTable();
                var sql = "Select * from seriesproperties where name='" + name + "' and seriesid = " + seriesID;
                svr.FillTable(tbl, sql);

                if (tbl.Rows.Count == 0)
                {
                    tbl.AddseriespropertiesRow(svr.NextID("seriesproperties", "id"), seriesID, name, value);
                }
                else
                {
                    tbl.Rows[0]["value"] = value;
                }

                svr.SaveTable(tbl);
            }


            /// <summary>
            /// Set property to in memory 
            /// </summary>
            /// <param name="name"></param>
            /// <param name="value"></param>
            /// <param name="seriesID"></param>
            public void Set(string name, string value, int seriesID)
            {
                var rows = Select("name='" + name + "' and seriesid = " + seriesID);
                if (rows.Length == 0)
                {
                    AddseriespropertiesRow(NextID(), seriesID, name, value);
                }
                else
                {
                    rows[0]["value"] = value;

                    if (rows.Length > 1)
                    {// delete duplicates.
                        for (int i = 1; i < rows.Length; i++)
                        {
                            rows[i].Delete();
                            //rows[0].Table.Rows.Remove(rows[i]);   
                        }
                    }

                }
            }

            /// <summary>
            /// Copies properties from one series to another
            /// </summary>
            internal void DuplicateProperties(int currentID, int newID)
            {
                var rows = Select("seriesid = " + currentID);
                foreach (var item in rows)
                {
                    AddseriespropertiesRow(NextID(), newID, item["name"].ToString(), item["value"].ToString());
                }
            }
            /// <summary>
            /// Delets all properties for a given id
            /// </summary>
            /// <param name="id"></param>
            internal void DeleteAll(int id)
            {
                var rows = Select("seriesid = " + id);

                foreach (var item in rows)
                {
                    item.Delete();
                }
            }
        }

        public partial class sitecatalogDataTable
        {

            public void AddsitecatalogRow(string siteid, string description, string state)
            {
                AddsitecatalogRow(siteid, description, state, 0, 0, 0, "", "", "", "", 0, "", "", "", "", "", "");
            }

            public bool Exists(string siteid)
            {
                var rows = Select("siteid = '" + siteid + "'");
                if (rows.Length == 1)
                    return true;
                if (rows.Length > 1)
                    Console.WriteLine("ERROR site exists more than onece " + siteid);
                return false;
            }
        }
        public partial class SeriesCatalogDataTable
        {
            BasicDBServer m_svr;

            public BasicDBServer Server
            {
                get { return m_svr; }
                set { m_svr = value; }
            }
            
            public void Save()
            {
                if (m_svr != null)
                    m_svr.SaveTable(this);
                else
                    Logger.WriteLine("server is not set.  Can't save this way");
            }
            public SeriesCatalogRow AddSeriesCatalogRow(Series s, int id, int parentID, string tableName = "")
            {
                if (tableName == "")
                    tableName = s.Table.TableName;
                if (tableName == "")
                    tableName = "ts_" + Guid.NewGuid();

                var rval = AddSeriesCatalogRow(id, parentID, 0, 0, s.Source, s.Name, s.SiteID, s.Units,
                    s.TimeInterval.ToString(), s.Parameter, tableName, s.Provider, s.ConnectionString, s.Expression, s.Notes, 1);
                return rval;
            }

            public string[] GetRootFolderNames()
            {
                var rows = this.Select("id = parentid");
                var rval = new List<string>();

                if (rows.Length == 0)
                {
                    Reclamation.Core.Logger.WriteLine("Tree Requires at least one root Node");
                    Logger.WriteLine("AutoCreation of New Root Folder");
                    AddFolder("root");
                    return new string[] { "root" };
                }

                else
                {
                    foreach (var item in rows)
                    {
                        rval.Add(item["name"].ToString());
                    }
                }
                return rval.ToArray();
            }


            public int AddFolder(string folderName, int id, int parentID)
            {
                AddSeriesCatalogRow(id, parentID, 1, 0, "", folderName, "", "", "", "", "", "", "", "", "", 0);
                return id;
            }
            public int AddFolder(string folderName, int parentID = -1)
            {
                int id = NextID();
                if (parentID == -1)
                    parentID = id;
                AddSeriesCatalogRow(id, parentID, 1, 0, "", folderName, "", "", "", "", "", "", "", "", "", 0);
                return id;
            }

            /// <summary>
            /// returns path (list of directories) to the specified series
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public string[] GetPath(int id)
            {
                var rval = new List<string>();

                var row = FindByid(id);
                do
                {
                    row = FindByid(row.ParentID);
                    if (row == null || row.Name == null)
                    {
                        Console.WriteLine("yikes! parent does not exist");
                        rval.Reverse();
                        return rval.ToArray();
                    }
                    rval.Add(row.Name);

                } while (row.ParentID != row.id); // ids equal at root level of tree

                rval.Reverse();
                return rval.ToArray();
            }


            int prevFolderID = -1;
            string prevFolderKey = "";

            public int GetOrCreateFolder(params string[] folderNames)
            {
                for (int i = 0; i < folderNames.Length; i++)
                {
                    folderNames[i] = folderNames[i].ToLower();
                }
                var key = String.Join(",", folderNames);
                if (prevFolderID >= 0 && key == prevFolderKey)
                    return prevFolderID;


                int rval = -1;
                for (int i = 0; i < folderNames.Length; i++)
                {
                    var fn = folderNames[i];
                    if (FolderExists(fn, rval))
                    {
                        rval = FolderID(fn, rval);
                        Logger.WriteLine(" found existing folder '" + fn + "'");
                    }
                    else
                    {
                        rval = AddFolder(fn, rval);
                        Logger.WriteLine("Creating folder '" + fn + "'");
                    }
                }

                prevFolderID = rval;
                prevFolderKey = key;
                return rval;
            }

            private int FolderID(string name, int parentid = -1)
            {
                string sql = "name = '" + name + "' and isfolder = true ";
                if (parentid != -1)
                    sql += " and parentid = " + parentid;
                DataRow[] foundFolder = this.Select(sql);
                if (foundFolder.Length == 1)
                    return Convert.ToInt32(foundFolder[0]["id"]);

                return -1;
            }

            public bool FolderExists(string folderName, int parentID = -1)
            {
                return FolderID(folderName, parentID) >= 0;
            }

            public int NextID()
            {
                if (this.Rows.Count > 0)
                {
                    return ((int)this.Compute("Max(id)", "") + 1);
                }
                return 1;

                //return (this.Rows.Count + 1);
            }

            public SeriesCatalogRow GetParent(SeriesCatalogRow row)
            {
                var items = this.Select("id = " + row.ParentID);
                if (items.Length == 1)
                    return items[0] as SeriesCatalogRow;

                return null;
            }

            public int AddRow(string siteID, int parentid, string units, string pcode, string expression = "", TimeInterval interval= TimeInterval.Irregular)
            {
                var provider = "Series";
                string iconName = "";
                if (expression != "")
                {
                    provider = "CalculationSeries";
                    iconName = "sum";
                }
                var tn = new TimeSeriesName(siteID + "_" + pcode,interval);

                string tableName = tn.GetTableName();

                var rows = Select("tablename = '" + tableName + "'");
                if (rows.Length > 0)
                    Console.WriteLine("Warning table:'" + tableName + "' allready exists");

                int rval = NextID();
                AddSeriesCatalogRow(rval, parentid, 0, 1, iconName, siteID + "_" + pcode, siteID, units,interval.ToString(),
                 pcode, tableName, provider, "", expression, "", 1);
                return rval;
            }


           
        }

        public partial class ScenarioRow
        {
            public string GetConnectionStringParameter(string name, string defaultValue = "")
            {
                return ConnectionStringUtility.GetToken(this.Path, name, defaultValue);
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
            private object _icon;

            public Object Icon
            {
                get { return _icon; }
                set { _icon = value; }
            }


        }


    }
}

namespace Reclamation.TimeSeries.TimeSeriesDatabaseDataSetTableAdapters
{


    public partial class SeriesCatalogTableAdapter {


    }
}
