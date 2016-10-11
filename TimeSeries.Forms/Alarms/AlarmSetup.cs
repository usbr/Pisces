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
    public partial class AlarmSetup : UserControl
    {
        AlarmDataSet m_ds;
        public AlarmSetup()
        {
            InitializeComponent();
        }

        public AlarmSetup(AlarmDataSet ds)
        {
            InitializeComponent();
            this.m_ds = ds;
        }

        private void AlarmSetup_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void Init(string label = "")
        {
            dgvList.DataSource = m_ds.GetList();

            if (label != "")
            {
                for (int i = 0; i < dgvList.Rows.Count; i++)
                {
                    if (dgvList.Rows[i].Cells[0].Value.ToString() == label)
                    {
                        dgvList.CurrentCell = dgvList.Rows[i].Cells[0];
                        break;
                    }
                }
               
            }
            
            RefreshRecipients();
        }


        AlarmDataSet.alarm_recipientDataTable alarm_recipient = new AlarmDataSet.alarm_recipientDataTable();
        private void RefreshRecipients()
        {
            string label = CurrentLabel();
            if (label != "")
            {
                alarm_recipient = m_ds.GetRecipients(label);
                dataGridViewRecipient.DataSource = alarm_recipient;

                dataGridViewRecipient.Columns["id"].Visible = false;
                dataGridViewRecipient.Columns["list"].Visible = false;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var dlg = new FormNewAlarmGroup();

            if( dlg.ShowDialog() == DialogResult.OK && dlg.AlarmGroupText.Trim() != "")
            {
                m_ds.AddNewAlarmGroup(dlg.AlarmGroupText);
                Init(dlg.AlarmGroupText);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
          string label = CurrentLabel();

          if (label != "")
          {
              m_ds.DeleteAlarmGroup(label);
              Init();
          }
           
        }

        private string CurrentLabel()
        {
            string rval = "";
            if (dgvList.SelectedCells.Count == 1)
            {
                rval = dgvList.SelectedCells[0].Value.ToString();
            }
            return rval;
        }

        private void buttonSaveRecipients_Click(object sender, EventArgs e)
        {
            m_ds.SaveTable(alarm_recipient);
            Init(CurrentLabel());   
        }

        private void dataGridViewList_MouseClick(object sender, MouseEventArgs e)
        {
            RefreshRecipients();
        }



    }
}
