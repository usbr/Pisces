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
        DataTable tbl;
        public AlarmQueue(AlarmDataSet ds)
        {
            InitializeComponent();
            this.m_ds = ds;
            Init();
        }

        private void Init()
        {
             tbl = m_ds.GetAlarmQueue(this.checkBoxAllAlarms.Checked);
             this.dataGridView1.DataSource = tbl;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            // readonly except the active column 
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                var c = dataGridView1.Columns[i];
                c.ReadOnly = c.Name != "active";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Init();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            m_ds.SaveTable(tbl);
        }


    }
}
