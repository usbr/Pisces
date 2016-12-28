using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using Reclamation.Core;
using System.Data;

namespace Reclamation.TimeSeries
{
    public class SQLiteSeries : Series
    {
        string fileName;

        public SQLiteSeries(TimeSeriesDatabase db, Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow sr):base(db,sr)
        {
            ExternalDataSource = true;
            Init(db);

        }

        private void Init(TimeSeriesDatabase db)
        {
            fileName = ConnectionStringUtility.GetFileName(ConnectionString, db.DataSource);   
            ScenarioName = Path.GetFileNameWithoutExtension(fileName);
        }

        public override Series CreateScenario(TimeSeriesDatabaseDataSet.ScenarioRow scenario)
        {
            if (scenario.Name == ScenarioName)
            {
                return this;
            }
            else
            {
                string fn = ConnectionStringUtility.GetFileName(scenario.Path, m_db.DataSource);
                Logger.WriteLine("Reading series from " + fn);
                var sr = m_db.GetNewSeriesRow(false);
                sr.ItemArray = SeriesCatalogRow.ItemArray;
                
                sr.ConnectionString = ConnectionStringUtility.Modify(sr.ConnectionString, "FileName", fn);
                Series s = new SQLiteSeries(m_db, sr);
                s.ReadOnly = true;
                s.ScenarioName = scenario.Name;
                return s;
            }
        }

        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            SQLiteServer svr = new SQLiteServer(fileName);
            Clear();

            string sql = "SELECT * from " + svr.PortableTableName(this.Table.TableName);

            if (t1 != MinDateTime || t2 != MaxDateTime)
            {
                sql += " WHERE datetime >= " + svr.PortableDateString(t1, TimeSeriesDatabase.dateTimeFormat)
                + " AND "
                + " datetime <= " + svr.PortableDateString(t2, TimeSeriesDatabase.dateTimeFormat);
            }
            sql += " order by datetime ";


            svr.FillTable(this.Table,sql);

        }

        public static void CreatePiscesTree(string filename, PiscesFolder CurrentFolder, TimeSeries.TimeSeriesDatabase db)
        {
            var sqlite = new SQLiteServer(filename);
      
            /*insert into seriescatalog (id,parentid,name,isfolder) values (1,1,"database name",1);
insert into seriescatalog (id,parentid,name,units,tablename)

            */
            Logger.WriteLine("reading " + filename, "ui");
            var variables = sqlite.Table("variables","select a.var_name,b.value AS units from variables a " 
            +"left join attributes b on a.var_name = b.var_name and b.attribute_name = 'units' order by a.var_name DESC");
            Logger.WriteLine("variable count = "+variables.Rows.Count, "ui");
            var sc = db.GetSeriesCatalog();

            string rootFolderName = Path.GetFileNameWithoutExtension(filename);
            var rootID = sc.AddFolder(rootFolderName);
            
            var prev_FolderName = rootFolderName;
            int folderID = rootID;
            int id = sc.NextID();
            var cs = ConnectionStringUtility.MakeFileNameRelative("FileName="+filename+";", db.DataSource);
            foreach (DataRow row in variables.Rows)
            {
                var var_name = row["var_name"].ToString();

                if (var_name.IndexOf("_") > 0)
                {
                    var tokens = var_name.Split('_');
                    if( tokens[0] != prev_FolderName)
                    {
                       folderID = sc.AddFolder(tokens[0], rootID);
                       id++;
                       prev_FolderName = tokens[0];
                    }
                }

                var newRow = sc.NewSeriesCatalogRow();
                newRow.id = id++;
                newRow.ParentID = folderID;
                newRow.Provider = "SQLiteSeries";
                newRow.ConnectionString = cs;
                newRow.IsFolder = 0;
                newRow.TableName =  var_name;
                newRow.Units = row["units"].ToString();
                newRow.Name = var_name;
                newRow.Parameter = row["var_name"].ToString();
                sc.AddSeriesCatalogRow(newRow);
            }

            db.Server.SaveTable(sc);
        }
    }
}
