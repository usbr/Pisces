using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.Core;

namespace Reclamation.TimeSeries
{
    public partial class TimeSeriesDatabase
    {

        private void ExecuteCreateTable(BasicDBServer svr , string sql)
        {
            svr.CreateTable(sql);
        }

        private void CreateTablesWithSQL()
        {
            CreatePiscesInfoTable();
            CreateSeriesCatalogTable();
            CreateScenarioTable();
            CreateLimitTable();
            CreateSiteTable();
            CreateSitePropertiesTable();
            CreateParameterCatalogTable();
            CreateSeriesPropertiesTable();

            if (m_settings.GetDBVersion() >= 3)
            {
                CreateAlarmGroups();
                CreateMeasurementTable();
                CreateRatingTable();
                CreateAlarmRecipient();
                CreateAlarmDef();
                CreateAlarmPhoneQueue();
                CreateAlarmScripts();
            }

        }

        private void CreateRatingTable()
        {
            if (!m_server.TableExists("rating_tables"))
            {
                string sql = "Create Table rating_tables"
                + " (id int not null primary key, "
                + " version " + m_server.PortableCharacterType(256) + " not null , "
                + " siteid " + m_server.PortableCharacterType(256) + " not null default '', "
                + " x_variable " + m_server.PortableCharacterType(256) + " not null default '', "
                + " y_variable " + m_server.PortableCharacterType(256) + " not null default '', "
                + " csv_table " + m_server.PortableCharacterType(100000) + " not null default '' "
                + " )";
                ExecuteCreateTable(m_server, sql);
            }
        }



        private void CreateAlarmGroups()
        {

            if (!m_server.TableExists("alarm_list"))
            {
                string sql = "Create Table alarm_list"
                + " ( list " + m_server.PortableCharacterType(256) + " not null  primary key "
                + " )";
                ExecuteCreateTable(m_server, sql);
            }
        }
        private void CreateAlarmRecipient()
        {
            if (!m_server.TableExists("alarm_recipient"))
            {
                string sql = "create table alarm_recipient ("
                        + " id int not null primary key, "
                        + " list    " + m_server.PortableCharacterType(20) + " not null default '', "
                        + " call_order int not null default 0,"
                        + " phone    " + m_server.PortableCharacterType(20) + " not null default '', "
                        + " name    " + m_server.PortableCharacterType(20) + " not null default '', "
                        + " email    " + m_server.PortableCharacterType(20) + " not null default '' ) ";
                ExecuteCreateTable(m_server,sql);
            }
        }

        private void CreateAlarmPhoneQueue()
        {

            if (!m_server.TableExists("alarm_phone_queue"))
            {
                string sql = "Create Table alarm_phone_queue "
                + "( id  int not null primary key, "
                + " list " + m_server.PortableCharacterType(256) + " not null default '', "
                + " siteid " + m_server.PortableCharacterType(256) + " not null default '', "
                + " parameter " + m_server.PortableCharacterType(256) + " not null default '', "
                + " value " + m_server.PortableFloatType() + " not null, "
                + " status " + m_server.PortableCharacterType(256) + " not null default '', "
                + " status_time " + m_server.PortableDateTimeType() + " not null , "
                + " confirmed_by " + m_server.PortableCharacterType(256) + " not null default '', "
                + " event_time " + m_server.PortableDateTimeType() + " not null  ,"
                + " priority int not null default 10 "
                + " )";
                ExecuteCreateTable(m_server, sql);
            }

        }

        private void CreateAlarmScripts()
        {

            if (!m_server.TableExists("alarm_scripts"))
            {
                string sql = "Create Table alarm_scripts "
                + "( id  int not null primary key, "
                + " text " + m_server.PortableCharacterType(2048) + " not null default '', "
                + " filename " + m_server.PortableCharacterType(2048) + " not null default '' "
                + " )";
                ExecuteCreateTable(m_server, sql);
            }

        }

        private void CreateAlarmDef()
        {

            if (!m_server.TableExists("alarm_definition"))
            {
                string sql = "Create Table alarm_definition "
                + "( id  int not null primary key, "
                + " enabled boolean not null default false, "
                + " list " + m_server.PortableCharacterType(256) + " not null default '', "
                + " siteid " + m_server.PortableCharacterType(256) + " not null default '', "
                + " parameter " + m_server.PortableCharacterType(256) + " not null default '', "
                + " alarm_condition " + m_server.PortableCharacterType(256) + " not null default '', "
                + " clear_condition " + m_server.PortableCharacterType(256) + " not null default '', "
                + " priority int not null default 10 "
                + " , unique(siteid,parameter) )";
                ExecuteCreateTable(m_server, sql);
            }
        }

