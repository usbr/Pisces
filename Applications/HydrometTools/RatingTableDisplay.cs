using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Usgs;
using Reclamation.Core;
using System.IO;
using Reclamation.TimeSeries.Graphing;
namespace HydrometTools
{
    public partial class RatingTableDisplay : UserControl
    {
        public RatingTableDisplay()
        {
            InitializeComponent();
        }

        string cbtt = "";
        string yparm = "";
        //string ratingName = ""; //name in url to table
        TimeSeriesDatabaseDataSet.RatingTableDataTable hydrometRatingTable;
        TimeSeriesDatabaseDataSet.RatingTableDataTable usgsRatingTable;
        private void buttonSelect_Click(object sender, EventArgs e)
        {

            Cursor = Cursors.WaitCursor;
            try
            {
                SelectTable();

            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void SelectTable()
        {
            hydrometRatingTable = new TimeSeriesDatabaseDataSet.RatingTableDataTable();
            var f = new RatingTableSelector();
            if (f.ShowDialog() == DialogResult.OK)
            {

                cbtt = f.cbtt;
                yparm = f.pcode;

                this.labelSiteName.Text = "Description: " + HydrometInfoUtility.LookupSiteDescription(cbtt);
                this.labelcbtt.Text = "cbtt: " + cbtt;
                string altid = HydrometInfoUtility.LookupAltID(cbtt).Trim();
                if (altid.Length > 0)
                    labelcbtt.Text += " altid: " + altid;
                this.labelyparm.Text = "y parameter: " + yparm;

                ReadTableFromInternet(cbtt, yparm, f.RatingName, altid);

                hydrometRatingTable.Name = f.RatingName + " -- " + HydrometInfoUtility.LookupSiteDescription(cbtt);
                hydrometRatingTable.EditDate = f.DateModified;

                this.labelDate.Text = "Modified " + f.DateModified;

            }
        }

        private void ReadTableFromInternet(string cbtt, string yparm,string ratingName, string altid)
        {
           hydrometRatingTable = HydrometInfoUtility.GetRatingTable(cbtt, yparm, ratingName);
           usgsRatingTable = new TimeSeriesDatabaseDataSet.RatingTableDataTable();

            if (altid.Trim() != "")
            {
                try
                {
                    usgsRatingTable = Reclamation.TimeSeries.Usgs.Utility.GetRatingTable(altid);
                }
                catch (Exception eek)
                {
                    MessageBox.Show(eek.Message);
                }
            }

            this.ratingTableGraph1.RatingTable = new TimeSeriesDatabaseDataSet.RatingTableDataTable[] { hydrometRatingTable, usgsRatingTable };

            this.ratingTableTableHydromet.RatingTable = hydrometRatingTable;
            this.ratingTableTableUsgs.RatingTable = usgsRatingTable;
            
        }

        private void linkLabelGraphEditor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ratingTableGraph1.ShowEditor();
        }

       
    }
}
