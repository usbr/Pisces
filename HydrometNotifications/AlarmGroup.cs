using System;
using System.Net.Mail;
using Reclamation.Core;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace HydrometNotifications
{
    
    /// <summary>
    /// Manages a group of alarms 
    /// </summary>
    class AlarmGroup
    {

        string group_name;
        bool useHydrometCache;
        bool sendEmail;
        int alarmCount;

        public int AlarmCount
        {
            get { return alarmCount; }
        }
        int clearCount;

        public int ClearCount
        {
            get { return clearCount; }
        }

        public int ProcessCount
        {
            get { return alarmCount+clearCount; }
        }


        /// <summary>
        /// daily 'd' or instant 'i'
        /// </summary>
        public string Interval
        {
            get
            {
                var alarmdef = AlarmDataSet.GetActiveAlarmDefinitionTable(group_name);
                if (alarmdef.Rows.Count == 0)
                    return "";

                string rval = alarmdef[0].database;
                foreach (var row in alarmdef)
                {
                    if (row.database != rval)
                    {
                        Console.WriteLine("cbtt="+row.cbtt);
                        throw new Exception("Error: All the data must be from the same database in the group_name:" + group_name);
                    }
                }

                return rval;

            }
        }


                /// <summary>
        /// checks alarms within a group, sending emails when required.
        /// </summary>
        public AlarmGroup(string groupName, bool useHydrometCache=true, bool sendEmail=true)
        {
            group_name = groupName;
            this.useHydrometCache = useHydrometCache;
            this.sendEmail = sendEmail;
        }

        public void UpdateRows(string columnName, object value)
        {
            var alarmdef = AlarmDataSet.GetActiveAlarmDefinitionTable(group_name);
            for (int i = 0; i < alarmdef.Count; i++)
            {
                if( !alarmdef[i].IsVirtual)
                  alarmdef[i][columnName] = value;
            }
            alarmdef.Save();
        }


        internal void ProcessGroup(DateTime t)
        {
            alarmCount = 0;
            clearCount = 0;
            var alarmdef = AlarmDataSet.GetActiveAlarmDefinitionTable(group_name);

            if (useHydrometCache)
            {
                AlarmDataSet.PreloadInstantHydrometData(alarmdef);
            }
            string emailMsg = "";
            string txtMsg = "";
            string emailMessageBody = "";
            string txtMessageBody = "";
            foreach (var row in alarmdef)
            {
                if (row.enabled)
                {
                    if (!row.active)
                    {
                        Alarm alarmCondition = AlarmFactory(row, "alarm_condition");
                        if (alarmCondition.Check(t))
                        {  // set the alarm.
                            if (!row.IsVirtual) // don't set active on 'virtual' alarms
                                row.active = true;
                            alarmCount++;
                             alarmCondition.CreateMessage(out emailMsg,out txtMsg);
                             txtMessageBody += " " +txtMsg;
                            emailMessageBody += "<br/>Hydromet Notification: \n<br>" + emailMsg;
                        }
                    }
                    else
                    {  // check if alarm can be cleared.
                        if (row.clear_condition != "") // without clear condition you must manually clear that alarm
                        {
                            Alarm clearCondition = AlarmFactory(row, "clear_condition");
                            if (clearCondition.Check(t))
                            { // clear the alarm.
                                if (row.IsVirtual)
                                    throw new ApplicationException("Assertion.... unexpected virtual rows");
                                row.active = false; 
                                
                                emailMessageBody += " The Alarm condition below has been CLEARED<br/> ";
                                clearCondition.CreateMessage(out emailMsg, out txtMsg);
                                emailMessageBody += "<br/>Hydromet Notification: \n<br>" + emailMsg;
                                txtMessageBody += txtMsg;
                                clearCount++;
                            }
                        }
                    }
                }
            }

            if (ProcessCount > 0 && sendEmail)
            {

                emailMessageBody += "\n\n <a href=\"https://www.usbr.gov/pn/hydromet/disclaimer.html\">provisional data disclaimer</a>";
                SendEmailNotice(AlarmDataSet.EmailAddresses(group_name),  group_name, emailMessageBody,txtMessageBody);
            }
            // phone ??
            alarmdef.Save();
        }





        private static void SendEmailNotice(string[] email_list, string subject, string emailMsg,string txtMsg)
        {
            var emailRecipients = new List<string>();
            var txtRecipients = new List<string>();
            foreach (var emailAddress in email_list)
            {
                var address = emailAddress.Trim();
                if (Regex.IsMatch(emailAddress, @"^(\d{10}@)")) // cell phone---> use text message
                {
                    txtRecipients.Add(address);
                }
                else// use html formatted email message
                {
                    emailRecipients.Add(address);
                }
            }

            SendEmail(txtRecipients.ToArray(), subject, txtMsg);
            SendEmail(emailRecipients.ToArray(), subject, emailMsg);
        }

        private static void SendEmail(string[] address, string subject, string body)
        {
            if (address.Length == 0)
                return;
            MailMessage msg = new MailMessage();

            foreach (var item in address)
            {
                msg.To.Add(item);   
            }
           
            msg.From = new MailAddress(ConfigurationManager.AppSettings["email_reply"]);
            //msg.From = new MailAddress(WindowsUtility.GetShortUserName() + "@" + WindowsUtility.GetMachineName());
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;
            SmtpClient c = new System.Net.Mail.SmtpClient();
            c.Host = ConfigurationManager.AppSettings["smtp"];
            c.Send(msg);

            
            Logger.WriteLine("mail server " + c.Host);
            Logger.WriteLine("to : "+ address);
            Logger.WriteLine("from : " + msg.From.Address);
            Logger.WriteLine("message sent ");
            Logger.WriteLine(body);
        }
        private static Alarm AlarmFactory(AlarmDataSet.alarm_definitionRow row, string conditionColumnName)
        {
            string condition =row[conditionColumnName].ToString(); 

            if (Regex.IsMatch(condition, CountAlarm.Expression))
            {
                return new CountAlarm(row, conditionColumnName);
            }
            else if (Regex.IsMatch(condition, LimitAlarm.Expression))
            {
                return new LimitAlarm(row, conditionColumnName);
            }
            else if (Regex.IsMatch(condition, FlagAlarm.Expression))
            {
                return new FlagAlarm(row, conditionColumnName);
            }
            else if (Regex.IsMatch(condition, RogueMinimumFlow.Expression))
            {
                return new RogueMinimumFlow(row, conditionColumnName.ToLower()=="clear_condition");
            }
            else if (Regex.IsMatch(condition, RogueSystemState.Expression))
            {
                return new RogueSystemState(row);
            }

            throw new NotImplementedException("Invalid alarm condition " + row[conditionColumnName]);
        }

    }
}