        private void CreateMeasurementTable()
        {
            if (!m_server.TableExists("measurement"))
            {
                string sql = "Create Table measurement "
                + "( id  int not null primary key, "
                + " siteid " + m_server.PortableCharacterType(256) + " not null default '', "
                + " date_measured " + m_server.PortableDateTimeType() + " not null , "
                + " stage "+m_server.PortableFloatType() +" not null, "
                + " discharge " + m_server.PortableFloatType() + " not null, "
                + " quality " + m_server.PortableCharacterType(30) + " not null default '', "
                + " party " + m_server.PortableCharacterType(30) + " not null default '', "
                + " notes " + m_server.PortableCharacterType(300) + " not null default '' "
                + " )";
                ExecuteCreateTable(m_server, sql);

            }
        }

        private void CreateSiteTable()
        {
            if (!m_server.TableExists("sitecatalog"))
            {
                /*CREATE TABLE sitecatalog ( siteid   nvarchar(256)  not null primary key,  description  nvarchar(1024)  not null default '',  state  nvarchar(30)  not null default '',
  latitude  float  default 0,  longitude  float  default 0,  elevation  float  default 0,
  timezone  nvarchar(30)  not null default '',  install  nvarchar(30)  not null default '' , horizontal_datum nvarchar(30)  not null default '',
 vertical_datum nvarchar(30)  not null default '', vertical_accuracy float not null default 0, elevation_method nvarchar(100)  not null default '',
 tz_offset nvarchar(10)  not null default '', active_flag nvarchar(1) not null default 'T', type nvarchar(100) not null default '', responsibility nvarchar(30) not null default ''  );
                */

                string sql = "Create Table sitecatalog "
                + "( siteid  " + m_server.PortableCharacterType(255) + " not null primary key, "  
                + " description " + m_server.PortableCharacterType(1024) + " not null default '', "  
                + " state " + m_server.PortableCharacterType(30) + " not null default '', "
                + " latitude float default 0, "
                + " longitude float default 0, "
                + " elevation float default 0, "
                + " timezone " + m_server.PortableCharacterType(30) + " not null default '', "
                + " install " + m_server.PortableCharacterType(30) + " not null default '', "
                + " horizontal_datum " + m_server.PortableCharacterType(30) + "  not null default '',"
                + " vertical_datum " + m_server.PortableCharacterType(30) + "  not null default '', "
                + " vertical_accuracy float not null default 0,  "
                + " elevation_method " + m_server.PortableCharacterType(100) + "  not null default '', "
                + " tz_offset " + m_server.PortableCharacterType(10) + "  not null default '',  "
                + " active_flag " + m_server.PortableCharacterType(1) + " not null default 'T', "
                + " type " + m_server.PortableCharacterType(100) + " not null default '', "
                + " responsibility " + m_server.PortableCharacterType(30) + " not null default '' ,"
                + " agency_region " + m_server.PortableCharacterType(30) + " not null default '' "

                + " )";
                ExecuteCreateTable(m_server, sql);

            }
        }

        private void CreateSitePropertiesTable()
        {
            if (!m_server.TableExists("siteproperties"))
            {
                string sql = "Create Table siteproperties "
                               + "( id int not null primary key, "
                               + " siteid " + m_server.PortableCharacterType(256) + " not null default '' , "
                               + " name " + m_server.PortableCharacterType(1024) + " not null default '', "
                               + " value " + m_server.PortableCharacterType(1024) + " not null default '' "
                               + " )";
                ExecuteCreateTable(m_server, sql);

            } 

        }

        private void CreateSeriesPropertiesTable()
        {
            if (!m_server.TableExists("seriesproperties"))
            {
                string sql = "Create Table seriesproperties "
                + "( id int not null primary key, "
                + " seriesid int not null default 0, " // + m_server.PortableCharacterType(256) + " not null default 0, "
                + " name " + m_server.PortableCharacterType(100) + " not null default '', "
                + " value " + m_server.PortableCharacterType(100) + " not null default '' "


                + " )";
                ExecuteCreateTable(m_server, sql);

                
                sql = " ALTER TABLE seriesproperties  "
                    + " ADD UNIQUE INDEX idx1 (seriesid ASC, `name` ASC)"; //mysql

                if (m_server is MySqlServer)
                    m_server.RunSqlCommand(sql);

                sql = " ALTER TABLE siteproperties "
                    + " ADD CONSTRAINT idx1 UNIQUE(siteid, name);"; // postgresql

                if( m_server is PostgreSQL)
                   m_server.RunSqlCommand(sql);

                // to do sqlite
            }
        }

        private void CreateParameterCatalogTable()
        {
            if (!m_server.TableExists("parametercatalog"))
            {
                string sql = "Create Table parametercatalog "
                + "( id  " + m_server.PortableCharacterType(100) + " not null, "
                + " timeinterval " + m_server.PortableCharacterType(1024) + " not null default '', "
                + " units " + m_server.PortableCharacterType(1024) + " not null default '', "
                + " statistic " + m_server.PortableCharacterType(1024) + " not null default '' ,"
                + " name " + m_server.PortableCharacterType(1024) + " not null default '', "
                + " CONSTRAINT parametercatalog_pkey PRIMARY KEY (id, timeinterval) "
                + " )";
                ExecuteCreateTable(m_server, sql);
 
            }
        }

