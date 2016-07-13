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

        /// <summary>
        /// Main entry form
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // Starts the application.
            Application.Run(new rwisForm());
        }

        /// <summary>
        /// Generate form
        /// </summary>
        public rwisForm()
        {
            InitializeComponent();            
        }

        /// <summary>
        /// Gets Region option selected for use by other options
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Controls visibility of Required Regional Connection Information based on Region selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setRegion(object sender, EventArgs e)
        {
            var region = GetRegion();
            if (region == "LC" || region == "UC")
            {
                this.siteCodeTextbox.Enabled = false;
                this.parameterCodeTextBox.Enabled = false;
                this.sdiTextBox.Enabled = true;
            }
            else
            {
                this.siteCodeTextbox.Enabled = true;
                this.parameterCodeTextBox.Enabled = true;
                this.sdiTextBox.Enabled = false;
            }
            GetDataProvider();
        }

        /// <summary>
        ///  Gets Parameter Type options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void getParTypes(object sender, EventArgs e)
        {
            DataTable distinctPars = parCat.DefaultView.ToTable(true, "name");
            var outVals = new List<string>(); ;
            foreach (DataRow row in distinctPars.Rows)
            { this.parameterTypeComboBox.Items.Add(row[0].ToString()); }
        }

        /// <summary>
        /// Gets Site Type options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void getSiteTypes(object sender, EventArgs e)
        {
            DataTable distinctSites = siteCat.DefaultView.ToTable(true, "type");
            var outVals = new List<string>(); ;
            foreach (DataRow row in distinctSites.Rows)
            { this.siteTypeComboBox.Items.Add(row[0].ToString()); }
        }

        /// <summary>
        /// Gets Sites given option selections
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                if (distinctSites.Count() < 1)
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = "No sites found in " + region + " for SiteType=" + typeVal + "...";
                    item.Value = "";
                    this.siteComboBox.Items.Add(item);
                }
                else
                {
                    foreach (DataRow row in distinctSites)
                    {
                        ComboboxItem item = new ComboboxItem();
                        item.Text = row["description"].ToString();
                        item.Value = row["siteid"].ToString();
                        this.siteComboBox.Items.Add(item);
                    }
                }
            }
            else
            { this.siteComboBox.Items.Add("SELECT A SITE TYPE..."); }
        }

        /// <summary>
        /// Clear contents for labels on option changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearSiteComboBox(object sender, EventArgs e)
        {
            this.siteComboBox.SelectedIndex = -1;
        }

        /// <summary>
        /// Gets Parameters given option selections
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Clear contents for labels on option changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearParameterComboBox(object sender, EventArgs e)
        {
            this.parameterComboBox.SelectedIndex = -1;
            GetDataProvider();
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

        /// <summary>
        /// Sets text for Parameter Info labels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Builds the Data Provider string
        /// </summary>
        private void GetDataProvider()
        {
            var region = GetRegion();
            string intervalCode = intervalCode = this.tstepComboBox.SelectedItem.ToString();
            switch (intervalCode)
            {
                case "Day":
                    intervalCode = "Daily";
                    break;
                case "Month":
                    intervalCode = "Monthly";
                    break;
                default:
                    intervalCode = "Daily";
                    break;
            }
            string provider = "";
            switch (region)
            {
                case "PN": case "GP": 
                    provider += "Hydromet" + intervalCode + "Series";
                    break;
                case "LC": case "UC":
                    provider += "HDBSeries";
                    break;
                case "MP":
                    provider += "SFTPSeries";
                    break;
            }
            this.dataProviderLabel.Text = "Data Provider: " + provider;
        }

        /// <summary>
        /// Resolves the table and series name for storage into RWIS
        /// </summary>
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

        /// <summary>
        ///  Build Connection String to be stored in RWIS
        /// </summary>
        private void BuildConnectionString(object sender, EventArgs e)
        {
            string conx = "";
            var region = GetRegion();
            var siteCode = this.siteCodeTextbox.Text.ToString();
            var parCode = this.parameterCodeTextBox.Text.ToString();
            var sdiCode = this.sdiTextBox.Text.ToString();
            var intervalCode = this.tstepComboBox.SelectedItem.ToString();

            switch (intervalCode)
            {
                case "Day":
                    intervalCode = "Daily";
                    break;
                case "Month":
                    intervalCode = "Monthly";
                    break;
                default:
                    intervalCode = "Daily";
                    break;                
            }
            switch (region)
            {
                case "PN":
                    conx += "server=PN;cbtt=" + siteCode + ";pcode=" + parCode;
                    break;
                case "GP":
                    conx += "server=GreatPlains;cbtt=" + siteCode + ";pcode=" + parCode;
                    break;
                case "LC":
                    conx += "server=LCHDB2;sdi="+sdiCode+";timeinterval="+intervalCode;
                    break;
                case "UC":
                    conx += "server=UCHDB2;sdi=" + sdiCode + ";timeinterval=" + intervalCode;
                    break;
                case "MP":
                    conx += "server=MPSFTP;cbtt=" + siteCode + ";pcode=" + parCode;
                    break;

            }
            this.conxnStringLabel.Text = "RWIS Connection: " + conx;
        }

        /// <summary>
        /// Close Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseForm(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
