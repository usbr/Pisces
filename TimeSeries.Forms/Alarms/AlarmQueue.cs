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
    public partial class AlarmQueue : UserControl
    {
        public AlarmQueue()
        {
            InitializeComponent();
        }

        AlarmDataSet m_ds;
        public AlarmQueue(AlarmDataSet ds)
        {
            InitializeComponent();
            this.m_ds = ds;
            Init();
        }

        private void Init()
        {
            this.dataGridView1.DataSource = m_ds.GetAlarmQueue(this.checkBoxAllAlarms.Checked);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Init();
        }


    }
}