        private void CreatePiscesInfoTable()
        {
            if (!m_server.TableExists("piscesinfo"))
            {
                string sql = "Create Table piscesinfo "
                + "( name  "+m_server.PortableCharacterType(255)+" not null primary key, "
                + " value "+m_server.PortableCharacterType(1024)+" not null default '' "
                + " )";
                ExecuteCreateTable(m_server, sql);

                
                sql = "insert INTO piscesinfo values ('FileVersion', '4')";
                m_server.RunSqlCommand(sql);
            }

            InitSettings();
            //if (m_settings.GetDBVersion() != 2)
            //{
            //    m_settings.Set("FileVersion", 2);
            //    m_settings.Save();
            //}
        }



        internal void CreateSeriesTable(string tableName, bool hasFlags)
        {
            string dataType = m_server.PortableFloatType();

            string sql = "Create Table " + m_server.PortableTableName(tableName);
            if (!hasFlags)
            {
                sql += "( datetime " + m_server.PortableDateTimeType() + " primary key, value " + dataType + " )";
            }
            else
            {
                sql += "( datetime " + m_server.PortableDateTimeType() + " primary key, value " + dataType + ", flag " + m_server.PortableCharacterType(50) + " )";
            }
            ExecuteCreateTable(m_server,sql);
        }

        ///// <summary>
        ///// Create list of Dates to simplify queries
        ///// </summary>
        //private void CreateCalendarTables()
        //{
        //    if (m_server.TableExists("DailyCalendar"))
        //        return;
        //    var sql = "Create Table [DailyCalendar] "
        //        + " ( DateTime DateTime primary key  )";

        //    m_server.RunSqlCommand(sql);

        //    var tbl = m_server.Table("DailyCalendar");
        //    DateTime t = MinDateTime.Date;
        //    DateTime t2 = new DateTime(2100,12,31);
        //    while (t < t2)
        //    {
        //        tbl.Rows.Add(t);
        //        t = t.AddDays(1).Date;
        //    }
        //    m_server.SaveTable(tbl);
        //}


        private void CreateScenarioTable()
        {
            if (m_server.TableExists("scenario"))
                return;

            string sql = "Create Table scenario "
                         + " ( "
                         + " sortorder int not null primary key default 0,"
                         + " name " + m_server.PortableCharacterType(200) + " not null default '',"
                         + "  path " + m_server.PortableCharacterType(1024) + " not null default '', "
                         + "  checked smallint not null default 0, "
                         + "  sortmetric float not null default 0)";
            ExecuteCreateTable(m_server, sql);
        }



        private void CreateSeriesCatalogTable()
        {
            if (m_server.TableExists("seriescatalog"))
            {
                return;
            }
            string sql = "Create Table seriescatalog "
                + " ( "
                + " id int not null primary key ,"
                + " parentid int not null default 0, "
                + " isfolder smallint not null default 0, "
                + " sortorder int not null default 0,"
                + " iconname " + m_server.PortableCharacterType(100) + " not null default '',"
                + " name " + m_server.PortableCharacterType(200) + " not null default '',"
                + " siteid " + m_server.PortableCharacterType(2600) + " not null default '',"
                + " units " + m_server.PortableCharacterType(100) + " not null default '',"
                + " timeinterval " + m_server.PortableCharacterType(100) + " not null default 'irregular',"
                + " parameter " + m_server.PortableCharacterType(100) + " not null default '',"
                + " tablename " + m_server.PortableCharacterType(128) + " not null default '',"
//                + " fileindex int not null default 0," // main file is 0, next 1, etc..
                + " provider " + m_server.PortableCharacterType(200) + " not null default '', "
                + " connectionstring " + m_server.PortableCharacterType(2600) + " not null default '', "
                + " expression " + m_server.PortableCharacterType(2048) + " not null default '', "
                + " notes " + m_server.PortableCharacterType(2048) + " not null default '', "
              //  + " acl " + m_server.PortableCharacterType(50)+ " not null default '',"
                + " enabled smallint not null default 1"
                + "  )";

            ExecuteCreateTable(m_server, sql);
        }

      

        private void CreateLimitTable()
        {
            if (m_server.TableExists("quality_limit"))
            {
                return;
            }
            string sql = "Create Table quality_limit "
                + " ( "
                + " tablemask " + m_server.PortableCharacterType(100) + " not null primary key,"
                + " high float null default null, "
                + " low float null default null, "
                + " delta float null default null "
                + "  )";

            ExecuteCreateTable(m_server, sql);
        }
        //private void CreateCalculationTable()
        //{
        //    if (m_server.TableExists("calculation"))
        //    {
        //        return;
        //    }
        //    string sql = "Create Table calculation "
        //        + " ( "
        //        + " tablemask " + m_server.PortableCharacterType(100) + " not null primary key,"
        //        + " expression "+ m_server.PortableCharacterType(1024) +" not null default '' , "
        //        + " interval " + m_server.PortableCharacterType(20) + " not null default 'instant' , "
        //        + " group_name " + m_server.PortableCharacterType(20) + " not null default ''  "
        //        + "  )";

        //    ExecuteCreateTable(m_server, sql);
        //}

       
    }
}
