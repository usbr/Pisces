using System;
using System.IO;
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

        public DataTable GetShefTable()
        {
            return shefDataTable;
        }

        public string GetShefLocation()
        {
            return this.stationsComboBox.SelectedItem.ToString();
        }

        public string GetShefCode()
        {
            return this.pecodesComboBox.SelectedItem.ToString();
        }

        public string GetShefFileName()
        {
            return this.shefFileSelected.Text.ToString();
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
                ReadShefFile(choofdlog.FileName);
                GetShefLocations();
            }
        }

        private void ReadShefFile(string fileName)
        {
            var s = new SHEF.ShefSeries();
            shefDataTable = s.ReadShefFile(fileName);
        }

        private void GetShefLocations()
        {
            DataView view = new DataView(shefDataTable);
            DataTable distinctLocations = view.ToTable(true, "location");
            foreach (DataRow item in distinctLocations.Rows)
            {
                this.stationsComboBox.Items.Add(item["location"].ToString());
            }
            this.stationsComboBox.SelectedItem = this.stationsComboBox.Items[0];
        }

        private void GetShefCodeForLocation(object sender, EventArgs e)
        {
            string location = this.stationsComboBox.SelectedItem.ToString();
            DataView view = new DataView(shefDataTable);
            DataTable distinctPairs = view.ToTable(true, "location", "shefcode");
            var distinctCodes = new DataView(distinctPairs);
            distinctCodes.RowFilter = "location = '" + location + "'";
            var codeTable = distinctCodes.ToTable();
            this.pecodesComboBox.Items.Clear();
            foreach (DataRow item in codeTable.Rows)
            {
                this.pecodesComboBox.Items.Add(item["shefcode"].ToString());
            }
            this.pecodesComboBox.SelectedItem = this.pecodesComboBox.Items[0];
        }


    }
}
