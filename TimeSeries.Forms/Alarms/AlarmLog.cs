using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Reclamation.TimeSeries.Alarms;
using DgvFilterPopup;

namespace Reclamation.TimeSeries.Forms.Alarms
{
    public partial class AlarmLog : UserControl
    {
        public AlarmLog()
        {
            InitializeComponent();
        }


         AlarmDataSet m_ds;
         public AlarmLog(AlarmDataSet ds)
        {
            InitializeComponent();
            this.m_ds = ds;
            RefreshLog();
        }

         DgvFilterManager filterManager = new DgvFilterManager();
         private void RefreshLog()
         {
             filterManager = null;
             filterManager = new DgvFilterManager();
             int minutes = Convert.ToInt32(this.textBoxDaysBack.Text) * 1440;
             this.dataGridView1.DataSource = m_ds.GetLog(minutes);

             filterManager.DataGridView = dataGridView1;
         }

         private void button1_Click(object sender, EventArgs e)
         {
             RefreshLog();
         }
    }
}
