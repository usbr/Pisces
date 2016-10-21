using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries.Alarms;

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
    }
}
