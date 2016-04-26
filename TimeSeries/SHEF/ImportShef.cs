using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.SHEF
{
    public partial class ImportShef : Form
    {
        DataTable shefDataTable = new DataTable();

        public ImportShef()
        {
            InitializeComponent();
        }

        private void shefSelectButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog choofdlog = new OpenFileDialog();
            choofdlog.Multiselect = false;

            DialogResult result = choofdlog.ShowDialog();
            if (result == DialogResult.OK) 
            {
                this.shefFileSelected.Text = choofdlog.FileName;
                var shef = new ShefSeries();
                shef.ReadShefFile(choofdlog.FileName);
                GetShefLocations();
            }
        }

        public void GetShefLocations()
        {
            DataView view = new DataView(shefDataTable);
            DataTable distinctLocations = view.ToTable(true, "location");
            foreach (DataRow item in distinctLocations.Rows)
            {
                this.stationsListBox.Items.Add(item["location"].ToString());
            }
        }

        public void GetShefCodeForLocation(object sender, EventArgs e)
        {
            string location = this.stationsListBox.SelectedItem.ToString();
            DataView view = new DataView(shefDataTable);
            DataTable distinctPairs = view.ToTable(true, "location", "shefcode");
            var distinctCodes = new DataView(distinctPairs);
            distinctCodes.RowFilter = "location = '" + location + "'";
            var codeTable = distinctCodes.ToTable();
            foreach (DataRow item in codeTable.Rows)
            {
                this.peCodesListBox.Items.Add(item["shefcode"].ToString());
            }
        }


    }
}
