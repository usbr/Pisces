using Reclamation.Core;
using System;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Text.RegularExpressions;
namespace Reclamation.TimeSeries.Alarms
{
}
namespace Reclamation.TimeSeries.Alarms
{


    public partial class AlarmDataSet
    {
        partial class alarm_definitionDataTable
        {
        }
    
        private Core.BasicDBServer m_server;

        public static AlarmDataSet CreateInstance(BasicDBServer server = null)
        {
            AlarmDataSet rval;
            if (server == null)
            { // create from config files.
                var db = TimeSeriesDatabase.InitDatabase(new Arguments(new string[] { }));
                rval = new AlarmDataSet();
                rval.m_server = db.Server;
            }
            else
            {// create using server
                rval = new AlarmDataSet();
                rval.m_server = server;
            }
            return rval;
        }


        public void SaveTable(DataTable table)
        {
            m_server.SaveTable(table);
        }

        public alarm_listDataTable GetList()
        {
            var tbl = new alarm_listDataTable();
            m_server.FillTable(tbl, "select * from alarm_list order by list");
            return tbl;
        }

        /// <summary>
        /// Gets a list of alarms in priority order for processing
        /// only alarms with status (new, or unconfirmed)
        /// </summary>
        /// <returns></returns>
        public alarm_phone_queueDataTable GetNewAlarms()
        {
            AlarmDataSet.alarm_phone_queueDataTable tbl = new alarm_phone_queueDataTable();
            string sql = "select * from alarm_phone_queue where status='new' or status = 'unconfirmed' order by priority";
            m_server.FillTable(tbl, sql);
            return tbl;
        }


        public string[] GetPhoneNumbers(string list)
        {
            string sql = "select phone from alarm_recipient where list='" + list + "' order by call_order";
            var tbl = m_server.Table("alarm_recipient", sql);
            return DataTableUtility.Strings(tbl, "", "phone");
        }

        public string[] GetEmailList(string list)
        {
            string sql = "select email from alarm_recipient where list='" + list + "' order by call_order";
            var tbl = m_server.Table("alarm_recipient", sql);
            return DataTableUtility.Strings(tbl, "", "email");
        }


        public partial class alarm_scriptsDataTable
        {
            public int NextID()
            {
                if (this.Rows.Count > 0)
                {
                    return ((int)this.Compute("Max(id)", "") + 1);
                }
                return 1;
            }
        }


        public alarm_recipientDataTable GetRecipients(string label)
        {
            var tbl = new alarm_recipientDataTable();
            m_server.FillTable(tbl, "select * from alarm_recipient where list = '" + label + "' order by call_order");
            tbl.idColumn.AutoIncrementSeed = m_server.NextID("alarm_recipient", "id");
            tbl.listColumn.DefaultValue = label;
            return tbl;
        }


        public void AddNewAlarmGroup(string label)
        {
            var tbl = GetList();
            tbl.Addalarm_listRow(label);
            m_server.SaveTable(tbl);
        }

        public void DeleteAlarmGroup(string label)
        {
            DeleteRecipients(label);
            var tbl = GetList();
            var rows = tbl.Select("list = '" + label + "'");
            if (rows.Length == 1)
            {
                rows[0].Delete();
            }
            m_server.SaveTable(tbl);
        }

        public void DeleteRecipients(string label)
        {
            var tbl = GetRecipients(label);
            for (int i = tbl.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = tbl.Rows[i];
                dr.Delete();
            }
            m_server.SaveTable(tbl);
        }

        public AlarmDataSet.alarm_phone_queueDataTable GetAlarmQueue(string siteid, string parameter)
        {
            var tbl = new AlarmDataSet.alarm_phone_queueDataTable();
            string sql = "select * from alarm_phone_queue ";
            sql += " where siteid = '" + siteid + "' and parameter = '" + parameter + "' ";
                sql += " and status in ('new', 'unconfirmed')";

            m_server.FillTable(tbl, sql);

            return tbl;

        }
        public AlarmDataSet.alarm_phone_queueDataTable GetAlarmQueue(bool everything = false)
        {
            var tbl = new AlarmDataSet.alarm_phone_queueDataTable();
            string sql = "select * from alarm_phone_queue ";

            if (!everything)
            {
                sql += " where status in ('new', 'unconfirmed')";
            }

            m_server.FillTable(tbl, sql);

            return tbl;

        }
        /// <summary>
        /// Check each point in the series for an alarm
        /// </summary>
        /// <param name="s"></param>
        internal void Check(Series s)
        {
            var alarm = GetAlarmDefinition(s.SiteID, s.Parameter);
            // is alarm defined
            if (alarm.Rows.Count == 0)
                return;
            if (alarm.Rows.Count > 1)
                throw new Exception("bad... alarm_definition constraint not working (siteid,parameter)");


            AlarmDataSet.alarm_definitionRow row = alarm[0];
            // check alarm_condition for each value

            AlarmRegex alarmEx = new AlarmRegex(row.alarm_condition);

            if (alarmEx.IsMatch())
            {

                foreach(var c in alarmEx.AlarmConditions())
                {
                    if(c.Condition == AlarmType.Above)
                    {
                        foreach (Point p in s)
                        {
                            if (!p.IsMissing && p.Value > c.Value)
                            {
                                    Console.WriteLine("Alarm above found");
                                    CreateAlarm(row, p);
                                    return;
                            }
                        }
                    }

                    if(c.Condition == AlarmType.Below)
                    {
                        foreach (Point p in s)
                        {
                            if (!p.IsMissing && p.Value < c.Value)
                            {
                                Console.WriteLine("Alarm below found");
                                CreateAlarm(row, p);
                                return;
                            }
                        }
                    }

                }

               
            }

            // TO DO  clear alarms if clear_condition



        }

