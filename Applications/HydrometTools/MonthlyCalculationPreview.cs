using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;

namespace HydrometTools
{
    public partial class MonthlyCalculationPreview : Form
    {
        public MonthlyCalculationPreview()
        {
            InitializeComponent();
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(HydrometMonthlySeries.Flags);
            comboBox1.SelectedItem = "V - Loaded directly from ARCHIVES";

        }

        public DataTable DataSource {
        set {
             
            this.dataGridView1.DataSource = value;
            if (dataGridView1.ColumnCount >= 4)
            {
                dataGridView1.Columns[0].DefaultCellStyle.Format = "MMM yyyy";
                dataGridView1.Columns[1].DefaultCellStyle.Format = "F3";
                dataGridView1.Columns[2].DefaultCellStyle.Format = "F3";
                dataGridView1.Columns[3].DefaultCellStyle.Format = "F3";
            }
         }

    }

        public string SelectedFlag
        {
            get
            {
                return this.comboBox1.SelectedItem.ToString().Substring(0, 1);
            }
        }


    }
}
