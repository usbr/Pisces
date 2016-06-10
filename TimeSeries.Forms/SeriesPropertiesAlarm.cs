using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Forms
{
    public partial class SeriesPropertiesAlarm : UserControl
    {
        public SeriesPropertiesAlarm()
        {
            InitializeComponent();
        }

        private DataTable m_table;

        public DataTable Table
        {
            get { return m_table; }
            set {
                
                m_table = value;
                var groups = DataTableUtility.SelectDistinct(m_table, "alarm_group");
                this.comboBoxGroup.DataSource = groups;
                this.comboBoxGroup.DisplayMember = "alarm_group";
                this.comboBoxGroup.ValueMember = "alarm_group";

                this.dataGridView1.DataSource = m_table;
                m_table.DefaultView.RowFilter = "alarm_group = '" + comboBoxGroup.SelectedValue + "'";
            }
        }

        private void buttonReadTestDAta_Click(object sender, EventArgs e)
        {
            //var fn = @"C:\Users\rherrera\Documents\workspace\Pisces\Asterisk\ExtensionBuilder\src\callout_list.csv";
            //CsvFile csv = new CsvFile(fn);

            //for (int i = 0; i < csv.Rows.Count; i++)
            //{
            //    m_table.Rows.Add(csv.Rows[i].ItemArray);
            //}
        }



    }
}
