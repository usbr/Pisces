using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;
using Reclamation.TimeSeries.Alarms;

namespace Reclamation.TimeSeries.Forms.Alarms
{
    public partial class SoundFiles : UserControl
    {
        AlarmDataSet.alarm_scriptsDataTable tbl = new AlarmDataSet.alarm_scriptsDataTable();

        public SoundFiles()
        {
            InitializeComponent();
        }

        BasicDBServer m_svr;
        public SoundFiles(BasicDBServer svr)
        {
            m_svr = svr;
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            if (!m_svr.TableExists(tbl.TableName))
                return;
           m_svr.FillTable(tbl);
           tbl.Columns[0].AutoIncrementSeed = tbl.NextID();
           tbl.Columns[0].AutoIncrementStep = 1;
           dataGridView1.DataSource = tbl;
           dataGridView1.Columns[0].Visible = false;

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            m_svr.SaveTable(tbl);
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {

        }
    }
}
