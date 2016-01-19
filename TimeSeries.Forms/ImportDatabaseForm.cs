using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class ImportDatabaseForm : Form
    {
        public ImportDatabaseForm()
        {
            InitializeComponent();
        }

        //public string DatabaseFilename
        //{
        //    get
        //    {
        //        return textBoxsdf.Text;
        //    }        
        //}

        public string CatalogFilename
        {
            get
            {
                return textBoxcatalog.Text;
            }    
        }

        public bool IncludeSeriesData
        {
            get
            {
                return checkBoxGetSeriesData.Checked;
            }    
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            //saveFileDialog1.DefaultExt = ".pdb";
            //saveFileDialog1.Filter = "Pisces Database Files (*.pdb)|*.pdb";
            //saveFileDialog1.AddExtension = true;
            //saveFileDialog1.RestoreDirectory = true;
            //saveFileDialog1.Title = "Create a new Database";
            
            //if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    this.textBoxsdf.Text = saveFileDialog1.FileName;                
            //}
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = ".csv";
            openFileDialog1.Filter = "Comma Seperated Files (*.csv)|*.csv";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Title = "Open";
            openFileDialog1.FileName = null;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBoxcatalog.Text = openFileDialog1.FileName;
            }
        }

        private void Ok_Click(object sender, EventArgs e)
        {

        }

    }
}
