using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Reclamation.Core;
using Reclamation.TimeSeries;

namespace Rwis.Sync
{
    public partial class rwisForm : Form
    {
        // Connect to DB Server
        private static string dbname = ConfigurationManager.AppSettings["MySqlDatabase"];
        private static string server = ConfigurationManager.AppSettings["MySqlServer"];
        private static string user = ConfigurationManager.AppSettings["MySqlUser"];
        private static BasicDBServer svr = MySqlServer.GetMySqlServer(server, dbname, user);
        private static TimeSeriesDatabase db = new TimeSeriesDatabase(svr);
        private static DataTable siteCat = db.GetSiteCatalog();
        private static DataTable parCat = db.GetParameterCatalog();


        public static void Main(string[] args)
        {

            // Starts the application.
            Application.Run(new rwisForm());
        }

        public rwisForm()
        {
            InitializeComponent();
        }


        public string GetRegion()
        {
            if (this.pnRadioButton.Checked)
            { return "PN"; }
            else if (this.gpRadioButton.Checked)
            { return "GP"; }
            else if (this.lcRadioButton.Checked)
            { return "LC"; }
            else if (this.ucRadioButton.Checked)
            { return "UC"; }
            else if (this.mpRadioButton.Checked)
            { return "MP"; }
            else
            { return "PN"; }
        }

        public void getParTypes(object sender, EventArgs e)
        {
            DataTable distinctPars = parCat.DefaultView.ToTable(true, "name");
            var outVals = new List<string>(); ;
            foreach (DataRow row in distinctPars.Rows)
            { this.parameterTypeComboBox.Items.Add(row[0].ToString()); }
        }

        public void getSiteTypes(object sender, EventArgs e)
        {
            DataTable distinctSites = siteCat.DefaultView.ToTable(true, "type");
            var outVals = new List<string>(); ;
            foreach (DataRow row in distinctSites.Rows)
            { this.siteTypeComboBox.Items.Add(row[0].ToString()); }
        }

        private void getSitesByType(object sender, EventArgs e)
        {
            string typeVal;
            try
            { typeVal = this.siteTypeComboBox.SelectedItem.ToString(); }
            catch
            { typeVal = ""; }
            var region = GetRegion();
            this.siteComboBox.Items.Clear();
            if (typeVal != "")
            {
                DataRow[] distinctSites = siteCat.Select("type='" + typeVal + "' AND agency_region='" + region + "'");
                foreach (DataRow row in distinctSites)
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = row["description"].ToString();
                    item.Value = row["siteid"].ToString();
                    this.siteComboBox.Items.Add(item);
                }
            }
            else
            { this.siteComboBox.Items.Add("SELECT A SITE TYPE..."); }
        }
        
        private void clearSiteComboBox(object sender, EventArgs e)
        {
            this.siteComboBox.SelectedIndex = -1;
        }

        private void getParametersByTypeTimeStep(object sender, EventArgs e)
        {
            string typeVal, tStep;
            try
            {
                typeVal = this.parameterTypeComboBox.SelectedItem.ToString();
                tStep = this.tstepComboBox.SelectedItem.ToString();
            }
            catch
            {
                typeVal = "";
                tStep = "";
            }
            this.parameterComboBox.Items.Clear();
            if (typeVal != "" && tStep != "")
            {
                DataRow[] distinctSites = parCat.Select("name='" + typeVal + "' AND timeinterval='" + tStep + "'");
                foreach (DataRow row in distinctSites)
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = row["id"].ToString();
                    item.Value = row["id"].ToString();
                    this.parameterComboBox.Items.Add(item);
                }
            }
            else
            { this.parameterComboBox.Items.Add("SELECT A TIME-STEP AND PARAMETER TYPE..."); }
        }

        private void clearParameterComboBox(object sender, EventArgs e)
        {
            this.parameterComboBox.SelectedIndex = -1;
        }

        /// <summary>
        /// ComboBoxItem class to add text-value pairs to ComboBoxes
        /// http://stackoverflow.com/questions/3063320/combobox-adding-text-and-value-to-an-item-no-binding-source
        /// </summary>
        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        /// <summary>
        /// Sets text for the Site Info labels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetSiteInfo(object sender, EventArgs e)
        {
            string siteCode;
            try
            { siteCode = (this.siteComboBox.SelectedItem as ComboboxItem).Value.ToString(); }
            catch
            { siteCode = ""; }
            if (siteCode != "")
            {
                DataRow[] siteInfo = siteCat.Select("siteid='" + siteCode + "'");
                var info = siteInfo[0];
                string info1 = "", info2 = "";
                info1 = "Site Info: Region: " + info["agency_region"] +
                    ", Site ID: " + info["siteid"] +
                    ", Time Zone: " + info["timezone"];
                info2 += "State: " + info["state"] +
                    ", Lat: " + info["latitude"] +
                    ", Lon: " + info["longitude"] +
                    ", Elev: " + info["elevation"];
                this.siteInfoLabel1.Text = info1;
                this.siteInfoLabel2.Text = info2;
            }
            else
            {
                this.siteInfoLabel1.Text = "Site Info : N/A";
                this.siteInfoLabel2.Text = "N/A";
            }
            GetPiscesName();
        }

        private void GetParameterInfo(object sender, EventArgs e)
        {
            string parCode;
            try
            { parCode = (this.parameterComboBox.SelectedItem as ComboboxItem).Value.ToString(); }
            catch
            { parCode = ""; }
            if (parCode != "")
            {
                DataRow[] siteInfo = parCat.Select("id='" + parCode + "'");
                var info = siteInfo[0];
                string info1 = "", info2 = "";
                info1 = "Parameter Info: Parameter ID: " + info["id"];
                info2 += "Time Interval: " + info["timeinterval"] +
                    ", Physical Units: " + info["units"] +
                    ", Data Statistic: " + info["statistic"];
                this.parameterInfoLabel1.Text = info1;
                this.parameterInfoLabel2.Text = info2;
            }
            else
            {
                this.parameterInfoLabel1.Text = "Parameter Info : N/A";
                this.parameterInfoLabel2.Text = "N/A";
            }
            GetPiscesName();
        }

        private void GetPiscesName()
        {
            string siteId, parId;
            try
            {
                siteId = (this.siteComboBox.SelectedItem as ComboboxItem).Value.ToString();
                parId = (this.parameterComboBox.SelectedItem as ComboboxItem).Value.ToString();
                this.rwisNameLabel.Text = "RWIS Name: " + siteId + "_" + parId;
            }
            catch
            {
                this.rwisNameLabel.Text  = "RWIS Name: N/A";
            }



        }

    }
}
