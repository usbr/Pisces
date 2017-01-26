using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries.Alarms;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Forms.Alarms
{
    public partial class AlarmDefinition : UserControl
    {
        AlarmDataSet m_ds;
        AlarmDataSet.alarm_definitionDataTable alarm_definition;

        public AlarmDefinition()
        {
            InitializeComponent();
        }
        public AlarmDefinition(AlarmDataSet ds)
        {
            InitializeComponent();
            m_ds = ds;
            //RefreshDefinition();
        }

        
        private void RefreshDefinition() 
        {
            alarm_definition = m_ds.GetAlarmDefinition();

            dataGridView1.DataSource = alarm_definition;
           dataGridView1.Columns["id"].Visible = false;
        }


        private void AlarmDefinition_Load(object sender, EventArgs e)
        {
            RefreshDefinition();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {

            m_ds.SaveTable(alarm_definition);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            RefreshUi();
        }
        private void RefreshUi()
        {
            Enabling();
            this.comboBoxTestList.DataSource= m_ds.GetList();
            comboBoxTestList.DisplayMember = "list"; 

        }

        private void Enabling()
        {
            var sr = dataGridView1.SelectedRows;

            this.buttonTest.Enabled = sr.Count == 1;
            this.textBoxTest.Enabled = sr.Count == 1;
            this.comboBoxTestList.Enabled = sr.Count == 1;
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
           
         DataRowView currentDataRowView = (DataRowView)dataGridView1.CurrentRow.DataBoundItem;
         AlarmDataSet.alarm_definitionRow alarm
             = (AlarmDataSet.alarm_definitionRow)currentDataRowView.Row;

         var numbers = m_ds.GetPhoneNumbers(comboBoxTestList.Text);

              var login = new Login();
              if (login.ShowDialog() == System.Windows.Forms.DialogResult.OK)
              {

                  //Asterisk.Call(alarm.siteid, alarm.parameter,
                    //   textBoxTest.Text, numbers,login.Username,login.Password);
              }

        }
    }
}
