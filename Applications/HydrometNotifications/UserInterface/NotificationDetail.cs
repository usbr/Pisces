using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HydrometNotifications.UserInterface
{
    public partial class NotificationDetail : UserControl
    {
        public NotificationDetail()
        {
            InitializeComponent();
        }

        DataTable alarm_definition;
        DataTable alarm_sites;

        string m_group = "";
        internal void SetGroup(string group)
        {
            m_group = group;
            this.textBoxName.Text = m_group;
            Read();
            // email list
            var emailList = AlarmDataSet.EmailAddresses(group);
            this.textBoxEmailList.Text = String.Join(",", emailList);
            //  reports

            //  notifications
            dataGridViewNotifications.DataSource = alarm_definition;
            dataGridViewNotifications.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //  site lists  (shared for all groups)
            dataGridViewSiteList.DataSource = alarm_sites;
           dataGridViewSiteList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            
        }

        private void Read()
        {
            alarm_definition = AlarmDataSet.GetAlarmDefinitionTable(m_group);
            alarm_definition.Columns[0].AutoIncrementSeed = AlarmDataSet.GetNextID("alarm_definition");
            alarm_sites = AlarmDataSet.alarm_sites();
        }

        private void Save()
        {
            this.dataGridViewNotifications.Select();
            this.dataGridViewSiteList.Select();

            int a1 = AlarmDataSet.DB.SaveTable(alarm_definition);
           int a2 = AlarmDataSet.DB.SaveTable(alarm_sites);
           int a3 = AlarmDataSet.SaveEmailAddresses(m_group, this.textBoxEmailList.Text);

           int total = a1 + a2 + a3;
           labelStatus.Text ="saved " + total + " rows of data";

           Read();
           
        }

        private void Delete()
        {
            if (MessageBox.Show("Delete all data for '"+m_group+"' ?","Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                // delete from email list
                AlarmDataSet.DB.RunSqlCommand("delete from alarm_group where group_name = '" + m_group + "'");
                

                // delete from alarm_definition
                AlarmDataSet.DB.RunSqlCommand("delete from alarm_definition where group_name = '" + m_group + "'");
                linkLabelBack_LinkClicked(this,new LinkLabelLinkClickedEventArgs( linkLabelBack.Links[0]));
            }
        }

        public event EventHandler<EventArgs> OnClose;

        private void linkLabelBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            if (OnClose != null)
                OnClose(this, EventArgs.Empty);

            this.Visible = false;
            this.Parent = null;

        }

        private void linkLabelSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Save();
        }

        private void linkLabelDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Delete();
        }

       
    }
}
