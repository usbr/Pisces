using Reclamation.Core;
using System;
using System.Collections.Generic;
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
            if( server != null)
            Logger.WriteLine("AlarmDataSet.CreateInstance("+server.Name+")");
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
        /// Gets a list of alarms that need to be procesed.
        /// </summary>
        /// <returns></returns>
        public alarm_phone_queueDataTable GetUnconfirmedAlarms()
        {
            AlarmDataSet.alarm_phone_queueDataTable tbl = new alarm_phone_queueDataTable();
            string sql = "select * from alarm_phone_queue where (status='new' or status = 'unconfirmed') and active ="+m_server.PortableWhereBool(true);
            m_server.FillTable(tbl, sql);
            return tbl;
        }


        public string[] GetPhoneNumbers(string list)
        {
            string sql = "select phone from alarm_recipient where list='" + list + "' order by call_order";
            var tbl = m_server.Table("alarm_recipient", sql);
            var a= DataTableUtility.Strings(tbl, "", "phone");
            return RemoveEmptyStrings(a);
        }

        public string[] GetEmailList(string list)
        {
            string sql = "select email from alarm_recipient where list='" + list + "' order by call_order";
            var tbl = m_server.Table("alarm_recipient", sql);
            var a = DataTableUtility.Strings(tbl, "", "email");
            return RemoveEmptyStrings(a);
        }

        private static string[] RemoveEmptyStrings(string[] a)
        {
            var rval = new List<string>();

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i].Trim() != "")
                    rval.Add(a[i].Trim());
            }
            return rval.ToArray();
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

        /// <summary>
        /// Gets list of active alarms.
        /// </summary>
        /// <param name="siteid"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public AlarmDataSet.alarm_phone_queueDataTable GetAlarmQueue(int alarm_definition_id)
        {
            var tbl = new AlarmDataSet.alarm_phone_queueDataTable();
            string sql = "select * from alarm_phone_queue ";
            sql += " where alarm_definition_id = " + alarm_definition_id;
            sql += " and active=" + m_server.PortableWhereBool(true);
                

            m_server.FillTable(tbl, sql);

            return tbl;

        }
        public AlarmDataSet.alarm_phone_queueDataTable GetAlarmQueue(bool everything = false)
        {
            var tbl = new AlarmDataSet.alarm_phone_queueDataTable();
            string sql = "select * from alarm_phone_queue ";

            if (!everything)
            {
                sql += " where active="+m_server.PortableWhereBool(true);
            }

            m_server.FillTable(tbl, sql);

            return tbl;

        }
        /// <summary>
        /// Check each point in the series for an alarm
        /// If there is an alarm condition add entry to alarm_phone_queue
        /// </summary>
        /// <param name="s"></param>
        internal void Check(Series s)
        {
            Logger.WriteLine("Check for alarms " + s.SiteID + " " + s.Parameter);
            var alarm = GetActiveAlarmDefinition(s.SiteID.ToLower(), s.Parameter.ToLower());

            if (alarm.Rows.Count == 0)
            {
                Logger.WriteLine("no alarms defined." + s.SiteID + "/" + s.Parameter);
                return;// no alarm defined
            }

            foreach (var item in alarm.Rows) 
            {
                Check(s, (AlarmDataSet.alarm_definitionRow)item);    
            }
            

            // TO DO  clear alarms if clear_condition

        }

        private void Check(Series s, alarm_definitionRow alarm)
        {
            Logger.WriteLine("found alarm definition " + s.SiteID + " " + s.Parameter);

            AlarmRegex alarmEx = new AlarmRegex(alarm.alarm_condition);

            if (alarmEx.IsMatch())
            {

                foreach (var c in alarmEx.AlarmConditions())
                {
                    if (c.Condition == AlarmType.Above)
                    {
                        foreach (Point p in s)
                        {
                            if (!p.FlaggedBad && !p.IsMissing && p.Value > c.Value)
                            {
                                Logger.WriteLine("alarm_condition: " + alarm.alarm_condition);
                                Logger.WriteLine("Alarm above found: " + p.Value);
                                CreateAlarm(alarm, p);
                                return;
                            }
                        }
                    }

                    if (c.Condition == AlarmType.Below)
                    {
                        foreach (Point p in s)
                        {
                            if (!p.FlaggedBad && !p.IsMissing && p.Value < c.Value)
                            {
                                Console.WriteLine("Alarm below found");
                                CreateAlarm(alarm, p);
                                return;
                            }
                        }
                    }

                    if (c.Condition == AlarmType.Dropping)
                    {
                        // TO DO.. fix assumption of 4 values...
                        // check for flags.
                        double num_a = s[0].Value;
                        double num_b = s[1].Value;
                        double num_c = s[2].Value;
                        double num_d = s[3].Value;

                        if ((num_a - num_b) > c.Value
                            | (num_b - num_c) > c.Value
                            | (num_c - num_d) > c.Value)
                        {
                            Console.WriteLine("Alarm dropping found");
                            CreateAlarm(alarm, s[0]);
                            return;
                        }
                    }

                    if (c.Condition == AlarmType.Rising)
                    {
                        // TO DO.. fix assumption of 4 values...
                        // check for flags.
                        double num_a = s[0].Value;
                        double num_b = s[1].Value;
                        double num_c = s[2].Value;
                        double num_d = s[3].Value;

                        if ((num_b - num_a) > c.Value
                            | (num_c - num_b) > c.Value
                            | (num_d - num_c) > c.Value)
                        {
                            Console.WriteLine("Alarm dropping found");
                            CreateAlarm(alarm, s[0]);
                            return;
                        }
                    }
                }

            }
        }

        
        /// <summary>
        ///  Check for alarm condition. If an alarm condition is found
        ///  create an entry in the alarm_phone_queue.
        /// </summary>
        /// <param name="alarm"></param>
        /// <param name="pt"></param>
        public void CreateAlarm(AlarmDataSet.alarm_definitionRow alarm,
                             Point pt)
        {
            var tbl = GetAlarmQueue(alarm.id);

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
            row.alarm_definition_id = alarm.id;
            row.siteid = alarm.siteid;
            row.parameter = alarm.parameter;
            row.value = pt.Value;
            row.status = "new";
            row.status_time = DateTime.Now;
            row.confirmed_by = "";
            row.event_time = pt.DateTime;
            row.current_list_index = -1;// queue manager will increment ++
            row.active = true;
            tbl.Rows.Add(row);
            m_server.SaveTable(tbl);
        }

        private void SendEmail(alarm_definitionRow alarm, Point pt)
        {
            // old:  Alarm condition at site WICEWS for parameter GH -- value = 0.43
            var siteDescription = "";
            var t = m_server.Table("sitecatalog", "select description from sitecatalog where siteid='" + alarm.siteid + "'");
            if (t.Rows.Count > 0)
                siteDescription = t.Rows[0][0].ToString();

            var parameterName = "";
            t = m_server.Table("parametercatalog", "select name from parametercatalog where id='" + alarm.parameter + "' and timeinterval = 'Irregular'");
            if (t.Rows.Count > 0)
                parameterName = t.Rows[0][0].ToString();

            var subject = "Alarm Condition at " + siteDescription + " " + alarm.siteid.ToUpper();
            subject += "  " + parameterName;
            var body = "alarm condition: "+ alarm.alarm_condition;
            body += "\n<br/>"+ pt.ToString();
            body += "\n<br/>\n<br/>" + subject;

            var emails = GetEmailList(alarm.list);
             if( emails.Length == 0)
             {
                 Logger.WriteLine("no emails found for list='"+alarm.list+"'");
                 Logger.WriteLine("subject: " + subject);
                 Logger.WriteLine("body: " + body);
             }
             else
             {
            SendEmail(emails, subject, body);
             }


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
                Console.WriteLine("Error: email_reply not defined in config file");
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
            var sql = "select * from alarm_definition order by siteid";
            m_server.FillTable(alarm_definition, sql);
            alarm_definition.idColumn.AutoIncrementSeed = m_server.NextID("alarm_definition", "id");
            return alarm_definition;
        }

        // cachine alarm def (41 records/s )
        //static alarm_definitionDataTable s_alarmdef;


        /// <summary>
        /// Returns a Table of enabled alarm definitions
        /// </summary>
        /// <returns></returns>
        public AlarmDataSet.alarm_definitionDataTable GetActiveAlarmDefinition(string siteid, string parameter)
        {
            var alarm_definition = new AlarmDataSet.alarm_definitionDataTable();

            siteid = BasicDBServer.SafeSqlLikeClauseLiteral(siteid);
            parameter = BasicDBServer.SafeSqlLikeClauseLiteral(parameter);
            var sql = "select * from alarm_definition where siteid='" + siteid + "' and parameter ='" + parameter + "'"
            +" and  enabled = " + m_server.PortableWhereBool(true);
            Logger.WriteLine(sql);
           m_server.FillTable(alarm_definition, sql);

            return alarm_definition;
        }


        /// <summary>
        /// determines if there is any asterisk activity
        /// within in a specified number of minutes.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public bool CurrentActivity(int id, int minutes)
        { // look in alarm_log table and check for recent activity
            var alarm_log = new AlarmDataSet.alarm_logDataTable();

            DateTime t = DateTime.Now.AddMinutes(-minutes);
            var sql = "select * from alarm_log where datetime >= "
                 + m_server.PortableDateString(t, TimeSeriesDatabase.dateTimeFormat)
                 + " and alarm_phone_queue_id = " + id; 

            m_server.FillTable(alarm_log, sql);

            return alarm_log.Rows.Count > 0;
        }


        public AlarmDataSet.alarm_logDataTable GetLog(int minutes)
        {
            var alarm_log = new AlarmDataSet.alarm_logDataTable();

            DateTime t = DateTime.Now.AddMinutes(-minutes);
            var sql = "select * from alarm_log where datetime >= "
                 + m_server.PortableDateString(t, TimeSeriesDatabase.dateTimeFormat);

            m_server.FillTable(alarm_log, sql);

            return alarm_log;
        }

        internal int NextID(string tableName, string columnName)
        {
            return m_server.NextID(tableName, columnName);
        }
    }
}