        /// <summary>
        /// make phone calls and send emails
        // 
        /// </summary>
        /// <param name="alarm"></param>
        /// <param name="Alarmvalue"></param>
        private void CreateAlarm(AlarmDataSet.alarm_definitionRow alarm,
                             Point pt)
        {
            var tbl = GetAlarmQueue(alarm.siteid, alarm.parameter);

            if (tbl.Rows.Count == 1)
            {
                Logger.WriteLine("Alarm already active in the queue: " + alarm.siteid + " " + alarm.parameter);
                return;
            }
            
            SendEmail(alarm, pt);
            //phone call by inserting into table alarm_queue

            var row = tbl.Newalarm_phone_queueRow();
            row.id = m_server.NextID("alarm_phone_queue", "id");
            row.list = alarm.list;
            row.siteid = alarm.siteid;
            row.parameter = alarm.parameter;
            row.value = pt.Value;
            row.status = "new";
            row.status_time = DateTime.Now;
            row.confirmed_by = "";
            row.event_time = pt.DateTime;
            row.priority = alarm.priority;
            tbl.Rows.Add(row);
            m_server.SaveTable(tbl);
        }

        private void SendEmail(alarm_definitionRow alarm, Point pt)
        {
            // old:  Alarm condition at site WICEWS for parameter GH -- value = 0.43
            var siteDescription = "";
            var t = m_server.Table("sitecatalog","select description from sitecatalog where siteid='" + alarm.siteid + "'");
            if( t.Rows.Count >0)
               siteDescription = t.Rows[0][0].ToString();

            var parameterName = "";
            t = m_server.Table("parametercatalog","select name from parametercatalog where id='" + alarm.parameter + "' and timeinterval = 'Irregular'");
            if (t.Rows.Count > 0)
                parameterName = t.Rows[0][0].ToString();

            var subject = "Alarm Condition at" + siteDescription+" "+alarm.siteid;
            var body = "Alarm condition at site" + alarm.siteid + "   parameter = " + alarm.parameter;

            var emails = GetEmailList(alarm.list);
            SendEmail(emails, subject, body);


        }

        private static void SendEmail(string[] address, string subject, string body)
        {
            MailMessage msg = new MailMessage();
            foreach (var item in address)
            {
                msg.To.Add(item);
            }

            string reply = "";
            if (ConfigurationManager.AppSettings["email_reply"] != null)
                reply = ConfigurationManager.AppSettings["email_reply"];
            if (reply == "")
            {
                Console.WriteLine("");
                return;
            }
            msg.From = new MailAddress(reply);
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;
            SmtpClient c = new System.Net.Mail.SmtpClient();
            c.Host = ConfigurationManager.AppSettings["smtp"];
            c.Send(msg);


            Logger.WriteLine("mail server " + c.Host);
            Logger.WriteLine("to : " + address);
            Logger.WriteLine("from : " + msg.From.Address);
            Logger.WriteLine("message sent ");
            Logger.WriteLine(body);
        }
        public alarm_definitionDataTable GetAlarmDefinition()
        {
            var alarm_definition = new AlarmDataSet.alarm_definitionDataTable();
            var sql = "select * from alarm_definition ";
            m_server.FillTable(alarm_definition, sql);
            alarm_definition.idColumn.AutoIncrementSeed = m_server.NextID("alarm_definition", "id");
            return alarm_definition;
        }
        public alarm_definitionDataTable GetAlarmDefinition(string siteid, string parameter)
        {
            var alarm_definition = new AlarmDataSet.alarm_definitionDataTable();

            siteid = PostgreSQL.SafeSqlLikeClauseLiteral(siteid);
            parameter = SqlServer.SafeSqlLikeClauseLiteral(parameter);
            var sql = "select * from alarm_definition where siteid='" + siteid + "'"
                    + " and parameter ='" + parameter + "'";
            m_server.FillTable(alarm_definition, sql);

            alarm_definition.idColumn.AutoIncrementSeed = m_server.NextID("alarm_definition", "id");
            return alarm_definition;

        }



    }
}



