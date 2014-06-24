using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries.Nrcs;
using DgvFilterPopup;

namespace Reclamation.TimeSeries.Forms.ImportForms
{
    public partial class ImportNrcsSnotel : Form
    {
        DgvFilterManager filterManager = new DgvFilterManager();

        public ImportNrcsSnotel()
        {
            InitializeComponent();

            filterManager = new DgvFilterManager();

            var tbl = NrcsSnotelSeries.SnotelSites;
            dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = tbl;
            this.dataGridView1.SelectionChanged += new EventHandler(dataGridView1_SelectionChanged);
            dataGridView1.DataError += dataGridView1_DataError;
            filterManager.DataGridView = dataGridView1;
            buttonOk.Enabled = false;
        }

        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            
        }

        void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            buttonOk.Enabled = true;
        }

        public string[] SelectedParameters
        {

            get
            {
                return checkedListBox1.CheckedItems.OfType<string>().ToArray();
            }
        }


        public string[] SelectedSiteNumbers
        {
            get
            {
                var rval = new List<string>();
                    for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                    {
                        DataRowView drv = (DataRowView)dataGridView1.SelectedRows[i].DataBoundItem;
                        rval.Add(drv["SiteID"].ToString());
                    }
                    return rval.ToArray();
            }
        }
    }
}
